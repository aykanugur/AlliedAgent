using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public float hp = 100f;
   


    public void CheckHp()
    {
        if (hp < 0)
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if(child.CompareTag("coverSide"))continue;
                if(child.name == "Canvas") continue;
                if (child.name == "CoverTriggerArea")
                {
                    Destroy(child.gameObject);
                    continue;
                }
                child.GetComponent<MeshCollider>().enabled = true;
                child.GetComponent<Rigidbody>().isKinematic = false;
            }
            GetComponent<BoxCollider>().enabled = false;
            
        }
    }

    
    
}
