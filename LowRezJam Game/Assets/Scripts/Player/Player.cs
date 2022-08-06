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
    private Vector2 moveDirection = Vector2.zero;

    // Crouch scale values
    private Vector3 originalScale;
    private Vector3 crouchScale;
    private Vector2 originalCollScale;
    private Vector2 crouchCollScale;

    public float crouchDivision = 2;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        crouchScale = new Vector3(originalScale.x, originalScale.y / crouchDivision, originalScale.z);

        originalCollScale = coll.size;
        crouchCollScale = new Vector3(originalCollScale.x, originalCollScale.y / crouchDivision);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        rb.AddForce(new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed));

        if(extraGravity)
        {
            ExtraGravity(gravity);
        }
    }

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
        }
        else if (moveDirection.x > 0)
        {
            transform.rotation = new Quaternion(0f, -0f, 0f, 0f);
        }

        Debug.Log("MOVING " + moveDirection);
    }

    private void OnJump(InputValue jumpValue)
    {
        rb.AddForce(Vector2.up * jumpForce);

        extraGravity = (jumpValue.Get<float>() != 1);
        Debug.Log("JUMP " + jumpValue.Get<float>());
    }

    private void OnAttack()
    {
        CameraShake.Shake();
        rb.AddForce(new Vector2(moveDirection.x * attackForce, moveDirection.y * attackForce));

        Debug.Log("NINJA ATTACK");
    }

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

    private void ExtraGravity(float gravityStrength)
    {
        rb.AddForce(Vector3.down * gravityStrength * rb.mass);
    }
}
