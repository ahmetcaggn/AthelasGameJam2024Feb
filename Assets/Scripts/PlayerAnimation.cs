using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerMainMovement playerMainMovement;
    private float Horizontal;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMainMovement = GetComponent<PlayerMainMovement>();
    }
    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
    }
}
