using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class FlashBang : MonoBehaviour
{
    private float _fadeTime = 1;
    public float fadeSpeed;
    public LayerMask layerMask;
    public float waitForBum;
    public Sprite canercan;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            StartCoroutine(WaitForFlash());
        }
    }

    IEnumerator WaitForFlash()
    {
        yield return new WaitForSeconds(1);
        GameObject lightObject = GameObject.Find("UI").transform.Find("FlashBangWhite").gameObject;
        Image whiteLight = GameObject.Find("UI").transform.Find("FlashBangWhite").GetComponent<Image>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10,layerMask);
            
        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
            if (collider.gameObject.CompareTag("Player"))
            {
                _fadeTime = 1 / Vector3.Distance(collider.transform.position, transform.position);
                if (_fadeTime < 0.5) _fadeTime = 0.5f;
                if (Random.Range(0, 100) == 5)
                {
                    lightObject.GetComponent<Image>().sprite = canercan;
                }
                else
                {
                    lightObject.GetComponent<Image>().sprite = null;
                }
                
                lightObject.SetActive(true);
                StartCoroutine(Fade(whiteLight,lightObject));
            }

            if (collider.gameObject.CompareTag("Enemy") && collider.gameObject.name != "CUBITTLER")
            {
                _fadeTime = 1 / Vector3.Distance(collider.transform.position, transform.position);
                collider.GetComponent<EnemyDamager>().SetFlashBanged(true);
                StartCoroutine(FlashTime(_fadeTime,collider.GetComponent<EnemyDamager>()));
            }
        }
    }

    IEnumerator FlashTime(float time,EnemyDamager dmgr)
    {
        yield return new WaitForSeconds(time);
        dmgr.SetFlashBanged(false);
    }
    

    IEnumerator Fade(Image light,GameObject lightObject)
    {
        while (_fadeTime >= 0)
        {
            light.color = new Vector4(1, 1, 1, _fadeTime);
            _fadeTime -= fadeSpeed;
            yield return new WaitForSeconds(0.1f);
        }
        lightObject.SetActive(false);
    }
    
}
