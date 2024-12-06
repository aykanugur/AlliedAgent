using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void StartReloadSoundPlay()
    {
        GameObject _currentGun = GetComponent<PlayerController>().currentGun;
        _currentGun.GetComponent<AudioSource>().clip = _currentGun.GetComponent<Gun>()._audioClips[1];
        _currentGun.GetComponent<AudioSource>().Play();
    }
    public void EndReloadSoundPlay()
    {
        GameObject _currentGun = GetComponent<PlayerController>().currentGun;
        _currentGun.GetComponent<AudioSource>().clip = _currentGun.GetComponent<Gun>()._audioClips[2];
        _currentGun.GetComponent<AudioSource>().Play();
    }
}
