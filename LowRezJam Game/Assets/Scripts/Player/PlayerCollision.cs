using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Player player => GetComponent<Player>();

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "Ground")
        {
            player.grounded = true;
            player.GravityTimer();
        }
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            player.grounded = true;
            player.GravityTimer();
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            player.grounded = false;
        }
    }
}
