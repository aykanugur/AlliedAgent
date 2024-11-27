using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{

    public UnityEvent onRocketShoot;
    public float shootForce;
    public float rocketMass;

    private bool hasFired = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire2") && !hasFired)
        {
            hasFired = true;
            Fire();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    void Explode()
    {
        //call DrawCircle here to deal splash damage
        
        Destroy(this.gameObject);
    }

    void Fire()
    {
        onRocketShoot?.Invoke();
        gameObject.GetComponent<Transform>().SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().mass = rocketMass;
        GetComponent<Rigidbody>().AddForce(transform.TransformDirection(-Vector3.right).normalized * shootForce, ForceMode.Impulse);
    }
}
