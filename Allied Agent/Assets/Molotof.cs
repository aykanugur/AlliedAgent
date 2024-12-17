using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotof : MonoBehaviour
{
    public float fireDPS = 5f;
    public float fireRadius = 7.5f;
    public float fireDuration = 5f;
    public GameObject fireEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 hitPosition = transform.position;
        hitPosition.y = 0.52f;
        //instantiate fire effect at hitPosition with appropriate radius. I am going to sleep now.
        
        Destroy(gameObject);
    }
}
