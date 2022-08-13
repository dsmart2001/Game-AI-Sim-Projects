using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI : MonoBehaviour
{
    public Player playerOne;
    public Player playerTwo;

    public TMP_Text player1Health;
    public TMP_Text player2Health;

    public List<Image> player1Energy;
    public List<Image> player2Energy;

    public TMP_Text player1Score;
    public TMP_Text player2Score;

    public GameObject messagePanel;
    public TMP_Text playerDefeatedText;

    // Start is called before the first frame update
    void Start()
    {
        messagePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player1Health.text = playerOne.health.ToString();
        player2Health.text = playerTwo.health.ToString();
    }

    public IEnumerator MessagePlayerDefeated()
    {
        messagePanel.SetActive(true);
        playerDefeatedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);
        playerDefeatedText.gameObject.SetActive(false);
    }
}
