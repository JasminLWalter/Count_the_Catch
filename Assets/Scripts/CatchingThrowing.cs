using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CatchingThrowing : MonoBehaviour
{
    public GameObject[] enemyList;
    public GameObject ball;
    public GameObject currentPlayer;
    public GameObject nextPlayer;

    public bool isCaught = false;
    public bool targetSelected = false;
    public bool isOnGround;

    public GameObject[] nextPlayerList;

    public int passCount;
    //private bool isflying;

    // private float throwTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // currentPlayer.GetComponent<EnemyMovement>().defaultIsWalking = false;
        // currentPlayer.GetComponent<EnemyMovement>().isSearching = true;
        passCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (isOnGround)
        {
            PickUpBall();
        }
        /*
        else if(isCaught)
        {
            FaceNextTarget();
            
        }
        */
        // else if (isCaught)
        // {
        //     GameObject nextPlayer = SelectPlayer();
        //     // throwTime -= Time.deltaTime;
        //     // if (throwTime < 0)
        //     // {
        //     //     ThrowBall();
        //     // }
        // }
        //
    }

    /*
    public void FaceNextTarget()
    {
        GameObject nextPlayer = SelectPlayer();
        
        nextPlayer.GetComponent<EnemyMovement>().isStandingFree = false;
        currentPlayer.GetComponent<NavMeshAgent>().isStopped = true;
        nextPlayer.GetComponent<NavMeshAgent>().isStopped = true;


       // currentPlayer.GetComponent<Transform>().LookAt(nextPlayer.GetComponent<Transform>());
        // nextPlayer.GetComponent<Transform>().LookAt(ball.GetComponent<Transform>());
        targetSelected = true;
        isCaught = false;
    }
*/
    public void PickUpBall()
    {
        nextPlayer.GetComponent<EnemyMovement>().isSearching = true;

    }

 

    
    
}
