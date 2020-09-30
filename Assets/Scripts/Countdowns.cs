using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class Countdowns : MonoBehaviour
{
    private float _timeRemaining = 60; // Game Time
    public float countdownTime = 10; // Countdown Time before Game Start
    public bool timerRunning = false;
    public bool countdownRunning = true;
	private bool _gameOver = false;
	private bool _gameOverTrigger = false;
    private Text _remainingTimeText;
    private Text _countdownText;
	private Text _gameOverText;
	private AudioSource _throwingSoundRobot;
	private AudioSource _throwingSoundCube;
	private AudioSource _fireworkSound;
	private AudioSource _fallingSound;
	private AudioSource _startSound;
	private AudioSource _buttonSound;
	public GameObject QuestionCanvas;
    public ColorCounter ColorCountActivator;
    public BallSpawner BallSpawnActivator;
    public BallColorSwitch ColorSwitchActivator;
	private RectTransform rectTransform;
	private Vector3 GameOverStartPosition = new Vector3(-170f,80f,9999f);
	private Vector3 GameOverEndPosition = new Vector3(-170f,80f,-850f);

	private GameObject startZoneR;
	private GameObject[] robotList;
	private GameObject[] boxPlayerList;
	private GameObject scriptM;
	private GameObject ball;
	private GameObject player;
    

    
    void Start()
    {
	    startZoneR = GameObject.Find("StartZoneRobots");
	    countdownRunning = true;
	    
	    // Deactivating the countdown script until the game starts
		((Countdowns)gameObject.GetComponent<Countdowns>()).enabled = false;

		//Texts and Canvas
		_remainingTimeText = GameObject.Find("GameCountdown").GetComponent<Text>();
		_remainingTimeText.enabled = false;
		_countdownText = GameObject.Find("StartCountdown").GetComponent<Text>();
		_countdownText.enabled = false;
		_gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
		_gameOverText.enabled = false;
		QuestionCanvas.SetActive(false);
		rectTransform = _gameOverText.GetComponent<RectTransform>();
		rectTransform.localPosition = GameOverStartPosition;
		
		//Sound Integration 
		_throwingSoundRobot = GameObject.Find("Throwing Sound Robot").GetComponent<AudioSource>();
		_throwingSoundCube = GameObject.Find("Throwing Sound Cube").GetComponent<AudioSource>();  
		_fireworkSound  = GameObject.Find("Firework Sound").GetComponent<AudioSource>();
		_fallingSound = GameObject.Find("Falling Sound").GetComponent<AudioSource>();
		_startSound =  GameObject.Find("Start Sound").GetComponent<AudioSource>();
		_buttonSound = GameObject.Find("Button Sound").GetComponent<AudioSource>();
		
		// Script Integration 
		ColorCountActivator = FindObjectOfType<ColorCounter>();
		BallSpawnActivator = FindObjectOfType<BallSpawner>();
		ColorSwitchActivator = FindObjectOfType<BallColorSwitch>();
		
		// get lists for start sequence
		scriptM = GameObject.Find("ScriptManager");
		boxPlayerList= scriptM.GetComponent<CatchingThrowing>().enemyList;
		robotList = scriptM.GetComponent<RobotEnemyManagement>().enemyList;
		// find ball and player game object
		ball = GameObject.Find("Ball");
		player = GameObject.Find("Player");

    }
    
    void Update()
    {
        StartCountdown();
        RemainingTime();
        GameEnd();
		GameOverMove();
    }

    public void StartCountdown()
    {
	    if ((countdownRunning == true) && (timerRunning == false) && (countdownTime > 0))
	    {
		    _countdownText.enabled = true;
		    countdownTime -= Time.deltaTime;
		    DisplayTime(countdownTime);

		    if (countdownTime > 9 && countdownTime < 10)
		    {
			    player.GetComponent<PlayerControllerRB>().gameRunning = true;
			    boxPlayerList[0].gameObject.SetActive(true);
			    robotList[0].gameObject.SetActive(true);
		    }
		    else if (countdownTime > 8 && countdownTime < 9)
		    {
			    boxPlayerList[1].gameObject.SetActive(true);
			    robotList[1].gameObject.SetActive(true);
		    }
		    else if (countdownTime > 7 && countdownTime < 8)
		    {
			    boxPlayerList[2].gameObject.SetActive(true);
			    robotList[2].gameObject.SetActive(true);
		    }
		    else if (countdownTime > 6 && countdownTime < 7)
		    {
			    boxPlayerList[3].gameObject.SetActive(true);
			    robotList[3].gameObject.SetActive(true);
		    }
		    else if (countdownTime > 5 && countdownTime < 6)
		    {
			    boxPlayerList[4].gameObject.SetActive(true);
			    robotList[4].gameObject.SetActive(true);
		    }
		    else if (countdownTime > 4 && countdownTime < 5)
		    {
			    boxPlayerList[5].gameObject.SetActive(true);
			    robotList[5].gameObject.SetActive(true);
		    }
	    }
	    
	    else if (countdownTime < 0)
        {
            timerRunning = true;
			countdownRunning = false;
            _countdownText.enabled = false;
            countdownTime = 0;
            ColorCountActivator.ActivateColorCount(); // Calling the ColorCounter script to activate the counter
            BallSpawnActivator.SpawnBall();
            ColorSwitchActivator.StartColorSwitch();
            startZoneR.SetActive(false);
            
            _throwingSoundRobot.mute = false;
            _throwingSoundCube.mute = false;
            _fireworkSound.mute = false;
            _fallingSound.mute = false;
            _startSound.mute = false;
        }
    }

    public void RemainingTime()
    {
        if ((_timeRemaining > 0) && (timerRunning == true))
        {
            _remainingTimeText.enabled = true;
            _timeRemaining -= Time.deltaTime;
            DisplayTime(_timeRemaining);
        }
        else
        {
            timerRunning = false;
            _remainingTimeText.enabled = false;
			_gameOverTrigger = true;
        }
    }

    public void GameEnd()
    {
        if ((timerRunning == false) && (countdownRunning == false) && (_gameOver == false) && (_gameOverTrigger == true))
        {
			_gameOverText.enabled = true;
			_remainingTimeText.enabled = false;
			_gameOver = true;
			
			// initiate end sequence of ball players
			foreach (var robot in robotList)
			{
				robot.GetComponent<RobotEnemyMovement>().endGame = true;
			}
			foreach (var boxPlayer in boxPlayerList)
			{
				boxPlayer.GetComponent<EnemyMovement>().endGame = true;
			}

			ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

	public void GameOverMove()
	{
		if((_gameOver == true) && (rectTransform.localPosition != GameOverEndPosition))
		{
		rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition , GameOverEndPosition, Time.deltaTime * 5000);
		}
		else if(rectTransform.localPosition == GameOverEndPosition)
		{
		QuestionCanvas.SetActive(true);
		Time.timeScale = 0;
		player.GetComponent<PlayerControllerRB>().gameRunning = false;

		((Countdowns)gameObject.GetComponent<Countdowns>()).enabled = false; // Disabled the Countdown script to be able to disable the questioncanvas - not beautiful 
		}	
	}


	public void PlayButtonSound()
	{
		_buttonSound.Play();
	}


    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float seconds90 = Mathf.FloorToInt(timeToDisplay);
        float seconds10 = Mathf.FloorToInt(timeToDisplay);
        _remainingTimeText.text = string.Format("{00}", seconds90);
        _countdownText.text = string.Format("{00}", seconds10);
    }

}
