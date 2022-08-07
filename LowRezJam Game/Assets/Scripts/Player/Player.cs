using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    // Components
    public PlayerInput input => GetComponent<PlayerInput>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private CapsuleCollider2D coll => GetComponent<CapsuleCollider2D>();
    public GameObject attackColl;

    // Player status variables
    public float health = 100;
    public float energy = 1;
    public float jumpForce = 5;
    public float moveSpeed = 1;
    public float attackForce = 10;
    public float gravity = 50;

    // Property values
    private bool inputMovement = true;
    private bool extraGravity = true;
    public bool grounded = true;

    public float gravityTimer = 1f;
    private float gravityTime;

    private Vector2 moveDirection = Vector2.zero;
    
    // Crouch scale values
    private Vector3 originalScale;
    private Vector3 crouchScale;
    private Vector2 originalCollScale;
    private Vector2 crouchCollScale;

    public float crouchDivision = 2;

    // Attack values
    public float attackCollTimer = 1f;
    public float attackDelayTimer = 0.5f;
    private float attackDelayTime;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement forces
        rb.AddForce(moveDirection * moveSpeed);

        if(extraGravity)
        {
            ExtraGravity(gravity);
        }
    }

    // Method that dictates movement direction, gets InputValue 
    private void OnMove(InputValue direction)
    {
        inputMovement = true;

        // Crouch
        if(direction.Get<Vector2>().y < 0)
        {
            Crouch(true);
            inputMovement = false;
        }
        else
        {
            Crouch(false);
            inputMovement = true;
        }


        // Get Movement to move
        if(inputMovement)
        {
            moveDirection = new Vector2(direction.Get<Vector2>().x, 0);
        }

        // Rotate character on move direction
        if (moveDirection.x < 0)
        {
            transform.rotation = new Quaternion(0f, -180f, 0f, 0f);

            // Dampen force on changing inputs for quick turns
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x / 3, rb.velocity.y);
            }
        }
        else if (moveDirection.x > 0)
        {
            transform.rotation = new Quaternion(0f, -0f, 0f, 0f);

            // Dampen force on changing inputs for quick turns
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x / 3, rb.velocity.y);
            }
        }

        Debug.Log("MOVING " + moveDirection);
    }

    // Method for the player jump, gets InputValue
    private void OnJump(InputValue jumpValue)
    {
        if(grounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            Debug.Log("JUMP " + jumpValue.Get<float>());
        }
    }

    // Coroutine to enact player attack
    private IEnumerator OnAttack()
    {
        if(Time.time > attackDelayTime)
        {
            attackDelayTime = Time.time + attackCollTimer;
            CameraShake.Shake();

            Vector2 refVal = Vector2.zero;

            // Add character force in direction of attack
            rb.velocity = new Vector2(0, 0);

            if (transform.rotation.y == 0)
            {
                rb.AddForce(Vector2.SmoothDamp(rb.velocity, new Vector2(transform.position.x * attackForce, transform.position.y), ref refVal, 0f));
            }
            else
            {
                rb.AddForce(Vector2.SmoothDamp(rb.velocity, new Vector2(transform.position.x * -attackForce, transform.position.y), ref refVal, 0f));
            }

            // Enable attack collider
            attackColl.SetActive(true);

            Debug.Log("NINJA ATTACK");

            yield return new WaitForSeconds(attackCollTimer);

            attackColl.SetActive(false);
            Debug.Log("NINJA ATTACK ENDED");
        }
    }

    // Method to set character to crouch
    private void Crouch(bool downInput)
    {
        if(downInput)
        {
            transform.localScale = crouchScale;
            coll.size = crouchCollScale;

            Debug.Log("NINJA CROUCH");
        }
        else
        {
            transform.localScale = originalScale;
            coll.size = originalCollScale;
        }
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
}
