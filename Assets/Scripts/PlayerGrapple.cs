using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class GrappleHook : MonoBehaviour
{
    // Maximum distance the grapple can reach.
    public float maxDistance = 2.8f;
    // Minimum distance for the grapple.
    public float minDistance = 0.5f;
    // Speed at which the grapple retracts.
    public float retractingSpeed = 0.5f;
    // Speed at which the player moves while retracting the grapple.
    public float grappleSpeed = 10f;
    // Speed at which the grapple is shot towards the target.
    public float grappleShootSpeed = 20f;
    // Flag to check if the player is currently grappling.
    bool isGrappling = false;
    // Flag to check if the grapple is currently retracting.
    public bool isHanging = false;
    // Reference to the LineRenderer component for drawing the grapple line.
    LineRenderer line;
    // Reference to the DistanceJoint2D component for handling the grappling mechanics.
    public DistanceJoint2D dj;
    // Layer mask to filter objects that can be grappled.
    [SerializeField] LayerMask GrappableMask;
    // Target position where the grapple will attach.
    Vector2 target;

    private void Start()
    {
        // Initialize the LineRenderer component.
        line = GetComponent<LineRenderer>();
        // Disable the DistanceJoint2D component initially.
        dj.enabled = false;
    }

    private void Update()
    {
        // Check for user input to initiate a grapple if not already grappling.
        if (Input.GetMouseButton(0) && !isGrappling) { StartGrapple(); }

        // Handle player input while hanging from the grapple.
        if (isHanging)
        {
            // Check for upward movement input (W key).
            if (Input.GetKey(KeyCode.W))
            {
                float newDistance = dj.distance - retractingSpeed * Time.deltaTime;
                // Ensure the distance does not go below the minimum allowed distance.
                if (minDistance > newDistance) { dj.distance = minDistance; }
                else { dj.distance = newDistance; }
            }

            // Check for downward movement input (S key).
            if (Input.GetKey(KeyCode.S))
            {
                float newDistance = dj.distance + retractingSpeed * Time.deltaTime;
                // Ensure the distance does not exceed the maximum allowed distance.
                if (newDistance > maxDistance) { dj.distance = maxDistance; }
                else { dj.distance = newDistance; }
            }
        }

        // Update the grapple line position while hanging.
        if (isHanging) { line.SetPosition(0, transform.position); }

        // Check for input to release the grapple (Right mouse button).
        if (Input.GetMouseButtonDown(1))
        {
            isGrappling = false;
            line.enabled = false;
            dj.enabled = false;
            isHanging = false;
        }
    }

    private void StartGrapple()
    {
        // Calculate the direction from the player to the mouse position.
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Cast a ray in the calculated direction to check for objects that can be grappled.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, GrappableMask);

        // If a grappleable object is hit, initiate the grapple.
        if (hit.collider != null)
        {
            target = hit.point;
            isGrappling = true;
            line.enabled = true;
            line.positionCount = 2;

            // Start the coroutine to handle the grapple animation.
            StartCoroutine(Grapple());
        }

        // Coroutine to handle the grapple animation.
        IEnumerator Grapple()
        {
            float t = 0;
            float time = 3f; // Time it takes for the grapple to reach the target.
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
            Vector2 newPosition;

            // Loop to update the grapple line position during the grapple animation.
            for (; t < time; t += grappleShootSpeed * Time.deltaTime)
            {
                newPosition = Vector2.Lerp(transform.position, target, t / time);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, newPosition);
                yield return null;
            }

            // Enable the DistanceJoint2D component and set its properties.
            dj.enabled = true;
            dj.connectedAnchor = target;
            dj.distance = (transform.position.ConvertTo<Vector2>() - target).magnitude;

            // Set the final position of the grapple line to the target and initiate retraction.
            line.SetPosition(1, target);
            isHanging = true;
        }
    }
}
