using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerMainMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movementSpeed = 5f;
    public float movementMaxSpeed = 5f;
    
    private float Horizontal;
    
    public bool isFacingRight = true;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    public float jumpSpeed = 18f;
    private bool isFalling = false;

    public Animator anim;

    public Volume volume;
    private ColorAdjustments _colorAdjustments;

    [SerializeField] AudioSource _audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        volume.profile.TryGet(out _colorAdjustments);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        _audioSource.mute = true;
    }

    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("SpeedX",Math.Abs(Horizontal));
        flip();

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() || Input.GetKeyDown(KeyCode.W) && IsGrounded() || Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            // myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, jumpSpeed);
            rb.AddForce(new Vector2(0f, 10f * jumpSpeed));
            anim.SetTrigger("Jump");
        }
        
        if (rb.velocity.y < -0.5f && !IsGrounded() && !isFalling)
        {
            anim.SetTrigger("Fall");
            anim.SetBool("IsFalling",true);
            isFalling = true;
        }

        if (IsGrounded())
        {
            anim.SetBool("IsFalling",false);
            anim.SetBool("IsJumping", false);
            anim.Play("idle");
            isFalling = false;
        }
        
        rb.AddForce(new Vector2(Horizontal * movementSpeed, 0f));
        if (rb.velocity.x >= movementMaxSpeed)
        {
            _audioSource.mute = false;
        }
        else if (rb.velocity.x <= -movementMaxSpeed)
        {
            _audioSource.mute = false;
        }
        else
        {
            _audioSource.mute = true;
        }

        // if (IsGrounded())
        // {
        //     anim.SetBool("IsFalling",false);
        // }

        // if (IsGrounded())
        // {
        //     anim.SetBool("IsJumping", false);
        // }
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


    public bool IsGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(GroundCheck.position, 0.3f, GroundLayer);
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
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "collider" || col.gameObject.layer == 6)
        {
            // gameObject.transform.position = new Vector3(15.65f, 8.29f, 0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        
        
        if (col.gameObject.name == "berry")
        {
            if (_colorAdjustments.saturation.value < 200)
            {
                _colorAdjustments.saturation.value += 35;   
            }
            // col.GetComponent<ParticleSystem>().Play();
            Destroy(col.gameObject);
            // StartCoroutine(DestroyObject(col.gameObject));
        }
    }
    IEnumerator DestroyObject(GameObject col)
    {
        yield return new WaitForSeconds(1f);
        Destroy(col);
    }
    
    
    
}
