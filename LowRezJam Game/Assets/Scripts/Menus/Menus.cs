using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public GameObject GameplayObject;
    public GameObject MenuObject;
    public GameObject SettingsObject;

    public List<GameObject> HideAtStart;
    public List<GameObject> ShowAtStart;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject menu in HideAtStart)
        {
            menu.SetActive(false);
        }

        foreach(GameObject menu in ShowAtStart)
        {
            menu.SetActive(true);
        }
    }

    public void StartGame()
    {
        MenuObject.SetActive(false);
        GameplayObject.SetActive(true);
    }

    public void ChangeMode(bool TwoPlayer)
    {
        switch (TwoPlayer)
        {
            case true:

                break;
            case false:

                break;
        }
    }

    public void Settings(bool open)
    {
        switch (open)
        {
            case true:
                SettingsObject.SetActive(true);
                Time.timeScale = 0;
                break;
            case false:
                SettingsObject.SetActive(false);
                Time.timeScale = 1;
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
