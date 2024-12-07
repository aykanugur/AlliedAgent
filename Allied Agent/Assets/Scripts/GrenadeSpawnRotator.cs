using System;
using UnityEngine;

public class GrenadeSpawnRotator : MonoBehaviour
{
    
    public GameObject target;
    public Transform ground;
    
    private Transform transform;
    private Transform targetTransform;
    private float grenadeSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
        transform = GetComponent<Transform>();
        grenadeSpeed = GetComponent<GrenadeThrow>().throwForce / GetComponent<GrenadeThrow>().grenadeMass;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.G))
        {
            RotateToTarget();
            RotateToThrowAngle();
        }
    }

    private void RotateToThrowAngle()
    {
        //set transform rotation x to throw angle
        
        float v = grenadeSpeed;
        float g = Mathf.Abs(Physics.gravity.y);
        float h = transform.position.y - targetTransform.position.y;
        float x = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetTransform.position.x, 0, targetTransform.position.z));
        float angle;

        float firstEquationResult = (g * x * x / (v * v) - h) / Mathf.Sqrt(h * h + x * x);
        if (firstEquationResult > 1 || firstEquationResult < -1)
        {
            //this means the target point is out of range, so we will just throw to the furthest possible distance, which is the throw at 45 degrees.
            
            angle = 45f;
        }
        else
        {
            //target point is in range, proceed with formula you took from that physics video that took hours to understand
            
            float teta = Mathf.Rad2Deg * Mathf.Atan(x / h);
            angle = (Mathf.Rad2Deg * Mathf.Acos(firstEquationResult) + teta) / 2;
        }
        
        
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
    }

    private void RotateToTarget()
    {
        Vector3 direction = targetTransform.position - transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.Euler(transform.rotation.x, direction.y, transform.rotation.z);
    }
}
