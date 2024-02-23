using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDoubleJump : MonoBehaviour
{
    private PlayerMainMovement _playerMainMovement;
    private bool _isJumped;
    
    
    private void Start()
    {
        _playerMainMovement = GetComponent<PlayerMainMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doubleJump();
        }

        if (_playerMainMovement.IsGrounded() && _isJumped)
        {
            _isJumped = false;
        }
    }

    private void doubleJump()
    {
        if (!_playerMainMovement.IsGrounded() && !_isJumped)
        {
            _playerMainMovement.rb.AddForce(new Vector2(0f, 10f * _playerMainMovement.jumpSpeed));
            _isJumped = true;
        }
        
    }
}
