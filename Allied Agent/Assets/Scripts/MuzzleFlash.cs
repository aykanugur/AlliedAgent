using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MuzzleFlash : MonoBehaviour
{
   
    private void Start()
    {
        
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            GameObject _currentGun = GetComponent<PlayerController>()._currentGun;
            float timeBetweenShots = 0.1f;
            if (_currentGun.name != "rocketlauncher")
            {
                timeBetweenShots = _currentGun.GetComponent<Gun>().timeBetweenShots;
                GameObject _flicker = _currentGun.transform.Find("Muzzle_V").gameObject;
            
                if (Input.GetButton("Fire1") == false || Input.GetButton("Fire2") == false || _currentGun.GetComponent<Gun>().currentCapacity<=0 || GetComponent<PlayerController>().isReload())
                {
                    _flicker.SetActive(false);
                }
                else
                {
                    _flicker.SetActive(!_flicker.activeSelf);
                    _flicker.GetComponent<VisualEffect>().Play();
                    _flicker.transform.Find("Muzzle_Flash_Partiacle").GetComponent<ParticleSystem>().Play();
                }
            }
            yield return new WaitForSeconds(timeBetweenShots);
        }
        
    }
}
