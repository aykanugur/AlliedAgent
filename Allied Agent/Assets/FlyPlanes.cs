using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlanes : MonoBehaviour
{
    private Transform _player;
    private bool _bomb;
    public GameObject bomb;
    public GameObject planes;
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _bomb = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0f,0,0.3f));
        float distance = Vector3.Distance(_player.position, transform.position);
        //170
        if (distance < 170 && _bomb == false)
        {
            _bomb = true;
            GameObject bombObject =Instantiate(bomb, transform.position, bomb.transform.rotation);
        }

        if (distance > 250 && _bomb)
        {
            Destroy(planes);
        }
    }
    
    
}
