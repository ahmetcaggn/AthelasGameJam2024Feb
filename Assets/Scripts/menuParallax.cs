using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuParallax : MonoBehaviour
{
    public float offsetMultiplier = 3f;
    public float smoothTime = 0.3f;
    private Vector2 startPositon;
    private Vector3 velocity;

    private void Start()
    {
        startPositon = transform.position;
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = Vector3.SmoothDamp(transform.position, startPositon + (offset * offsetMultiplier),
            ref velocity, smoothTime);
        
    }
}