using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAnswers : MonoBehaviour
{
	public GameObject SavingCanvas;
    public bool subjectFinalAnswer;
    private GameObject _questionDeactivator;
    private GameObject scriptManager;
    
    void Start()
	{
		SavingCanvas.SetActive(false);
		scriptManager = GameObject.Find("ScriptManager");
		_questionDeactivator = scriptManager.GetComponent<Countdowns>().QuestionCanvas;
	} 

    public void SaveCorrectAnswer()
    {
        subjectFinalAnswer = true;
        _questionDeactivator.SetActive(false);
        SavingCanvas.SetActive(true);
        string answer = "Yes ; " + Time.realtimeSinceStartup;
        scriptManager.GetComponent<Saving>().golemAnwser.Add(answer);

    }

    public void SaveWrongAnswer()
    {
        subjectFinalAnswer = false;
        _questionDeactivator.SetActive(false);
        SavingCanvas.SetActive(true);
        string answer = "Golem? ; " + Time.realtimeSinceStartup;
        scriptManager.GetComponent<Saving>().golemAnwser.Add(answer);
    }
}
