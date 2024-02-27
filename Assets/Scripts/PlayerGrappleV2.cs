using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashV2 : MonoBehaviour
{   
    /////////calculate the animation speed wrt. distance from beginning point to target.
    /////////can be considered decreasing gravity if the target position is in the bounds when the animation is playing.
    /////////research animating the rope while the player is moving correctly.
    [SerializeField] private int resolution, waveCount, wobbleCount;
    [SerializeField] private float waveSize, animSpeed, maxDistance, minDistance, retractingSpeed, grappleSpeed;
    private bool isGrappling = false;
    private bool isHanging = false;
    [SerializeField]private LineRenderer _lineRenderer;
    [SerializeField]private DistanceJoint2D _distanceJoint;
    [SerializeField] private LayerMask GrappableMask;
    Vector2 target;
    private RaycastHit2D hit;
    private Vector2 direction;
    private void Start()
    { _distanceJoint.enabled = false; }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGrappling)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            hit = Physics2D.Raycast(transform.position, direction, maxDistance, GrappableMask);
            if (hit.collider != null)
            {
                target = hit.point;
                isGrappling = true;
                _lineRenderer.enabled = true;
                StartCoroutine(AnimateRope(target));
            }
        }
        if (isHanging == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                float newDistance = _distanceJoint.distance - retractingSpeed * Time.deltaTime;
                if (minDistance > newDistance) { _distanceJoint.distance = minDistance; }
                else { _distanceJoint.distance = newDistance; }
            }
            if (Input.GetKey(KeyCode.S))
            {
                float newDistance = _distanceJoint.distance + retractingSpeed * Time.deltaTime;
                if (newDistance > maxDistance)
                { _distanceJoint.distance = maxDistance; }
                else
                { _distanceJoint.distance = newDistance; }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            isGrappling = false;
            _lineRenderer.enabled = false;
            _distanceJoint.enabled = false;
            isHanging = false;
        }

        if (isHanging == true)
        {
            float angle;
            if (hit.collider != null)
            {
                target = hit.point;
                angle = LookAtAngle((Vector3)target - (transform.position));
                SetPoints(target, 1, angle);
            }
        }
    }
    private IEnumerator AnimateRope(Vector3 targetPos)
    {
        _lineRenderer.positionCount = resolution;
        float angle = LookAtAngle(targetPos - transform.position);
        float percent = 0;
        while (percent <= 1f)
        {
            percent += Time.deltaTime * animSpeed;
            SetPoints(targetPos, percent, angle);
            yield return null;
        }
        SetPoints(targetPos, 1, angle);
        //_lineRenderer.SetPosition(1, targetPos);
        _distanceJoint.enabled = true;
        _distanceJoint.connectedAnchor = targetPos;
        _distanceJoint.distance = (transform.position.ConvertTo<Vector2>() - target).magnitude;
        isHanging = true;
    }
    private void SetPoints(Vector3 targetPos, float percent, float angle)
    {
        Vector3 ropeEnd = Vector3.Lerp(transform.position, targetPos, percent);
        float length = Vector2.Distance(transform.position, ropeEnd);
        for (int i = 0; i < resolution; i++)
        {
            float xPosition = (float)i / resolution * length;
            float reversePercent = 1 - percent;
            float amplitude = Mathf.Sin(reversePercent * wobbleCount * Mathf.PI);
            float yPosition = Mathf.Sin((float) wobbleCount * i / resolution * 2 * Mathf.PI * reversePercent) * amplitude;
            Vector2 pos = RotatePoint(new Vector2(xPosition + transform.position.x, yPosition + transform.position.y),
                transform.position, angle);
            _lineRenderer.SetPosition(i, pos);
        }
    }
    Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        Vector2 dir = point - pivot;
        dir = Quaternion.Euler(0, 0, angle) * dir;
        point = dir + pivot;
        return point;
    }
    private float LookAtAngle(Vector2 target) { return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg; }
}    

