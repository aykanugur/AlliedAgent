using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    public float waitSecond = 10;
    private bool _wait = false ;
    public bool bum;
    [SerializeField] private GameObject _planes;
    private void FixedUpdate()
    {
        if (_wait==false)
        {
            StartCoroutine(waitBeforeSpawn());
            _wait = true;
        }
    }

    IEnumerator waitBeforeSpawn()
    {
        yield return new WaitForSeconds(waitSecond);
        _wait = false;
        GameObject plane =Instantiate(_planes, transform.position, transform.rotation);
        if (bum)
        {
            plane.GetComponent<PanzerVisual>().random = true;
        }

    }
}
