using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Menus : MonoBehaviour
{
    private GameManager GM => GetComponent<GameManager>();

    // Menu Objects
    public GameObject GameplayObject;
    public GameObject MenuObject;
    public GameObject SettingsObject;
    public GameObject ControlsObject;
    public GameObject SetupPlayObject;
    public GameObject AudioObject;

    public GameObject KeyboardControls;
    public GameObject ControllerControls;

    // Menu lists
    public List<GameObject> HideAtStart;
    public List<GameObject> ShowAtStart;


    public AudioMixer audio;
    public Slider musicSlider;
    public Slider playerSlider;

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

        playerSlider.value = PlayerPrefs.GetFloat("PlayerVal", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVal", 0.75f);

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
        SettingsObject.SetActive(open);
    }

    public void Controls(bool open)
    {
        ControlsObject.SetActive(open);

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

    public void Audio(bool open)
    {
        AudioObject.SetActive(open);
    }

    public void ChangeVolumeMusic()
    {
        
        audio.SetFloat("MusicVal", Mathf.Log10(musicSlider.value) * 20);
    }

    public void ChangeVolumePlayer()
    {
        audio.SetFloat("PlayerVal", Mathf.Log10(playerSlider.value) * 20);
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
