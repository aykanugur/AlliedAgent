using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MenuManager : MonoBehaviour
{
    public Transform firepoint;

    public int range;

    public GameObject bomb;

    public GameObject settings, credits;

    public GameObject black;

    public AudioSource audioSource;
    
    
    

    private void Start()
    {
        PlayerPrefs.SetInt("save",0);
        StartCoroutine(Fire());
    }

    public void Credits()
    {
        settings.SetActive(false);
        credits.SetActive(true);
    }
    public void Settings()
    {
        settings.SetActive(true);
        credits.SetActive(false);
    }

    public void Exit()
    {
       Application.Quit();
    }

    public void StartGame()
    {
        black.SetActive(true);
        StartCoroutine(StartGameSequance());
        StartCoroutine(LoadIntro());
    }

    IEnumerator LoadIntro()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }

    IEnumerator StartGameSequance()
    {
        
        while (black.GetComponent<Image>().color.a >= 1 == false)
        {
            black.GetComponent<Image>().color = new Vector4(0, 0, 0, black.GetComponent<Image>().color.a + 0.05f);
            audioSource.volume = audioSource.volume - 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        
    }


    IEnumerator Fire()
    {
        while (true)
        {
            float x = Random.Range(firepoint.position.x - range, firepoint.position.x + range + 1 );
            float z = Random.Range(firepoint.position.z - range, firepoint.position.z + range + 1 );
            
            Instantiate(bomb, new Vector3(x, 100, z),bomb.transform.rotation);
            
            yield return new WaitForSeconds(3);
        }
    }
}
