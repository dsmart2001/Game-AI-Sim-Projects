using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Panda;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    // Components
    private GameManager GM;
    public PlayerInput input => GetComponent<PlayerInput>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private BoxCollider2D coll => GetComponent<BoxCollider2D>();

    public GameObject attackColl;

    public Image sprite;

    public GameObject[] energySprites = new GameObject[3];

    [Header("Player Overview")]

    // Player status variables
    public int playerNumber;
    public int score;

    public float health = 100;
    public float energy = 1;
    public float jumpForce = 5;
    public float moveSpeed = 1;
    public float attackForce = 10;
    public float gravity = 50;

    [Header("Movement Checks")]

    // Property values
    private bool inputMovement = true;
    private bool extraGravity = true;
    public bool grounded = true;
    public float movementMaxVelocity;
    private float _movementMaxVelocity;

    public float gravityTimer = 1f;
    private float gravityTime;

    private Vector2 moveDirection = Vector2.zero;
    
    // Crouch scale values
    private Vector3 originalScale;
    private Vector3 crouchScale;
    private Vector2 originalCollScale;
    private Vector2 crouchCollScale;

    public float crouchDivision = 2;
    private InputValue crouchInput;
    public bool crouched = false;

    [Header("Attack Values")]

    // Attack values
    public float attackCollTimer = 1f;
    public float attackDelayTimer = 0.5f;
    private float attackDelayTime;

    [Header("Energy Values")]

    // Energy values
    public float energyMultiplier = 1;
    public float energyDeclineTimer = 5f;
    private float energyDeclineTime;
    public float energyAdd = 0.5f;
    public float energyMax = 3;

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();

        // Get character scale and set crouch scale
        originalScale = transform.localScale;
        crouchScale = new Vector3(originalScale.x, originalScale.y / crouchDivision, originalScale.z);

        // Get collider scale and set crouch scale
        originalCollScale = coll.size;
        crouchCollScale = new Vector3(originalCollScale.x, originalCollScale.y / crouchDivision);

        attackColl.SetActive(false);

        _movementMaxVelocity = movementMaxVelocity;

        //GM.ActivePlayers.Add(this);

        foreach(GameObject i in energySprites)
        {
            i.SetActive(false);
        }
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        // Timer to enable extra gravity when in air
        if(Time.time > gravityTime)
        {
            extraGravity = true;
        }
        else
        {
            extraGravity = false;
        }

        // Timer to decrease energy
        if(Time.time > energyDeclineTime)
        {
            EnergyBoost(false);
            energyDeclineTime = Time.time + energyDeclineTimer;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement forces
        rb.AddForce(moveDirection * moveSpeed * energyMultiplier);

        if(extraGravity)
        {
            ExtraGravity(gravity);
        }

        // Modify character and velocity on changing inputs
        if (moveDirection.x < 0)
        {
            transform.rotation = new Quaternion(0f, -180f, 0f, 0f);

            // Dampen force on changing inputs for quick turns
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x / 4, rb.velocity.y);
            }

            // Limit max speed
            if (rb.velocity.x < -movementMaxVelocity)
            {
                rb.velocity = new Vector2(-movementMaxVelocity, rb.velocity.y);
            }
        }
        else if (moveDirection.x > 0)
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            // Dampen force on changing inputs for quick turns
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x / 4, rb.velocity.y);
            }

            // Limit max speed
            if (rb.velocity.x > movementMaxVelocity)
            {
                rb.velocity = new Vector2(movementMaxVelocity, rb.velocity.y);
            }
        }
    }

    // Method that dictates movement direction, gets InputValue 
    [Task]
    private void OnMove(InputValue direction)
    {
        inputMovement = true;

        // Get Movement to move
        if(inputMovement)
        {
            moveDirection = new Vector2(direction.Get<Vector2>().x, 0);
        }

        Debug.Log(gameObject.name + " MOVING " + moveDirection);
    }

    // Method for the player jump, gets InputValue
    [Task]
    private void OnJump(InputValue jumpValue)
    {
        if(grounded)
        {
            if(energyMultiplier > 2)
            {
                rb.AddForce(Vector2.up * jumpForce * (energyMultiplier / 2));
            }
            else
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
            Debug.Log(gameObject.name + " JUMP " + jumpValue.Get<float>());
        }
    }

    // Coroutine to enact player attack
    [Task]
    private IEnumerator OnAttack()
    {
        if(Time.time > attackDelayTime && !crouched)
        {
            attackDelayTime = Time.time + attackCollTimer;
            CameraShake.Shake(0.5f);

            Vector2 refVal = Vector2.zero;

            // Add character force in direction of attack
            rb.velocity = new Vector2(0, 0);

            if (transform.rotation.y == 0)
            {
                //rb.AddForce(Vector2.SmoothDamp(rb.velocity, new Vector2(transform.position.x * -attackForce * energyMultiplier, transform.position.y), ref refVal, 0f));
                rb.AddForce(new Vector2(attackForce * energyMultiplier, rb.velocity.y));
            }
            else
            {
                // rb.AddForce(Vector2.SmoothDamp(rb.velocity, new Vector2(transform.position.x * attackForce * energyMultiplier, transform.position.y), ref refVal, 0f));
                rb.AddForce(new Vector2(-attackForce * energyMultiplier, rb.velocity.y));
            }

            // Enable attack collider
            attackColl.SetActive(true);
            inputMovement = false;

            Debug.Log(gameObject.name + " NINJA ATTACK");

            yield return new WaitForSeconds(attackCollTimer);

            attackColl.SetActive(false);
            inputMovement = true;
            Debug.Log(gameObject.name + " NINJA ATTACK ENDED");
        }
    }

    // Method to set character to crouch
    [Task]
    private void OnCrouch(InputValue crouchValue)
    {
        if(crouchValue.Get<float>() == 1)
        {
            transform.localScale = crouchScale;
            coll.size = crouchCollScale;

            inputMovement = false;
            crouched = true;
        }
        else if(crouchValue.Get<float>() == 0)
        {
            transform.localScale = originalScale;
            coll.size = originalCollScale;

            inputMovement = true;
            crouched = false;
        }


        Debug.Log(gameObject.name + " NINJA CROUCH " + crouchValue.Get<float>());
    }

    // Method to force extra gravity on character
    private void ExtraGravity(float gravityStrength)
    {
        rb.AddForce(Vector3.down * gravityStrength * rb.mass);
    }

    // Method that sets extra gravity timer ahead of current time
    public void GravityTimer()
    {
        gravityTime = Time.time + gravityTimer;
    }

    // Method to increase energy of player
    public void EnergyBoost(bool increase)
    {
        switch(increase)
        {
            case true:
                // Iterate energy and set timers
                energyMultiplier += energyAdd;
                energyDeclineTime = Time.time + energyDeclineTimer;
                movementMaxVelocity += (_movementMaxVelocity * energyAdd);

                if(energyMultiplier > energyMax)
                {
                    energyMultiplier = energyMax;
                }

                Debug.Log(gameObject.name + " NINJA INCREASED ENERGY TO " + energyMultiplier + ", movement max velocity = " + movementMaxVelocity);
                break;
            case false:
                if(energyMultiplier > 1)
                {
                    energyMultiplier -= energyAdd;
                    movementMaxVelocity -= (_movementMaxVelocity * energyAdd);

                    Debug.Log(gameObject.name + " NINJA DECREASED ENERGY TO " + energyMultiplier + ", movement max velocity = " + movementMaxVelocity);

                    if (energyMultiplier < 1)
                    {
                        energyMultiplier = 1;
                    }
                }
                break;
        }

        switch (energyMultiplier)
        {
            case 1:
                foreach (GameObject i in energySprites)
                {
                    i.SetActive(false);
                }
                break;
            case 1.75f:
                energySprites[0].SetActive(true);
                energySprites[1].SetActive(false);
                energySprites[2].SetActive(false);

                break;
            case 2.5f:
                energySprites[0].SetActive(false);
                energySprites[1].SetActive(true);
                energySprites[2].SetActive(false); 
                break;
            case 3.25f:
                energySprites[0].SetActive(false);
                energySprites[1].SetActive(false);
                energySprites[2].SetActive(true); 
                break;
        }
    }

    // Coroutine for the player taking damage+
    public IEnumerator TakeDamage(float damage)
    {
        health -= damage;
        CameraShake.Shake(2f);

        Debug.Log(gameObject.name + " NINJA DAMAGED, HEALTH = " + health);

        // FAIL-STATE
        if(health <= 0)
        {
            GM.PlayerLost();
            health = 100;
        }

        // Modify player appearance for damage
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        sprite.color = Color.white;     
    }

    public IEnumerator WinAppearance()
    {
        sprite.color = Color.green;

        yield return new WaitForSeconds(2f);

        sprite.color = Color.white;
    }
}
