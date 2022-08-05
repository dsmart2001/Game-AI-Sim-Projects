using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float health = 100;
    public float energy = 1;
    public float jumpForce = 5;
    public float moveSpeed = 1;
    public float attackForce = 10;

    private Vector2 moveDirection = Vector2.zero;
    public PlayerInput input => GetComponent<PlayerInput>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        rb.AddForce(new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed));

    }

    public void OnMove(InputValue direction)
    {
        moveDirection = direction.Get<Vector2>();

        if(moveDirection.x < 0)
        {
            transform.rotation = new Quaternion(0f, -180f, 0f, 0f);
        }
        else if(moveDirection.x > 0)
        {
            transform.rotation = new Quaternion(0f, -0f, 0f, 0f);
        }
        Debug.Log("MOVING " + moveDirection);

    }

    public void OnJump()
    {
        rb.AddForce(Vector2.up * jumpForce);
        Debug.Log("JUMP");
    }

    public void OnAttack()
    {
        Debug.Log("NINJA ATTACK");
        CameraShake.Shake();
        rb.AddForce(new Vector2(moveDirection.x * attackForce, moveDirection.y * attackForce));
    }
}
