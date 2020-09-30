using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class RobotEnemyMovement : MonoBehaviour
{
    public Vector3 start_position;
    public Vector3 warp_position;
    public float wanderRadius = 50;
    public float wanderTimer =1;

    public bool startSequenz = false;
    public bool defaultIsWalking = false;
    public bool catchedBall = false;
    public bool aiming = false;
    public bool throwing = false;
    public bool isNextPlayer = false;
    public bool isSearching = false;
    public bool finishThrowMovement = false;
    public bool endGame = false;

    
    private float aimingTime;
    public float defaultAimingTime = 1;

    public GameObject ball;
    public NavMeshAgent agent;
    private GameObject scriptM;
    private GameObject attachPoint;
    private float wanderTime;
    public GameObject previousAim;
    public GameObject newAim;
    private GameObject[] targetList;
                       
    private Transform target;
    public float speedVelo;
    public Vector3 speedVec;
    private Animator _animator;
    private GameObject newAimCP; 
    public GameObject catchPoint;
    private GameObject currentPlayer;

    public float throwMoveTime;
    public float defaultThrowMoveTime;
    public float startDist;

    public float defaultWaitTime = 10f;
    private float waitTime;
    private bool firstStateD;
    private bool firstStateS;
    private float countdown;
    private GameObject[] enemyList;
    private int passCount;
    private GameObject nextPlayer;
    private ParticleSystem holdRay;


    private void Start()
    {
        this.gameObject.SetActive(false);
        scriptM = GameObject.Find("ScriptManager");
        attachPoint = this.transform.Find("Attach Point").gameObject;
        catchPoint = this.transform.Find("Catch Point").gameObject;
        ball = GameObject.Find("Ball");
        targetList = scriptM.GetComponent<RobotEnemyManagement>().enemyList;
        enemyList = scriptM.GetComponent<CatchingThrowing>().enemyList;
        // targetList = scriptM.GetComponent<EnemyManagement2>().enemyList;
        passCount = 0;
        aimingTime = defaultAimingTime;
        waitTime = defaultWaitTime;


        // set first state either to default walking or searching depending on if is current player or not
        var currentPlayer = scriptM.GetComponent<RobotEnemyManagement>().currentPlayer;
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

    }

    void OnEnable () {
        
        holdRay = GetComponentInChildren<ParticleSystem>();
        holdRay.Stop(); 
        agent = GetComponent<NavMeshAgent> ();
        _animator = GetComponent<Animator>();
        // timer = wanderTimer;
        wanderTime = wanderTimer;
        agent.Warp(warp_position);
        throwMoveTime = defaultThrowMoveTime;
        agent.SetDestination(start_position);
    }
 

    // Update is called once per frame
    void Update()
    {
        speedVec= agent.velocity;
        speedVelo = Mathf.Abs(speedVec.x) + Mathf.Abs(speedVec.y) + Mathf.Abs(speedVec.z);
        _animator.SetFloat("walkingSpeed",speedVelo);

        if (startSequenz)
        {
            agent.ResetPath();
            _animator.SetBool("startPosReached",true);
            countdown -= Time.deltaTime;
            
            if (countdown < 0)
            {
                startSequenz = false;
                defaultIsWalking = firstStateD;
                isSearching = firstStateS;
            }

        }
        else if (endGame)
        {
            agent.ResetPath();
            _animator.SetBool("gameEnded",true);

        }

        // if ball is catched
        else if (catchedBall)
        {
            // enable default behaviour of previous next Player if it is not identical to this enemy
            if (previousAim.name != this.name)
            {
                previousAim.GetComponent<RobotEnemyMovement>().defaultIsWalking = true;

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
                GameObject.Find("Throwing Sound Robot").GetComponent<AudioSource>().Play();
                // Debug.Log("start throwing motion");
                _animator.SetTrigger("throwing");
                if (newAim.CompareTag("Enemy"))
                {
                    newAimCP = newAim;
                }
                else
                {
                    newAimCP = newAim.GetComponent<RobotEnemyMovement>().catchPoint;
                    
                }
                
                Vector3 direction = CalculateVelocity(newAimCP.transform.position, this.transform.position, 1f);
                ball.transform.parent = null;
                ball.GetComponent<Rigidbody>().isKinematic = false;
                ball.GetComponent<Rigidbody>().velocity = direction;
                throwing = false;
                finishThrowMovement = true;
                holdRay.Stop();
                string info = Time.realtimeSinceStartup + "; pass ; sphere-robot";
                scriptM.GetComponent<Saving>().passCountList.Add(info);

            }
            

            // else if (aimingTime < 0)
            // {

            //         
            // }
        }
        
        else if (finishThrowMovement)
        {
            throwMoveTime -= Time.deltaTime;
            if (throwMoveTime < 0)
            {
                defaultIsWalking = true;
                wanderTime = wanderTimer;
                aimingTime = defaultAimingTime;
                throwMoveTime = defaultThrowMoveTime;
                finishThrowMovement = false;

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
            
                // agent.transform.LookAt(ball.transform);
                currentPlayer = scriptM.GetComponent<RobotEnemyManagement>().currentPlayer;
                agent.transform.LookAt(currentPlayer.transform);
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


    }
    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
     
        randDirection += origin;
     
        NavMeshHit navHit;
     
        UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
     
        return navHit.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // Debug.Log("Collision",attachPoint.transform);
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.position = attachPoint.transform.position;
            other.gameObject.transform.parent = attachPoint.transform;
            holdRay.Play();

            scriptM.GetComponent<RobotEnemyManagement>().isOnGround = false;
            // agent.isStopped = true;
            isNextPlayer = false;
            defaultIsWalking = false;
            catchedBall = true;
            isSearching = false; 
            scriptM.GetComponent<RobotEnemyManagement>().currentPlayer = this.gameObject;
            
            // check whether ball has hit aim
            previousAim = scriptM.GetComponent<RobotEnemyManagement>().nextPlayer;
            if (previousAim.name != this.name)
            {
                previousAim.GetComponent<RobotEnemyMovement>().isNextPlayer = false;
                previousAim.GetComponent<RobotEnemyMovement>().isSearching = false;
                previousAim.GetComponent<RobotEnemyMovement>().defaultIsWalking = true;     
                
            }
            

        }
        else if (other.CompareTag("startZone"))
        {
            startSequenz = true;
            countdown = scriptM.GetComponent<Countdowns>().countdownTime;

            
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
        
        passCount = scriptM.GetComponent<RobotEnemyManagement>().passCount;
        if (passCount > 5)
        {
            nextPlayer = enemyList[Random.Range(0, enemyList.Length)];
            scriptM.GetComponent<RobotEnemyManagement>().passCount = 0;
            // inform new next player - set condition to active
            nextPlayer.GetComponent<EnemyMovement>().isNextPlayer = true;
        }
        else
        {
            nextPlayer = targetList[Random.Range(0, targetList.Length)];
            while (nextPlayer.gameObject.name == this.name)
            {
                nextPlayer = targetList[Random.Range(0, targetList.Length)];
            }
            scriptM.GetComponent<RobotEnemyManagement>().passCount ++;
            nextPlayer.GetComponent<RobotEnemyMovement>().isNextPlayer = true;
        }
        
        return nextPlayer;
        
    }

    private void FixedUpdate()
    {
        // if (isNextPlayer)
        // {
        //     currentPlayer = scriptM.GetComponent<RobotEnemyManagement>().currentPlayer;
        //
        //     // rotate towards next player
        //     var neededRotation = Quaternion.LookRotation(currentPlayer.transform.position - this.transform.position);
        //     agent.transform.rotation = Quaternion.Slerp(this.transform.rotation, neededRotation, Time.deltaTime*3);
        // // }
        // else if (aiming)
        // {
        //     // agent.transform.LookAt(newAim.transform);
        //     var neededRotation = Quaternion.LookRotation(newAim.transform.position - this.transform.position);
        //     agent.transform.rotation = Quaternion.Slerp(this.transform.rotation, neededRotation, Time.deltaTime*10);
        // }
    }
}

