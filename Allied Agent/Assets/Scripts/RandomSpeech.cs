using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpeech : MonoBehaviour
{
    public string[] idleTextStrings, bemerkenTexts, prayTexts; // Dikkat: 'string' olmalı, 'String' değil
    public bool idle, pray;
    public TextMeshProUGUI text;
    public GameObject textO;
    private Quaternion _fixedRotation;

    private void Start()
    {
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI reference is missing!");
            return;
        }

        _fixedRotation = text.transform.rotation;
        MainFunction();
    }

    private void Update()
    {
        text.transform.rotation = _fixedRotation;
    }

    private void MainFunction()
    {
        if (idle)
        {
            Invoke(nameof(Idle), 2f);
        }
        else
        {
            if (pray)
            {
                text.text = "I SURRENDER!";
            }
            else
            {
                if (bemerkenTexts.Length > 0)
                {
                    text.text = bemerkenTexts[0];
                }
                Invoke(nameof(Bemerken), 2f);
            }
        }
    }

    private void Idle()
    {
        if (idleTextStrings.Length == 0)
        {
            Debug.LogWarning("idleTextStrings array is empty!");
            return;
        }

        if (Random.Range(0, 8) == 5)
        {
            int randomIndex = Random.Range(0, idleTextStrings.Length); // Tüm indeksleri kapsar
            text.text = idleTextStrings[randomIndex];
        }

        MainFunction(); // Tekrar çağırma
    }

    private void Bemerken()
    {
        if (bemerkenTexts.Length == 0)
        {
            Debug.LogWarning("bemerkenTexts array is empty!");
            return;
        }

        if (Random.Range(0, 8) == 5)
        {
            int randomIndex = Random.Range(0, bemerkenTexts.Length); // Tüm indeksleri kapsar
            text.text = bemerkenTexts[randomIndex];
        }

        MainFunction(); // Tekrar çağırma
    }
}