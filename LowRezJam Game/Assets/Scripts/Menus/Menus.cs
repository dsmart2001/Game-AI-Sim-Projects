using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    private GameManager GM => GetComponent<GameManager>();

    // Menu Objects
    public GameObject GameplayObject;
    public GameObject MenuObject;
    public GameObject SettingsObject;
    public GameObject ControlsObject;
    public GameObject SetupPlayObject;

    public GameObject KeyboardControls;
    public GameObject ControllerControls;

    // Menu lists
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
        SetupPlayObject.SetActive(false);

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
                break;
            case false:
                SettingsObject.SetActive(false);
                break;
        }
    }

    public void Controls(bool open)
    {
        switch (open)
        {
            case true:
                ControlsObject.SetActive(true);
                break;
            case false:
                ControlsObject.SetActive(false);
                break;
        }
    }

    public void ControlTypesInfo(bool gamepad)
    {
        switch (gamepad)
        {
            case true:
                KeyboardControls.SetActive(false);
                ControllerControls.SetActive(true);
                break;
            case false:
                KeyboardControls.SetActive(true);
                ControllerControls.SetActive(false); 
                break;
        }
    }

    public void SetupPlay(bool open)
    {
        MenuObject.SetActive(!open);
        SetupPlayObject.SetActive(open);
    }

    public void InputTypes(int inputVar)
    {
        // 0 = Split Keyboard      
        // 1 = Keyboard vs gamepad           
        // 2 = Gamepad vs gamepad     
        // 3 = Gamepad vs Keyboard  

        GM.SetupInputs(inputVar);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
