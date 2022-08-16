using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    // Components
    public PlayerInputManager inputManager => GetComponent<PlayerInputManager>();
    private StageCraft stageCraft => GetComponent<StageCraft>();
    private Menus menus => GetComponent<Menus>();
    private GUI gui => FindObjectOfType<GUI>();
    private InputUser inputUser;

    // Player Variables
    public Transform playerParent;
    public List<Player> ActivePlayers;

    public static PlayerInput player1;
    public GameObject player1Prefab;
    public static PlayerInput player2;
    public GameObject player2Prefab;

    public Vector3 playerScale;

    public int winScore = 10;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        //player1.SwitchCurrentControlScheme("KBM 1", Keyboard.current);
        //player2.SwitchCurrentControlScheme("KBM 2", Keyboard.current);

        StartGame();
        
    }

    public void StartGame()
    {
        // Instantiate and set players
        player1 = PlayerInput.Instantiate(player1Prefab, controlScheme: "KBM 1", pairWithDevice: Keyboard.current);
        player2 = PlayerInput.Instantiate(player2Prefab, controlScheme: "KBM 2", pairWithDevice: Keyboard.current);

        ActivePlayers.Add(player1.GetComponent<Player>()); 
        ActivePlayers.Add(player2.GetComponent<Player>()); 

        // Set player positions and transform parent
        foreach (Player i in ActivePlayers)
        {
            i.gameObject.transform.SetParent(playerParent);
            i.transform.localScale = playerScale;

            switch (i.playerNumber)
            {
                case 1:
                    i.transform.position = stageCraft.currentStage.spawn1.position;
                    break;
                case 2:
                    i.transform.position = stageCraft.currentStage.spawn2.position;
                    break;
            }
        }
    }

    public void SetupInputs(int inputVar)
    {
        switch(inputVar)
        {
            // Split Keyboard
            case 0:

                break;
            // Keyboard vs gamepad
            case 1:

                break;
            // Gamepad vs gamepad
            case 2:

                break;
            // Gamepad vs Keyboard
            case 3:

                break;
        }
    }

    public void PairPlayers()
    {
        player1.user.UnpairDevices();
        player2.user.UnpairDevices();
        InputUser.PerformPairingWithDevice(Keyboard.current, user: player1.user);
        InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);

    }

    public void PlayerLost()
    {
        stageCraft.NextStage();

        // Reset player health and score
        foreach(Player i in ActivePlayers) 
        {
            if(i.health > 0)
            {
                //StartCoroutine(i.WinAppearance());
                i.score++;
                Score(i.playerNumber, i.score);
                Debug.Log("NINJA SCORED, " + i.name + " " + i.score);
                
                // WIN STATE
                if (i.score >= winScore)
                {
                    if(i.playerNumber == 1)
                    {
                        gui.playerWinText.text = "RED WINS";
                    }
                    else
                    {
                        gui.playerWinText.text = "BLUE WINS";
                    }

                    StartCoroutine(gui.MessagePlayerWin());

                    Time.timeScale = 0;
                }

                i.health = 100;
            }

            // Check each player state
            switch (i.playerNumber)
            {
                case 1:
                    i.transform.position = stageCraft.currentStage.spawn1.position;

                    break;
                case 2:
                    i.transform.position = stageCraft.currentStage.spawn2.position;
                    break;
            }
        }
    }

    public void Score(int player, int score)
    {
        switch(player)
        {
            case 1:
                gui.player1Score.text = score.ToString();
                break;
            case 2:
                gui.player2Score.text = score.ToString();
                break;
        }
    }

    public void OnPlayerJoined()
    {
        Debug.Log("Player Joined");
    }
}
