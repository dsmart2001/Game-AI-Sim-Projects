using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Player player => GetComponent<Player>();

    private void OnCollisionEnter2D(Collision2D c)
    {
        if((c.gameObject.tag == "Ground" || c.gameObject.tag == "Player" || c.gameObject.tag == "Ground1Way") && transform.position.y > c.gameObject.transform.position.y)
        {
            player.grounded = true;
            player.GravityTimer();
        }

        if(c.gameObject.tag == "Ground1Way" && player.crouched)
        {
            
        }

    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if ((c.gameObject.tag == "Ground" || c.gameObject.tag == "Player" || c.gameObject.tag == "Ground1Way") && transform.position.y > c.gameObject.transform.position.y)
        {
            player.grounded = true;
            player.GravityTimer();
        }

        if (c.gameObject.tag == "Ground1Way" && player.crouched)
        {
            Physics2D.IgnoreCollision(c.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground" || c.gameObject.tag == "Player" || c.gameObject.tag == "Ground1Way")
        {
            player.grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Energy" && player.energyMultiplier != player.energyMax)
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
