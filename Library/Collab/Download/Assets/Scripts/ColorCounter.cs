using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCounter : MonoBehaviour
{
    private int red_counter_sub = 0; // the counter increased by the subject
    private int blue_counter_sub = 0;
    public static int red_scorevalue = 0;
    public static int blue_scorevalue = 0;
    public Text red_score;
    public Text blue_score;
	private float timer = 0f;
	public float current_time;
    
    
    // Start is called before the first frame update
    void Start()
    {
        red_score = GameObject.Find("Red Text").GetComponent<Text>();
        red_score.enabled = false;
        blue_score = GameObject.Find("Blue Text").GetComponent<Text>();
        blue_score.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        SubCounter();
		timer =+ Time.deltaTime;
    }

    void SubCounter()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            red_counter_sub++;
            red_score.text = "R/Red: " + red_counter_sub;
			current_time = timer;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            blue_counter_sub++;
            blue_score.text = "L/Blue: " + blue_counter_sub;
			current_time = timer;
        }
    }

    public void ActivateColorCount()
    {
        red_score.enabled = true;
        blue_score.enabled = true;
    }
    
}
