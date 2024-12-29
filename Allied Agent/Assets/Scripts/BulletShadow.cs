using System;
using UnityEngine;

public class BulletShadow : MonoBehaviour
{
    public GameObject shadow;
    
    
    private Transform shadowTransform;
    
    private const double TOLERANCE = 0.000000001;
    private const float HEIGHT = 0.011f;
    
    void Start()
    {
        GameObject currentShadow = Instantiate(shadow, transform.position, transform.rotation);
        shadowTransform = currentShadow.transform;
        shadowTransform.parent = transform;
        shadowTransform.position = new Vector3(shadowTransform.position.x, HEIGHT, shadowTransform.position.z);
        shadowTransform.rotation = Quaternion.Euler(90, 0, transform.rotation.eulerAngles.y);
    }
    
    void Update()
    {
        shadowTransform.rotation = Quaternion.Euler(90, 0, transform.rotation.eulerAngles.y);
        if (Math.Abs(shadowTransform.position.y - HEIGHT) > TOLERANCE)
        {
            shadowTransform.position = new Vector3(transform.position.x, HEIGHT, transform.position.z);
        }
    }
}
