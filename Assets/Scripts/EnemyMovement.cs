using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{

    public Vector3 start_position;
    public Vector3 warp_position;
    public float wanderRadius = 50;
    public float wanderTimer = 1 ;

    public bool startSequenz = false;
    public bool defaultIsWalking = false;
    public bool catchedBall = false;
    public bool aiming = false;
    public bool throwing = false;
    public bool isNextPlayer = false;
    public bool isSearching = false;
    public bool endGame = false;
    
    public float aimingTime =1;

    public GameObject ball;
    public NavMeshAgent agent;
    private GameObject scriptM;
    private GameObject attachPoint;
    private GameObject newAimCP;
    private float wanderTime;
    public GameObject previousAim;
    public GameObject newAim;
    private GameObject[] targetList;
    private GameObject[] enemyList;
                       
    private Transform target;
    private int passCount;
    private GameObject nextPlayer;
    private bool firstStateD;
    private bool firstStateS;
    private float countdownE;
    private Vector3 middle = new Vector3(-150f, 2f, 2.5f);
    private float waitTime;
    private float defaultWaitTime = 10f;
    

    private void Start()
    {
        this.gameObject.SetActive(false);

        scriptM = GameObject.Find("ScriptManager");
        attachPoint = this.transform.Find("Attach Point").gameObject;
        ball = GameObject.Find("Ball");
        targetList = scriptM.GetComponent<CatchingThrowing>().enemyList;
        enemyList = scriptM.GetComponent<RobotEnemyManagement>().enemyList;
        // targetList = scriptM.GetComponent<EnemyManagement2>().enemyList;
        passCount = 0;
        waitTime = defaultWaitTime;
        // set first state either to default walking or searching depending on if is current player or not
        var currentPlayer = scriptM.GetComponent<CatchingThrowing>().currentPlayer;
        if (this.name == currentPlayer.name)
        {
           
            firstStateD = false;
            firstStateS = true;
        }
        else
        {          
            firstStateD = true;
            firstStateS = false;
            
        }
        // countdownE = scriptM.GetComponent<Countdowns>().countdownTime;


        
    }

    void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        // timer = wanderTimer;
        wanderTime = wanderTimer;
        startSequenz = true;
        agent.Warp(warp_position);
        
    }
 

    // Update is called once per frame
    void Update()
    {

        if (startSequenz)
        {
            agent.SetDestination(start_position);
            // agent.ResetPath();
            countdownE = scriptM.GetComponent<Countdowns>().countdownTime;

            
            if (countdownE < 0)
            {
                startSequenz = false;
                defaultIsWalking = firstStateD;
                isSearching = firstStateS;

            }
            else if (agent.hasPath == false)
            {
                agent.transform.LookAt(middle);
            }
        }
        
        else if (endGame)
        {
            agent.ResetPath();
            var up = transform.position;
            up.y = 10;
            agent.transform.LookAt(up);
        }
        
        // if ball is catched
        else if (catchedBall)
        {
            // enable default behaviour of previous next Player if it is not identical to this enemy
            
            if (previousAim.name != this.name)
            {
                previousAim.GetComponent<EnemyMovement>().defaultIsWalking = true;

            }

            // then stop movement an enable aiming process
            agent.ResetPath();
            catchedBall = false;
            aiming = true;
             
        }
        else if (aiming)
        {
            // get new aim and save it
            newAim = SelectPlayer();
            
            // rotate towards next player
            agent.transform.LookAt(newAim.transform);
            
            aiming = false;   
            throwing = true;


        }
        else if (throwing)
        {
            // start throwing timer
            aimingTime -= Time.deltaTime;
            if (aimingTime < 0)
            {
                GameObject.Find("Throwing Sound Cube").GetComponent<AudioSource>().Play();
                if (newAim.CompareTag("Enemy"))
                {
                    newAimCP = newAim;
                }
                else
                {
                    newAimCP = newAim.GetComponent<RobotEnemyMovement>().catchPoint;
                }
                Vector3 direction = CalculateVelocity(newAim.transform.position, this.transform.position, 1f);
                ball.transform.parent = null;
                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Rigidbody>().velocity = direction;
                throwing = false;

                defaultIsWalking = true;
                wanderTime = wanderTimer;
                aimingTime = 0.5f;
                string info = Time.realtimeSinceStartup + " ; pass ; box-robot";
                scriptM.GetComponent<Saving>().passCountList.Add(info);
            }
        }
        
        else if (isNextPlayer)
        {
            waitTime -= Time.deltaTime;
            if (waitTime < 0)
            {
                defaultIsWalking = true;
                isNextPlayer = false;
                waitTime = defaultWaitTime;
            }
            else
            {
                agent.ResetPath(); 
                agent.transform.LookAt(ball.transform);
                defaultIsWalking = false;

                
            }
            
        }

        // if ball is on ground - searching behaviour
        else if(isSearching)
        {
            //transform.LookAt(ball.transform);
            agent.SetDestination(ball.transform.position);
        }
        
        // default behaviour
        else if (defaultIsWalking & wanderTime >= wanderTimer) 
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTime = 0;
        }
        
        else if (defaultIsWalking)
        {
            wanderTime += Time.deltaTime;
        }

        else
        {
            Debug.Log("Error, enemy behaviour fail ");
        }
        


    }
    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
     
        randDirection += origin;
     
        NavMeshHit navHit;
     
        UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
     
        return navHit.position;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.position = attachPoint.transform.position;
            other.gameObject.transform.parent = attachPoint.transform;

            scriptM.GetComponent<CatchingThrowing>().isOnGround = false;
            // agent.isStopped = true;
            isNextPlayer = false;
            defaultIsWalking = false;
            catchedBall = true;
            isSearching = false; 
            scriptM.GetComponent<CatchingThrowing>().currentPlayer = this.gameObject;
            
            // check whether ball has hit aim
            previousAim = scriptM.GetComponent<CatchingThrowing>().nextPlayer;
            if (previousAim.name != this.name)
            {
                previousAim.GetComponent<EnemyMovement>().isNextPlayer = false;
                previousAim.GetComponent<EnemyMovement>().isSearching = false;
                previousAim.GetComponent<EnemyMovement>().defaultIsWalking = true;     
                
            }
            



        }
    }
    
    
    public Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        // define distances
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;
        
        // create floats

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;


    }
    public GameObject SelectPlayer()
    {
        passCount = scriptM.GetComponent<CatchingThrowing>().passCount;
        if (passCount > 5)
        {
            nextPlayer = enemyList[Random.Range(0, enemyList.Length)];
            scriptM.GetComponent<CatchingThrowing>().passCount = 0;
            // inform new next player - set condition to active
            nextPlayer.GetComponent<RobotEnemyMovement>().isNextPlayer = true;
        }
        else
        {
            nextPlayer = targetList[Random.Range(0, targetList.Length)];
            while (nextPlayer.gameObject.name == this.name)
            {
                nextPlayer = targetList[Random.Range(0, targetList.Length)];
            }
            scriptM.GetComponent<CatchingThrowing>().passCount ++;
            nextPlayer.GetComponent<EnemyMovement>().isNextPlayer = true;
        }
        
       

        return nextPlayer;
    }
    
    
}




