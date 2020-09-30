using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptQuestionCanvas : MonoBehaviour
{

    public bool displayQuestionCanvas = false;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (displayQuestionCanvas)
        {
            this.gameObject.SetActive(true);
        }
        
    }
}
