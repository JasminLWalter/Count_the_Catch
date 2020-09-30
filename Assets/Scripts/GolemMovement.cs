using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GolemMovement : MonoBehaviour
{
    private Vector3 start_position = new Vector3(-16f, 0.64f, 0f );

    private Vector3 middle_target = new Vector3(0f, 1.5f, 0f);
    private Vector3 end_target = new Vector3(16f, 1.5f, 0f);
    private Animator _animator;
    public NavMeshAgent agent;
    // private Rigidbody rb;
    public bool walk;
    private GameObject player;
    public float dist;
    public float dist2;
    public bool firstWalk;
    public bool endWalk = false;
    public bool doCelebration = false;
    public float celebrationTimer = 5;
    private GameObject scriptM;

    private void Start()
    {
        scriptM = GameObject.Find("ScriptManager");
        dist2 = 10f;
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent> ();
        agent.Warp(start_position);
        firstWalk = true;



    }

    // Update is called once per frame
    void Update()
    {
        if (firstWalk)
        {
            agent.SetDestination(middle_target);
            _animator.SetBool("walk",true);
            // agent.SetDestination(ball.transform.position);

            dist = Vector3.Distance(middle_target,transform.position);
            if (dist < 2)
            {
                firstWalk = false;
                doCelebration = true;
            }

        }

        // if middle has been reached, trigger celebrating

        else if (doCelebration)
        {
            dist = Vector3.Distance(transform.position,end_target);
            agent.ResetPath();
            _animator.SetBool("walk",false);
            agent.transform.LookAt(player.transform);
            _animator.SetTrigger("celebrate");
            doCelebration = false;
            endWalk = true;
        }

        else if (endWalk)
        {
            celebrationTimer -= Time.deltaTime;
            dist2 = Vector3.Distance(transform.position,end_target);
            
            if (dist2 < 2)
            {
                scriptM.GetComponent<GolemSpawningController>().deactGolem = true;
            }
            
            else if (celebrationTimer < 0)
            {
                agent.SetDestination(end_target);
                _animator.SetBool("walk",true);
            }

            
        }

        

        // _animator.SetBool("walk",true);

        // if (testi)
        // {
        //     _animator.SetBool("walk",true);
        // }
        
    }

    // public void MoveToMiddle()
    // {
            // if (transform.position.x <= 0)
            // {
            //     transform.position = Vector3.MoveTowards(transform.position, middle_target, Time.deltaTime * speed);
            //     walk = true;
            // }
    //
    // }
    //
    // public void MoveToTarget()
    // {
    //     if (transform.position.x > 0)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, end_target, Time.deltaTime * speed);
    //     }
    // }

    // public void Hold()
    // {
    //     
    // }
}
