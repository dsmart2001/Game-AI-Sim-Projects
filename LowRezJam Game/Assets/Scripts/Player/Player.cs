using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    // Components
    public PlayerInput input => GetComponent<PlayerInput>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private BoxCollider2D coll => GetComponent<BoxCollider2D>();

    public GameObject attackColl;

    [Header("Player Overview")]

    // Player status variables
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
    private bool crouched = false;

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
        // Get character scale and set crouch scale
        originalScale = transform.localScale;
        crouchScale = new Vector3(originalScale.x, originalScale.y / crouchDivision, originalScale.z);

        // Get collider scale and set crouch scale
        originalCollScale = coll.size;
        crouchCollScale = new Vector3(originalCollScale.x, originalCollScale.y / crouchDivision);

        attackColl.SetActive(false);

        _movementMaxVelocity = movementMaxVelocity;
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
    private void OnMove(InputValue direction)
    {
        inputMovement = true;

        // Get Movement to move
        if(inputMovement)
        {
            moveDirection = new Vector2(direction.Get<Vector2>().x, 0);
        }

        Debug.Log("MOVING " + moveDirection);
    }

    // Method for the player jump, gets InputValue
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
            Debug.Log("JUMP " + jumpValue.Get<float>());
        }
    }

    // Coroutine to enact player attack
    private IEnumerator OnAttack()
    {
        if(Time.time > attackDelayTime && !crouched)
        {
            attackDelayTime = Time.time + attackCollTimer;
            CameraShake.Shake();

            Vector2 refVal = Vector2.zero;

            // Add character force in direction of attack
            rb.velocity = new Vector2(0, 0);

            if (transform.rotation.y == 0)
            {
                rb.AddForce(Vector2.SmoothDamp(rb.velocity, new Vector2(transform.position.x * attackForce * energyMultiplier, transform.position.y), ref refVal, 0f));
            }
            else
            {
                rb.AddForce(Vector2.SmoothDamp(rb.velocity, new Vector2(transform.position.x * -attackForce * energyMultiplier, transform.position.y), ref refVal, 0f));
            }

            // Enable attack collider
            attackColl.SetActive(true);
            inputMovement = false;

            Debug.Log("NINJA ATTACK");

            yield return new WaitForSeconds(attackCollTimer);

            attackColl.SetActive(false);
            inputMovement = true;
            Debug.Log("NINJA ATTACK ENDED");
        }
    }

    // Method to set character to crouch
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


        Debug.Log("NINJA CROUCH " + crouchValue.Get<float>());
    }

    // Method to force extra gravity on character
    private void ExtraGravity(float gravityStrength)
    {
        rb.AddForce(Vector3.down * gravityStrength * rb.mass);
    }

    public void GravityTimer()
    {
        gravityTime = Time.time + gravityTimer;
    }

    public void EnergyBoost(bool increase)
    {
        switch(increase)
        {
            case true:
                energyMultiplier += energyAdd;
                energyDeclineTime = Time.time + energyDeclineTimer;
                movementMaxVelocity += (_movementMaxVelocity * energyAdd);

                Debug.Log("NINJA INCREASED ENERGY TO " + energyMultiplier + ", movement max velocity = " + movementMaxVelocity);
                break;
            case false:
                if(energyMultiplier > 1)
                {
                    energyMultiplier -= energyAdd;
                    movementMaxVelocity -= (_movementMaxVelocity * energyAdd);

                    Debug.Log("NINJA DECREASED ENERGY TO " + energyMultiplier + ", movement max velocity = " + movementMaxVelocity);

                    if (energyMultiplier < 1)
                    {
                        energyMultiplier = 1;
                    }
                }
                break;
        }
    }
}
