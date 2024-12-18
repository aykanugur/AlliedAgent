using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeRandomRotator : MonoBehaviour
{
    public float maxRotationSpeed = 20f;
    
    //Start is called before the first frame update
    void Start()
    {
        RandomlyRotate();
    }

    // Update is called once per frame
    private void RandomlyRotate()
    {
        float randomValueX = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        float randomValueY = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        float randomValueZ = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        Vector3 randomRotation = new Vector3(randomValueX, randomValueY, randomValueZ);
        GetComponent<Rigidbody>().angularVelocity = randomRotation;
    }
}
