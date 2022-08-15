using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float itemValue = 1;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player")
        {
            if(c.gameObject.GetComponent<Player>().energyMultiplier != c.gameObject.GetComponent<Player>().energyMax)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
