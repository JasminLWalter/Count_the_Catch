using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class BallColorSwitch : MonoBehaviour
{

	public GameObject ball;
	public int redCounter = 0; // the actual counter for the color switch 
	public int blueCounter = 0;
	public bool gameEnded = false;
	public bool countdown_over = false;
	public float current_time_sub;
	
	private ParticleSystem _particleSystem;
	private bool activateColors;
	private AudioSource _firework;
	private bool _currentlyRed = true;
	private bool _currentlyBlue = false;
	private float timer = 0f;
	private float random_time;
	private GameObject scriptManager;
	private string savePath;
	private List<string> colorChangeList = new List<string>();




	// Start is called before the first frame update
	void Start()
	{
		Invoke("ColorSwitch", 0.1f);
		
		//Particle System Integration 
		_particleSystem = ball.GetComponent<ParticleSystem>();
		var main = _particleSystem.main;
		main.loop = false;
		_particleSystem.Stop();
		
		//Sound Integration 
		_firework = GameObject.Find("Firework Sound").GetComponent<AudioSource>();
		scriptManager = GameObject.Find("ScriptManager");
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (gameEnded)
		{
			savePath = scriptManager.GetComponent<StartStop>().savePath;
			using (StreamWriter writer = new StreamWriter(savePath +  @"\ColourChangeData.txt"))
			{
				// added a row with column names for better overview of the data file
				string description = "Time ; Colour";
				writer.WriteLine(description);

				foreach (var value in colorChangeList)
				{
					writer.WriteLine(value);
				}
			}
			
		}
	}

	public void ColorSwitch()
	{
		_particleSystem.Play();
		_firework.Play();
		// random time between 1 and 5 seconds for the color switch
		float randomTime = Random.Range(1f, 5f);

		// if red change to blue
		if (_currentlyRed)
		{
			ball.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
			string info = Time.realtimeSinceStartup + " ; blue";
			colorChangeList.Add(info);
			_currentlyBlue = true;
			_currentlyRed = false;

			redCounter++;
			current_time_sub = timer;
		}
		// if blue change to red
		else if (_currentlyBlue)
		{
			ball.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
			string info = Time.realtimeSinceStartup + " ; red";
			colorChangeList.Add(info);
			_currentlyBlue = false;
			_currentlyRed = true;

			blueCounter++;
			current_time_sub = timer;
		}

		// invoke the function again after random time interval
		Invoke("ColorSwitch", randomTime);
	}


public void StartColorSwitch()
    {
	    countdown_over = true;
	    redCounter = 0;
	    blueCounter = 0;
    }
}
