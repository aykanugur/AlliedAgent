using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    private bool _wait = false ;
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
        yield return new WaitForSeconds(10);
        _wait = false;
        GameObject plane =Instantiate(_planes, transform.position, transform.rotation);

    }
}
