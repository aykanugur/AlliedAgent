using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class DialogManager : MonoBehaviour
{
    
    // hey my name is aykan and i watched and learned this code from 
    //https://www.youtube.com/watch?v=8oTYabhj248&t=232s
    //i did not write all the code same, i wrote myself and change things
    
    
    public TextMeshProUGUI text;
    public String[] lines;
    private int _index;
    public AudioClip[] clips;
    private AudioSource _audioSource;
    public RawImage tutorFace;
    public Texture[] textures;
    public PlayerController playerController;
    public bool[] checkpoints;
    private bool _cont = true;
    public Missions missions;
    public GameObject wall;
    public GameObject bismarck;
    
    private void Start()
    {
        text.text = String.Empty;
        _audioSource = GetComponent<AudioSource>();
        StartDialog();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _cont)
        {
            if (text.text == lines[_index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[_index];
            }
        }
        
        
        
    }

    public void StartDialog()
    {
        playerController.enabled = false;
        _index = 0;
        StartCoroutine(TypeLine());
    }

    public void ContDialog()
    {
        playerController._currentGun.GetComponent<Gun>().enabled = false;
        playerController.enabled = false;
        _index++;
        text.text = String.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        _audioSource.clip = clips[_index];
        _audioSource.Play();
        foreach (char c in lines[_index].ToCharArray())
        {
            int faceIndex = Random.Range(0, textures.Length);
            tutorFace.texture = textures[faceIndex];
            text.text += c;
            yield return new WaitForSeconds(0.05f);

        }
        
    }

    public void NextLine()
    {
        if (_index < lines.Length - 1 && checkpoints[_index] == false) // do not forget array bound
        {
            _index++;
            text.text = String.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (checkpoints[_index])
            {
                gameObject.SetActive(false);
                playerController.enabled = true;
                playerController._currentGun.GetComponent<Gun>().enabled = true;
                missions.changeMissions();
            }
            else
            {
                bismarck.SetActive(true);
                missions.changeMissions();
                playerController.enabled = true;
                playerController._currentGun.GetComponent<Gun>().enabled = true;
                Destroy(wall);
                Destroy(this.gameObject);
            }
        }
    }
}
