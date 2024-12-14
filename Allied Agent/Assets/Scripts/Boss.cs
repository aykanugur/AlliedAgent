using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public int hp = 500;
    public GameObject player;
    public Transform muzzle;
    public bool[] forms;
    private float timer = 0f;
    public Rigidbody rb;
    public int horizontalSpeed;
    public void ShootPlayer()
    
    {
        muzzle.LookAt(player.transform.position);
    }

    private void Form1()
    {
        if (forms[0] == false)
        {
            forms[0] = true;
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
       rb.AddForce(Vector3.up * 10000);
    }
    

    private void Update()
    {
        switch (hp)
        {
         case > 400 :
             Form1();
             // form 1 
             
             // walk right left and shoot player
             break;
         
         case > 300 :
             //form 2 
             Form2();
             // walk right left and jump sometimes and shoot player
             break;
         
         case > 200 :
             
             // form 3 call bismarck
             // bismarck speed is 1 ammo 5 second
             // walk right left and jump sometimes and shoot player
         break;
         
         case > 100:
             //last form
             // bismarck shoot speed 1 ammo 3 second
             // walk right left and jump sometimes and shoot player
             // also use shield while not shooting 
         break;
         
         case < 0:
             // die 
         break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftBorder") || other.gameObject.CompareTag("RightBorder"))
        {
            horizontalSpeed *= -1;
        }
    }
}
