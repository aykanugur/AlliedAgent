using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Missions : MonoBehaviour
{
    public TextMeshProUGUI text;
    public String[] missions;
    [SerializeField]private int index = 0;
    public DialogManager _dialogManager;
    public GameObject tutor;
    public PlayerController playerController;
    public GameObject box;
    public bool active = true;
    
    
    public void changeMissions()
    {
        index++;
        text.text = "MISSON " + index + ": "+missions[index];
        active = true;
    }

    private void Update()
    {
        switch (index)
        {
            
            case 2 :
                if (Input.GetKeyDown(KeyCode.R)&& active)
                {
                    StartCoroutine(WaitBeforeStartOtherMission(1));
                    active = false;

                }
                break;
            case 3 :
                if (playerController.GetGranade()&& active)
                {
                    changeMissions();
                }
                break;
            case 4 :

                if (playerController.GetCrouch()&& active)
                {
                    StartCoroutine(WaitBeforeStartOtherMission(1));
                    active = false;
                }
                break;
            case 5 : 
                
                if (Input.GetKeyDown(KeyCode.Q) && active)
                {
                    StartCoroutine(WaitBeforeStartOtherMission(5));
                    active = false;
                }
                break;
            case 6 :
                if ((box.GetComponent<Cover>().hp <= 0) && active)
                {
                    active = false;
                    StartCoroutine(WaitBeforeStartOtherMission(1) );
                }
                
                break;
            case 7 : 
                
                
                break;
            
            
        }
    }

    IEnumerator WaitBeforeStartOtherMission(int time)
    {
        yield return new WaitForSeconds(time);
        tutor.SetActive(true);
        _dialogManager.ContDialog();
        
        
    }
}
