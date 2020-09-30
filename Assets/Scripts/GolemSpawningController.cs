using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSpawningController : MonoBehaviour
{
    
    public GameObject golem;

    public float spawnGolemTimer = 30f;
    public bool deactGolem = false;
    
    // Start is called before the first frame update
    void Start()
    {
        golem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        spawnGolemTimer -= Time.deltaTime;

        if (spawnGolemTimer < 0)
        {
            golem.SetActive(true);
        }

        if (deactGolem)
        {
            golem.SetActive(false);
        }
        
        
    }
}
