
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class MuzzleFlash : MonoBehaviour
{
   
    private void Start()
    {
        
        StartCoroutine(Flicker());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Flicker()
    {
        while (true)
        {
            var currentGun = GetComponent<PlayerController>().currentGun;
            var timeBetweenShots = 0.1f;
            if (currentGun.name != "rocketlauncher")
            {
                timeBetweenShots = currentGun.GetComponent<Gun>().timeBetweenShots;
                GameObject flicker = currentGun.transform.Find("Muzzle_V").gameObject;
            
                if (Input.GetButton("Fire1") == false || Input.GetButton("Fire2") == false || currentGun.GetComponent<Gun>().currentCapacity<=0 || GetComponent<PlayerController>().IsReload())
                {
                    flicker.SetActive(false);
                }
                else
                {
                    flicker.SetActive(!flicker.activeSelf);
                    flicker.GetComponent<VisualEffect>().Play();
                    flicker.transform.Find("Muzzle_Flash_Particle").GetComponent<ParticleSystem>().Play();
                }
            }
            yield return new WaitForSeconds(timeBetweenShots);
        }
        
    }
}
