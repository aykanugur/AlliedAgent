using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPredictor : MonoBehaviour
{
    public GrenadeThrow grenadeThrow;
    public Transform grenadeSpawnPoint;
    public int maxIncrements = 50;
    public float incrementSize = 0.025f;
    public float rayOverlap = 1.1f;
    public LineRenderer trajectoryLine;
    public Transform hitMarker;

    private float grenadeMass;
    private float throwForce;
    private Vector3 direction;
    
    
    // Start is called before the first frame update
    void Start()
    {
        grenadeMass = grenadeThrow.grenadeMass;
        throwForce = grenadeThrow.throwForce;
        direction = grenadeSpawnPoint.forward.normalized;
        if(trajectoryLine == null)
            trajectoryLine = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            PredictTrajectory();
        }
        
    }

    private void PredictTrajectory()
    {
        Vector3 velocity = direction * throwForce / grenadeMass;
        Vector3 position = grenadeSpawnPoint.position;
        Vector3 nextPosition;
        float overlap;

        for (int i = 1; i < maxIncrements; i++)
        {
            velocity = CalculateNextVelocity(velocity, incrementSize);
            nextPosition = position + velocity * incrementSize;
            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap))
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                break;
            }

            hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxIncrements, (i, position));
        }
    }

    private Vector3 CalculateNextVelocity(Vector3 velocity, float incrementSize)
    {
        velocity += Physics.gravity * incrementSize;
        return velocity;
    }

    private void UpdateLineRender(int count, (int point, Vector3 position) pointPosition)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPosition.point, pointPosition.position);
    }

    private void MoveHitMarker(RaycastHit hit)
    {
        hitMarker.gameObject.SetActive(true);
        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible)
    {
        trajectoryLine.enabled = visible;
        hitMarker.gameObject.SetActive(visible);
    }
}
