using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    // public float speed;
    // public float rotationSpeed;
    //
    // private Rigidbody _rb;
    // private float rotation;
    // private float translation;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     _rb = GetComponent<Rigidbody>();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    //     translation = Input.GetAxis("Vertical") * speed;
    //     rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
    //     
    //     _rb.velocity = transform.forward * translation;
    //     
    //     transform.Rotate(0, rotation, 0);
    //     
    //     
    // }
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public bool gameRunning = false;

    void Update () {
        if (gameRunning)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            
        }
        
    }
    // private float horizontalSpeed = 10.0f;
    // private float verticalSpeed = 10.0f;
    // public bool gameRunning = false;
    //
    // void Update()
    // {
    //     if (gameRunning)
    //     {
    //         // Get the mouse delta. This is not in the range -1...1
    //         float h = horizontalSpeed * Input.GetAxis("Mouse X");
    //         float v = verticalSpeed * Input.GetAxis("Mouse Y");
    //
    //         transform.Rotate(v, h, 0);
    //         
    //     }
    //
    // }
}
