using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AA : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawnPoint1,bulletSpawnPoint2;
    public Material[] materials;
    private bool _wait = false;


    private void FixedUpdate()
    {
        if (_wait == false)
        {
            StartCoroutine(WaitBeforeShoot());
            var plane = FindNearestPlane();
            bulletSpawnPoint1.transform.LookAt(plane.transform);
            bulletSpawnPoint2.transform.LookAt(plane.transform);

            for (int i = 0; i < 10; i++)
            {
                StartCoroutine(WaitBeforeShoot2());
            }
            
            _wait = true;
        }
    }

    IEnumerator WaitBeforeShoot()
    {
        yield return new WaitForSeconds(5);
        _wait = false;
    }

    IEnumerator WaitBeforeShoot2()
    {
        yield return new WaitForSeconds(0.5f);
        Shoot(bulletSpawnPoint1);
        Shoot(bulletSpawnPoint2);
    }

    private void Shoot(Transform spawnPoint)
    {
        Vector3 direction = spawnPoint.forward;
        GameObject currentBullet = Instantiate(bullet, spawnPoint.position, Quaternion.Euler(0, 90, 90));
        if (Random.Range(0, 3) == 0) // aykan kod
        {
            int random = Random.Range(0, 2);
            currentBullet.GetComponent<Renderer>().material = materials[random];
        }
        
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * 5, ForceMode.Impulse);
    }
    
    GameObject FindNearestPlane()
    {
        GameObject[] planes = GameObject.FindGameObjectsWithTag("Plane"); 
        if (planes.Length == 0)
        {
            return null; 
        }

        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity; 

        foreach (GameObject plane in planes)
        {
            float distance = Vector3.Distance(transform.position, plane.transform.position); 
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = plane;
            }
        }

        return nearest;
    }
}
