using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Import necessary Unity packages
using UnityEngine;

public class PlayerDashV2 : MonoBehaviour
{   
    // Serialized fields for inspector tweaking
    [SerializeField] private int resolution, waveCount, wobbleCount; // Parameters for rope animation
    [SerializeField] private float waveSize, maxDistance, minDistance, retractingSpeed, grappleSpeed; // Parameters for rope behavior
    // Booleans to track grappling and hanging states
    private bool isGrappling = false;
    private bool isHanging = false;
    // References to LineRenderer and DistanceJoint2D components
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private DistanceJoint2D _distanceJoint;
    [SerializeField] private LayerMask GrappableMask; // Mask for objects that can be grappled
    // Variables for storing target position, hit information, direction, rope vector, rope length, and animation speed
    Vector2 target;
    private RaycastHit2D hit;
    private Vector2 direction;
    private Vector3 ropeVector;
    private float ropeLength;
    [SerializeField] private float animSpeedRatio = 0.671428f;
    private float animSpeed;

    [SerializeField] private ParticleSystem _particleSystem;
    // Initialization
    private void Start()
    {
        _distanceJoint.enabled = false; // Disable DistanceJoint2D at start
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for left mouse button click to initiate grappling
        if (Input.GetMouseButtonDown(0) && !isGrappling) 
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Calculate direction towards mouse position
            hit = Physics2D.Raycast(transform.position, direction, maxDistance, GrappableMask); // Perform a raycast
            if (hit.collider != null)
            {
                target = hit.point; // Set the target position if an object is hit
                _particleSystem.transform.position = hit.point;
                _particleSystem.transform.position = hit.point;
                isGrappling = true; // Set grappling state to true
                _lineRenderer.enabled = true; // Enable LineRenderer for rope visualization
                ropeVector = (Vector3)target - transform.position; // Calculate rope vector
                ropeLength = Mathf.Sqrt(Mathf.Pow(ropeVector.x, 2) + Mathf.Pow(ropeVector.y, 2)); // Calculate rope length
                // Calculate animation speed based on rope length
                if (ropeLength > 4f) { animSpeed = ropeLength * animSpeedRatio; }
                else { animSpeed = ropeLength * animSpeedRatio + ropeLength; }
                StartCoroutine(AnimateRope(target)); // Start animating the rope towards the target
                _particleSystem.Play();
            }
        }

        // Handle hanging behavior if grappling
        if (isHanging == true)
        {
            // Handle retracting the rope with W key
            if (Input.GetKey(KeyCode.W))
            {
                float newDistance = _distanceJoint.distance - retractingSpeed * Time.deltaTime;
                if (minDistance > newDistance) { _distanceJoint.distance = minDistance; }
                else { _distanceJoint.distance = newDistance; }
            }
            // Handle extending the rope with S key
            if (Input.GetKey(KeyCode.S))
            {
                float newDistance = _distanceJoint.distance + retractingSpeed * Time.deltaTime;
                if (newDistance > maxDistance)
                { _distanceJoint.distance = maxDistance; }
                else
                { _distanceJoint.distance = newDistance; }
            }
        }

        // Check for right mouse button click to release grappling hook
        if (Input.GetMouseButtonDown(1))
        {
            isGrappling = false;
            _lineRenderer.enabled = false;
            _distanceJoint.enabled = false;
            isHanging = false;
        }

        // Update rope points if hanging
        if (isHanging == true)
        {
            float angle;
            if (hit.collider != null)
            {
                target = hit.point;
                angle = LookAtAngle((Vector3)target - (transform.position));
                SetPoints(target, 1, angle); // Set rope points based on target position and angle
            }
        }
    }

    // Coroutine for animating the rope towards the target
    private IEnumerator AnimateRope(Vector3 targetPos)
    {
        _lineRenderer.positionCount = resolution; // Set position count for LineRenderer
        float angle = LookAtAngle(targetPos - transform.position); // Calculate angle towards the target
        float percent = 0;
        while (percent <= 1f)
        {
            percent += Time.deltaTime * animSpeed; // Update percentage completion based on animation speed
            SetPoints(targetPos, percent, angle); // Set rope points based on current percentage and angle
            yield return null;
        }
        SetPoints(targetPos, 1, angle); // Set final rope points
        _distanceJoint.enabled = true; // Enable DistanceJoint2D
        _distanceJoint.connectedAnchor = targetPos; // Set connected anchor to target position
        _distanceJoint.distance = (transform.position.ConvertTo<Vector2>() - target).magnitude; // Set distance based on rope length
        isHanging = true; // Set hanging state to true
    }

    // Method for setting rope points
    private void SetPoints(Vector3 targetPos, float percent, float angle)
    {
        Vector3 ropeEnd = Vector3.Lerp(transform.position, targetPos, percent); // Calculate rope end position
        float length = Vector2.Distance(transform.position, ropeEnd); // Calculate rope length
        for (int i = 0; i < resolution; i++)
        {
            float xPosition = (float)i / resolution * length; // Calculate x position along the rope
            float reversePercent = 1 - percent; // Calculate reverse percentage
            float amplitude = Mathf.Sin(reversePercent * wobbleCount * Mathf.PI); // Calculate amplitude for rope wobbling
            float yPosition = Mathf.Sin((float) wobbleCount * i / resolution * 2 * Mathf.PI * reversePercent) * amplitude; // Calculate y position with wobbling effect
            Vector2 pos = RotatePoint(new Vector2(xPosition + transform.position.x, yPosition + transform.position.y), // Rotate rope point
                transform.position, angle);
            _lineRenderer.SetPosition(i, pos); // Set rope point position
        }
    }

    // Method for rotating a point around a pivot
    Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        Vector2 dir = point - pivot; // Calculate direction vector
        dir = Quaternion.Euler(0, 0, angle) * dir; // Rotate direction vector
        point = dir + pivot; // Update point position
        return point; // Return rotated point
    }

    // Method for calculating angle between object and player
    private float LookAtAngle(Vector2 target) { return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg; }
}    
