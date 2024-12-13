using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCode : MonoBehaviour
{
    public GameObject bombFire;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            Instantiate(bombFire, transform.position, bombFire.transform.rotation);
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this.gameObject);
        }
    }
}
