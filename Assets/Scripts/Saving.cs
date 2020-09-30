using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
public class Saving : MonoBehaviour // eigentlich static aber dunno
{
    
    public List<string> passCountList = new List<string>();
    public List<string> colourCountList = new List<string>();
    public List<string> golemAnwser = new List<string>();

    public GameObject scriptManager;
    private string savePath;


    public void saveData()
    {
        // enable saving in other scripts
        scriptManager.GetComponent<BallColorSwitch>().gameEnded = true;
        
        // save pass data gathered here
        savePath = scriptManager.GetComponent<StartStop>().savePath;
        using (StreamWriter writer = new StreamWriter(savePath +  @"\PassCountData.txt"))
        {
            // added a row with column names for better overview of the data file
            string description = "Time ; Pass ; ThrowingRobot";
            writer.WriteLine(description);

            foreach (var value in passCountList)
            {
                writer.WriteLine(value);
            }
        }
        
        using (StreamWriter writer = new StreamWriter(savePath +  @"\ColourCountData.txt"))
        {
            // added a row with column names for better overview of the data file
            string description = "Time ; ParticipantAnswer";
            writer.WriteLine(description);

            foreach (var value in colourCountList)
            {
                writer.WriteLine(value);
            }
        }
        
        using (StreamWriter writer = new StreamWriter(savePath +  @"\GolemAnswer.txt"))
        {
            // added a row with column names for better overview of the data file
            string description = "GolemAnswer ; Time";
            writer.WriteLine(description);

            foreach (var value in golemAnwser)
            {
                writer.WriteLine(value);
            }
        }
        
        Application.Quit();

        // put everything together and save
    }

    // public void discardData()
    // {
    //     //Debug.Log("Discard and Quit");
    //     // if no saving is wanted, just stop the game
    // }
}
