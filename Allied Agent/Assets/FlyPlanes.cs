using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlanes : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitBeforeDestroy());
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0f,0,0.3f));
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(180f);
        Destroy(this);
    }
}
