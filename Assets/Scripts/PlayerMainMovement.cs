using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movementSpeed = 5f;
    public float movementMaxSpeed = 5f;
    
    private float Horizontal;
    
    public bool isFacingRight = true;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    public float jumpSpeed = 18f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        flip();

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, jumpSpeed);
            rb.AddForce(new Vector2(0f, 10f * jumpSpeed));
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(Horizontal * movementSpeed, 0f));
        if (rb.velocity.x >= movementMaxSpeed)
        {
            rb.velocity = new Vector2(movementMaxSpeed, rb.velocity.y);
        }
        if (rb.velocity.x <= -movementMaxSpeed)
        {
            rb.velocity = new Vector2(-movementMaxSpeed, rb.velocity.y);
        }
    }


    private bool IsGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(GroundCheck.position, 0.7f, GroundLayer);
        return colliders;
    }
    
    // Character Flip function
    void flip()
    {
        if (isFacingRight && Horizontal < 0f || !isFacingRight && Horizontal > 0f)
        {            
            
            isFacingRight = !isFacingRight;
            
            //Flip with Scale

            // Vector3 cevir = transform.localScale;
            // cevir.x *= -1;
            // transform.localScale = cevir;

            
            //Flip with Y rotation

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y += 180f;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
    
    
    //Collision part
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("diken"))
        // {
        //     if (health <= 0)
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         health -= 30;
        //         Debug.Log("Current health is: " + health);
        //     }
        // }
    }
    
    
    
}