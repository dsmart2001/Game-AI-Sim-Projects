using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private StageCraft stageCraft => GetComponent<StageCraft>();
    private Menus menus => GetComponent<Menus>();

    private GUI gui => FindObjectOfType<GUI>();

    public List<Player> ActivePlayers;
    public int winScore = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayerLost()
    {
        stageCraft.NextStage();

        // Reset player health and score
        foreach(Player i in ActivePlayers) 
        {
            if(i.health <= 0)
            {
                i.health = 100;
            }
            else if(i.health > 0)
            {
                //StartCoroutine(i.WinAppearance());
                i.score++;
                Score(i.playerNumber, i.score);

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
}
