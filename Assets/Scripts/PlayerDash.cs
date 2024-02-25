using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isDashing;
    private bool canDash = true;
    public float dashingPower = 15f;
    //private float dashingPower = 5f; add force using instead of this
    private float dashingTime = 0.5f;
    private float dashingCooldown = 0.2f;
    private float Horizontal;
    PlayerMainMovement playerMovement;
    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMainMovement>();
    }
    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        if (isDashing)
        {
            return;
        }
        if(canDash && Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            StartCoroutine(Dash());
        }
    }
    private float CurrentDirection()
    {
        if(playerMovement.isFacingRight == true)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    private IEnumerator Dash()
    {
        playerMovement.movementMaxSpeed = 24f;
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0f;
        //rb.AddForce(new Vector2(CurrentDirection() * dashingPower, 0), ForceMode2D.Force); 
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * CurrentDirection(), 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb.velocity = Vector3.zero;
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        playerMovement.movementMaxSpeed = 5;
    }
}
