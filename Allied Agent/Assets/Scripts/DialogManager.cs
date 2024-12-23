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
    public Manager manager;
    
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

    private void StartDialog()
    {
        playerController.enabled = false;
        _index = 0;
        StartCoroutine(TypeLine());
    }

    public void ContDialog()
    {
        if (manager != null)
        {
            manager.gunGameObject.SetActive(false);
            manager.ammo.SetActive(false);
        }
        if (playerController.currentGun.name != "rocketlauncher")
        {
            playerController.currentGun.GetComponent<Gun>().enabled = false;
        }
        else
        {
            playerController.currentGun.GetComponent<PseudoRocket>().enabled = false;
        }
        playerController.enabled = false;
        _index++;
        text.text = String.Empty;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
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

    private void NextLine()
    {
        if (_index < lines.Length - 1 && checkpoints[_index] == false) // do not forget array bound
        {
            _index++;
            text.text = String.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (manager != null)
            {
                manager.gunGameObject.SetActive(true);
                manager.ammo.SetActive(true);
            }
            if (checkpoints[_index])
            {
                gameObject.SetActive(false);
                playerController.enabled = true;
                if (playerController.currentGun.name != "rocketlauncher")
                {
                    playerController.currentGun.GetComponent<Gun>().enabled = true;
                }
                else
                {
                    playerController.currentGun.GetComponent<PseudoRocket>().enabled = true;
                }
                missions.changeMissions();
            }
            else
            {
                bismarck.SetActive(true);
                missions.changeMissions();
                playerController.enabled = true;
                if (playerController.currentGun.name != "rocketlauncher")
                {
                    playerController.currentGun.GetComponent<Gun>().enabled = true;
                }
                else
                {
                    playerController.currentGun.GetComponent<PseudoRocket>().enabled = true;
                }
                Destroy(wall);
                Destroy(this.gameObject);
                
            }
        }
    }
}
