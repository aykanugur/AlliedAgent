using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":

                break;
            
            case "Reflective":
                ReflectBullet(other.gameObject.transform.forward);
                break;
            case "DoNotDestroy":
                break;
            
            default:
                Destroy(this.gameObject);
                break;
                
        }
    }

    private void ReflectBullet(Vector3 normal)
    {
        Vector3 direction = rb.velocity.normalized;
        direction = Vector3.Reflect(direction, normal);
        rb.velocity = direction * rb.velocity.magnitude;
    }
    
    /*
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":

                break;
            
            case "Reflective":

                break;
            
            default:
                Destroy(this.gameObject);
                break;
                
        }
    }
    */
     
     
}

