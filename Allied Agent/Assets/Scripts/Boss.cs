using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public float hp = 1000;
    public GameObject player;
    public Transform muzzle,muzzle2;
    public bool[] forms;
    public Rigidbody rb;
    public int horizontalSpeed;
    public GameObject bismarck;
    public GameObject shield;
    public bool shieldBool;
    public GameObject box;
    
    IEnumerator Shoot()
    {
        while (true)
        {
            muzzle.LookAt(player.transform.position);
            muzzle2.LookAt(player.transform.position);
            // write basic shoot code here pls !! 
            // with red tail
            if (shieldBool)
            {
                shield.SetActive(false);
                StartCoroutine(OpenShield());
            }
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator OpenShield()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("shiled activated");
        shield.SetActive(true);
    }

    private void Form1()
    {
        if (forms[0] == false)
        {
            forms[0] = true;
            StartCoroutine(Shoot());
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Form2()
    {
        if (forms[1] == false)
        {
            InvokeRepeating(nameof(Jump),1f,5f);
            forms[1] = true;
        }
    }
    
    private void FixedUpdate()
    {
        if (forms[0])
        {
            rb.velocity = new Vector3(0, rb.velocity.y, horizontalSpeed);
        }
    }

    void Jump()
    {
       rb.AddForce(Vector3.up * 5000);
    }

    public void Die()
    {
        box.SetActive(false);
        Destroy(this.gameObject);
    }

    private void Form3()
    {
        if (forms[2] == false)
        {
            forms[2] = true;
            bismarck.SetActive(true);
            bismarck.GetComponent<Bismarck>().shootSpeed = 5;
        }
    }

    private void Form4()
    {
        if (forms[3] == false)
        {
            Debug.Log("FORM 4 ");
            forms[3] = true;
            bismarck.GetComponent<Bismarck>().shootSpeed = 3;
        }
    }
    

    private void Update()
    {
        switch (hp)
        {
         case > 800 :
             Form1();
             // form 1 
             
             // walk right left and shoot player
             break;
         
         case > 600 :
             //form 2 
             Form2();
             // walk right left and jump sometimes and shoot player
             break;
         
         case > 400 :
             Form3();
             // form 3 call bismarck
             // bismarck speed is 1 ammo 5 second
             // walk right left and jump sometimes and shoot player
         break;
         
         case > 200:
             Form4();
             //last form
             // bismarck shoot speed 1 ammo 3 second
             // walk right left and jump sometimes and shoot player
             // also use shield while not shooting 
             shieldBool = true;
         break;
         
         case < 0:
             // die 
             Die();
             
         break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftBorder") || other.gameObject.CompareTag("RightBorder"))
        {
            Debug.Log("test");
            horizontalSpeed *= -1;
        }
    }
}
