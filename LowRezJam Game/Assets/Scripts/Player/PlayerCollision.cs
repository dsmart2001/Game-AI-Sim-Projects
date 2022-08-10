using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Player player => GetComponent<Player>();

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "Ground" && transform.position.y > c.gameObject.transform.position.y)
        {
            player.grounded = true;
            player.GravityTimer();
        }
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground" && transform.position.y > c.gameObject.transform.position.y)
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

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Energy")
        {
            player.EnergyBoost(true);
        }

        if (c.gameObject.tag == "Attack")
        {
            if (!c.transform.IsChildOf(transform))
            {
                StartCoroutine(player.TakeDamage(c.gameObject.GetComponent<AttackObject>().damage));

            }
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        
    }
    private void OnTriggerExit2D(Collider2D c)
    {
        
    }
}
