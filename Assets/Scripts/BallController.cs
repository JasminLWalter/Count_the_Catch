using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameObject scriptMa;
    // Start is called before the first frame update
    void Start()
    {
        scriptMa = GameObject.Find("ScriptManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            scriptMa.GetComponent<CatchingThrowing>().isOnGround = true;
            scriptMa.GetComponent<RobotEnemyManagement>().isOnGround = true;
        }
    }
}
