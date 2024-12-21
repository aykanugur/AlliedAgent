using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Manager : MonoBehaviour
{
   public Image gun;
   public TextMeshProUGUI ammoText,currentAmmoText;
   public Sprite[] sprites;
   public GameObject gunGameObject, ammo,currentAmmoGameObject;
   public float shootSpeed;
   public GameObject black;
   public AudioSource audioSource;

   private void Start()
   {
      StartCoroutine(StartGameSequance());
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
            gun.sprite = sprites[2];
            ammoText.text = "-";
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
      SceneManager.LoadScene(3);
   }
   IEnumerator EndGameSequance()
   {
        
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
}
