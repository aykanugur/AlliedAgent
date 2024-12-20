using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Manager : MonoBehaviour
{
   public Image gun;
   public TextMeshProUGUI ammoText;
   public Sprite[] sprites;
   public GameObject gunGameObject, ammo;
   public float shootSpeed;
   
   
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
         gun.sprite = sprites[0];
         ammoText.text = currentGun.GetComponent<Gun>().currentCapacity.ToString();
         shootSpeed = currentGun.GetComponent<Gun>().time;
      }
      else
      {
         if (currentGun.name == "mac10")
         {
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
}
