using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Debug = System.Diagnostics.Debug;

public class StartStop : MonoBehaviour
{
    public bool menuEnabled = true;
    public bool countdownEnabled = false;
    public bool gameRunning = false;
    public GameObject menuCanvas;
    public GameObject subjectIdCanvas;
    public GameObject taskCanvas;
    public TMP_InputField subjectIdText;
    public string SubjectID;
    public string savePath;
    public bool inputFieldActive = false;
    public GameObject inputFieldObj;
    const string PlayerNamePrefKey = "SubjectID";
	private bool subIDran = false;
	private AudioSource _buttonSound;
	private string folderPath = @"D:\Unity\Data\CountTheCatch\";
	private DirectoryInfo folder;
    
    // Start is called before the first frame update
    void Start()
    {
	    // Pausing the time before the start of the experiment 
        Time.timeScale = 0;
        
        // Finding the necessary Canvasses
        menuCanvas = GameObject.Find("Menu Canvas");
		menuCanvas.SetActive(false);
        subjectIdCanvas = GameObject.Find("SubID Canvas");
        subjectIdCanvas.SetActive(true);
        taskCanvas = GameObject.Find("Task Canvas");
        taskCanvas.SetActive(false);
        
        // Button Sound
        _buttonSound = GameObject.Find("Button Sound").GetComponent<AudioSource>();
        
		SubId();
		inputFieldActive = true;
    }
    
    public void StartGame() // Start Game Function activates the Countdown script and starts the time
    {
        taskCanvas.SetActive(false);
        ((Countdowns)gameObject.GetComponent<Countdowns>()).enabled = true;
        Time.timeScale = 1;
    }

	public void Update()
	{
		SubId();
	}


    public void SubId()
    {
        SubjectID = subjectIdText.text;

        if (!Input.GetKeyDown(KeyCode.Return) || subIDran != false) return;
       _buttonSound.Play();
        inputFieldActive = false;
        subjectIdCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        subIDran = true;
        // Debug.Log(SubjectID);
        folder = Directory.CreateDirectory(folderPath+SubjectID);
        savePath = folderPath +SubjectID;
    }

    public void TaskDescription()
    {
	    menuCanvas.SetActive(false);
	    taskCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
