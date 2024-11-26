using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
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
        switch (other.gameObject.tag)
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

