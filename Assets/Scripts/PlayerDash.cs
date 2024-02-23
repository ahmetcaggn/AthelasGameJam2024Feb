using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isDashing;
    private bool canDash = true;
    private float dashingPower = 250f;
    //private float dashingPower = 5f; add force using instead of this
    private float dashingTime = 1f;
    private float dashingCooldown = 0.2f;
    private float Horizontal;
    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if(Horizontal < 0)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.AddForce(new Vector2(CurrentDirection() * dashingPower, 0), ForceMode2D.Force); 
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
