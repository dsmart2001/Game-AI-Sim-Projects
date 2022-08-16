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
    public GameObject player1KBMPrefab;
    public GameObject player1GPPrefab;

    public static PlayerInput player2;
    public GameObject player2KBMPrefab;
    public GameObject player2GPPrefab;

    public Vector3 playerScale;

    public int winScore = 10;

    // Instantiate and setup player references
    public void StartGame(int inputVar)
    {
        switch (inputVar) 
        {
            case 0:
                // Instantiate and set players
                player1 = PlayerInput.Instantiate(player1KBMPrefab, controlScheme: "KBM 1", pairWithDevice: Keyboard.current);
                player2 = PlayerInput.Instantiate(player2KBMPrefab, controlScheme: "KBM 2", pairWithDevice: Keyboard.current);
                break;
            case 1:
                player1 = PlayerInput.Instantiate(player1KBMPrefab, controlScheme: "KBM 1", pairWithDevice: Keyboard.current);
                player2 = PlayerInput.Instantiate(player2GPPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.current);
                break;
            case 2:
                player1 = PlayerInput.Instantiate(player1GPPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.current);
                player2 = PlayerInput.Instantiate(player2GPPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.current);
                break;
            case 3:
                player1 = PlayerInput.Instantiate(player1GPPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.current);
                player2 = PlayerInput.Instantiate(player2KBMPrefab, controlScheme: "KBM 2", pairWithDevice: Keyboard.current);
                break;
        }
    
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

    // Method for buttons to choose input types and pair players
    public void SetupInputs(int inputVar)
    {
        menus.GameplayObject.SetActive(true);

        StartGame(inputVar);

        switch (inputVar)
        {
            // Split Keyboard
            case 0:
                PairPlayers();
                break;
            // Keyboard vs gamepad
            case 1:
                player1.user.UnpairDevices();
                player2.user.UnpairDevices();
                InputUser.PerformPairingWithDevice(Keyboard.current, user: player1.user);
                InputUser.PerformPairingWithDevice(Gamepad.current, user: player2.user);

                break;
            // Gamepad vs gamepad
            case 2:
                player1.user.UnpairDevices();
                player2.user.UnpairDevices();
                InputUser.PerformPairingWithDevice(Gamepad.current, user: player1.user);
                InputUser.PerformPairingWithDevice(Gamepad.current, user: player2.user);
                break;
            // Gamepad vs Keyboard
            case 3:
                player1.user.UnpairDevices();
                player2.user.UnpairDevices();
                InputUser.PerformPairingWithDevice(Gamepad.current, user: player1.user);
                InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
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

    // Method for settings players and stage up after one is defeated
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
