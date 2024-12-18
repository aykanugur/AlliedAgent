using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public float fireDPS = 5f;
    public float fireDuration = 5f;

    private float oneSecond = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireDuration <= 0)
        {
            Destroy(gameObject);
        }
        
        fireDuration -= Time.deltaTime;
        oneSecond -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                if (oneSecond <= 0)
                {
                    oneSecond = 1;
                    other.gameObject.GetComponent<EnemyAnimations>().hp -= fireDPS;
                }
                break;
            
            case "Player":
                if (oneSecond <= 0)
                {
                    break; //delete break after you write the code to damage the player
                    oneSecond = 1;
                    //damage the player here
                    
                }
                break;
            
            case "cover":
                if (oneSecond <= 0)
                {
                    oneSecond = 1;
                    other.gameObject.GetComponent<Cover>().hp -= fireDPS;
                }
                break;
            
            
            default:
                
                break;
        }
    }
}
