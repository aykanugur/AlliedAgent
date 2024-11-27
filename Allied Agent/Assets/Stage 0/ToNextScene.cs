using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ToTheNextScene());
    }

    IEnumerator ToTheNextScene()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(1);
    }
}
