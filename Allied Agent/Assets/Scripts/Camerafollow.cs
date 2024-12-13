using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    public bool cameraDontMove = false;
    
    void Update()
    {
        if (cameraDontMove == false)
        {
            transform.position = new Vector3(player.position.x+3, player.transform.position.y+2,transform.position.z);
        }


    }
}
