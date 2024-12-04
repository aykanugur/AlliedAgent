using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private bool _w, _a, _s, _d;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _w = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _a = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _s = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _d = true;
        }
    }

    public void WASD()
    {
        if (_w && _a && _s && _d)
        {
            
        }
    }
}
