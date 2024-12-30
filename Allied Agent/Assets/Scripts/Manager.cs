using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{
   public Image gun,bombI;
   public TextMeshProUGUI ammoText,currentAmmoText,bombtext;
   public Sprite[] sprites,bombsprites;
   public GameObject gunGameObject, ammo;
   public float shootSpeed;
   public GameObject black;
   public AudioSource audioSource;
   public GameObject[] guns;
   public bool first;
   public GameObject player;
   public GameObject checkpoint;
   public GameObject tutor;
   public Missions Missions;
   public GameObject bismarck;

   private void Start()
   {
      StartCoroutine(StartGameSequance());
      if (first == false)
      {
         guns[0].GetComponent<Gun>().currentAmmo = PlayerPrefs.GetInt("ak47C");
         guns[0].GetComponent<Gun>().currentCapacity = PlayerPrefs.GetInt("ak47M");
         guns[1].GetComponent<Gun>().currentAmmo = PlayerPrefs.GetInt("macC");
         guns[1].GetComponent<Gun>().currentCapacity = PlayerPrefs.GetInt("macM");
         guns[2].GetComponent<PseudoRocket>().rocketCount = PlayerPrefs.GetInt("rpgC");
         guns[3].GetComponent<GrenadeThrow>().currentGrenadeCount = PlayerPrefs.GetInt("grd");
         ChangeCurrentBombCount(guns[3].GetComponent<GrenadeThrow>().currentGrenadeCount);
         ChangeCurrentAmmo(guns[0].GetComponent<Gun>().currentAmmo);
         ChangeAmmo(guns[0].GetComponent<Gun>().currentCapacity);
      }
      Debug.Log(PlayerPrefs.GetInt("save"));

      if (PlayerPrefs.GetInt("save") == 1)
      {
         Debug.Log("checkpoint save activated");
         Destroy(tutor);
         Missions.index = 6;
         Missions.changeMissions();
         player.transform.position = new Vector3(checkpoint.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
         bismarck.SetActive(true);
         
      }
   }
   

   public void ChangeCurrentAmmo(int current)
   {
      currentAmmoText.text = current.ToString();
   }
   public void ChangeAmmo(int currentAmmo)
   {
      if (currentAmmo == 0)
      {
         ammoText.color = Color.red;
      }
      else
      {
         StartCoroutine(ChangeToRedTemporarily(Color.green));
      }
      ammoText.text = currentAmmo.ToString();
      
      
   }

   public void ChangeBomb(int index)
   {
      
      bombI.sprite = bombsprites[index];
   }

   public void ChangeCurrentBombCount(int current)
   {
      bombtext.text = current.ToString();
   }
   
   private IEnumerator ChangeToRedTemporarily(Color color)
   {
      ammoText.color = color; 
      yield return new WaitForSeconds(shootSpeed); 
      ammoText.color = Color.white;
   }

   public void ChangeWeapon(GameObject currentGun)
   {
      if (currentGun.name == "ak47")
      {
         ChangeCurrentAmmo(currentGun.GetComponent<Gun>().currentAmmo);
         gun.sprite = sprites[0];
         ammoText.text = currentGun.GetComponent<Gun>().currentCapacity.ToString();
         shootSpeed = currentGun.GetComponent<Gun>().time;
      }
      else
      {
         if (currentGun.name == "mac10")
         {
            ChangeCurrentAmmo(currentGun.GetComponent<Gun>().currentAmmo);
            gun.sprite = sprites[1];
            ammoText.text = currentGun.GetComponent<Gun>().currentCapacity.ToString();
            shootSpeed = currentGun.GetComponent<Gun>().time;
         }
         else
         {
            ChangeCurrentAmmo(currentGun.GetComponent<PseudoRocket>().rocketCount);
            gun.sprite = sprites[2];
         }
      }
   }

   public void StartNext()
   {
      black.SetActive(true);
      StartCoroutine(EndGameSequance());
      StartCoroutine(Next());

   }

   IEnumerator Next()
   {
      yield return new WaitForSeconds(4);
      PlayerPrefs.SetInt("ak47C",guns[0].GetComponent<Gun>().currentAmmo);
      PlayerPrefs.SetInt("ak47M",guns[0].GetComponent<Gun>().currentCapacity);
      PlayerPrefs.SetInt("macC",guns[1].GetComponent<Gun>().currentAmmo);
      PlayerPrefs.SetInt("macM",guns[1].GetComponent<Gun>().currentCapacity);
      PlayerPrefs.SetInt("rpgC",guns[2].GetComponent<PseudoRocket>().rocketCount);
      PlayerPrefs.SetInt("grd",guns[3].GetComponent<GrenadeThrow>().currentGrenadeCount);
      SceneManager.LoadScene(3);
      PlayerPrefs.SetInt("save",0);
   }
   public IEnumerator EndGameSequance()
   {
      black.SetActive(true);
        
      while (black.GetComponent<Image>().color.a >= 1 == false)
      {
         black.GetComponent<Image>().color = new Vector4(0, 0, 0, black.GetComponent<Image>().color.a + 0.05f);
         audioSource.volume = audioSource.volume - 0.05f;
         yield return new WaitForSeconds(0.1f);
      }
        
   }
   IEnumerator StartGameSequance()
   {
        
      while (black.GetComponent<Image>().color.a <0 == false)
      {
         black.GetComponent<Image>().color = new Vector4(0, 0, 0, black.GetComponent<Image>().color.a - 0.01f);
         yield return new WaitForSeconds(0.1f);
      }
      black.SetActive(false);
        
   }

   public void ReloadScene()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   public void Exit()
   {
      Application.Quit(0);
   }
}
