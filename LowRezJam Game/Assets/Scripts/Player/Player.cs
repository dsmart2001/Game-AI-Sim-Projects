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
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

    }

    public void OnMove(InputValue direction)
    {
        moveDirection = direction.Get<Vector2>();
    }

    public void OnJump()
    {
        rb.AddForce(Vector2.up * jumpForce);
        Debug.Log("JUMP");
    }

    public void OnAttack()
    {
        Debug.Log("NINJA ATTACK");
    }
}
