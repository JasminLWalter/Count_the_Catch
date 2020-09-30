using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAnswers : MonoBehaviour
{
	public GameObject SavingCanvas;
    public bool subjectFinalAnswer;
    private GameObject _questionDeactivator;
    
    void Start()
	{
		SavingCanvas.SetActive(false);
		_questionDeactivator = GameObject.Find("ScriptManager").GetComponent<Countdowns>().QuestionCanvas;
	} 

    public void SaveCorrectAnswer()
    {
        subjectFinalAnswer = true;
        _questionDeactivator.SetActive(false);
        SavingCanvas.SetActive(true);
    }

    public void SaveWrongAnswer()
    {
        subjectFinalAnswer = false;
        _questionDeactivator.SetActive(false);
        SavingCanvas.SetActive(true);
    }
}
