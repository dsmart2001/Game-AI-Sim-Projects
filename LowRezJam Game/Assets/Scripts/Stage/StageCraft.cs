using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCraft : MonoBehaviour
{
    public List<Stage> Stages;
    public int currentStageNumber = 0;
    public Stage currentStage;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Stage stage in Stages)
        {
            stage.gameObject.SetActive(false);
        }
    }

    // Set next stage
    public void NextStage()
    {
        Stages[currentStageNumber].gameObject.SetActive(false);
        currentStageNumber++;

        if(currentStageNumber > Stages.Count - 1)
        {
            currentStageNumber = 0;
        }

        currentStage = Stages[currentStageNumber];

        Stages[currentStageNumber].gameObject.SetActive(true);
        Debug.Log("SET NEW STAGE: " + currentStage);
    } 
}
