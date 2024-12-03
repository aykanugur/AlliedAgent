using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StopBlood());
    }

    IEnumerator StopBlood()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
