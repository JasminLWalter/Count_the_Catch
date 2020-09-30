using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Rigidbody rb;
    public bool ballSpawned = false;
    private AudioSource _fallingSound;
    private AudioSource _startSound;

    // Start is called before the first frame update
    void Start()
    {
        //Sound Integration 
        _fallingSound = GameObject.Find("Falling Sound").GetComponent<AudioSource>();
        _startSound = GameObject.Find("Start Sound").GetComponent<AudioSource>();
    }

    public void SpawnBall() // Spawning the ball above the playground + Sounds
    {
        if (ballSpawned == false)
        {
            rb.transform.position = new Vector3(-0f, 17f, 2.5f);
            rb.useGravity = true;
            _fallingSound.Play();
            _startSound.Play();
        }

        ballSpawned = true;
    }
}
