using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCraft : MonoBehaviour
{
    public List<Stage> Stages;
    public int currentStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Stage stage in Stages)
        {
            stage.gameObject.SetActive(false);
        }
    }

    public void NextStage()
    {
        Stages[currentStage].gameObject.SetActive(false);
        currentStage++;

        if(currentStage > Stages.Count)
        {
            currentStage = 0;
        }

        Stages[currentStage].gameObject.SetActive(true);
    } 
}
