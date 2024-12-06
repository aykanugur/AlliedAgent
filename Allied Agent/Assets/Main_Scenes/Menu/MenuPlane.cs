using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class MenuPlane : MonoBehaviour
{
    public GameObject prop;
    public GameObject _cinemachineTracked;
    private void FixedUpdate()
    {
        Debug.Log("test");
        prop.transform.Rotate(3600 * Time.deltaTime, 0, 0);
    }
}
