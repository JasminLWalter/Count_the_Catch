using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGorilla : MonoBehaviour
{
    private float speed = 2.0f;
    private float timer = 0.0f;
    private float rotationspeed = 360.0f;
    private Vector3 middle_target = new Vector3(0f, 1.5f, 7.5f);
    private Vector3 end_target = new Vector3(16f, 1.5f, 7.5f);


    // Update is called once per frame
    void Update()
    {
        MoveToMiddle();
        Hold();
        MoveToTarget();
    }

    public void MoveToMiddle()
    {
        if (transform.position.x <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, middle_target, Time.deltaTime * speed);
        }
    }

    public void MoveToTarget()
    {
        if (transform.position.x > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, end_target, Time.deltaTime * speed);
        }
    }

    public void Hold()
    {
        if (transform.position == middle_target)
        {
            timer += Time.deltaTime;
            transform.Rotate(new Vector3(0f, rotationspeed*Time.deltaTime, 0f)); // actually trigger winning animation
        }

        if (timer >= 5)
        {
            transform.position = new Vector3(0.1f,1.5f,7.5f);
			transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            timer = 0; 
        }
    }
}
