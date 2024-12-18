using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float damage = 10f;
    public float damageRadius = 5f;
    public float countdownTime = 4.8f;
    public float explosionForce = 50f;
    public GameObject explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdownTime -= Time.deltaTime;
        if (countdownTime <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider hit in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (hit.gameObject != null)
            {
                switch (hit.gameObject.tag)
                {
                    case "Player":
                        //damage the player here
                    
                        break;
                
                    case "Enemy":
                        hit.gameObject.GetComponent<EnemyAnimations>().hp -= damage/(distance * distance);
                        if (hit.gameObject.GetComponent<EnemyAnimations>().hp <= 0)
                        {
                            Vector3 direction = hit.transform.position - transform.position;
                            direction = direction.normalized;
                            hit.GameObject().GetComponent<Rigidbody>().AddForce(direction * explosionForce / distance, ForceMode.Impulse);
                        }
                        break;
                
                    case "cover":
                        if (hit.gameObject.GetComponent<Cover>() != null)
                        {
                            hit.gameObject.GetComponent<Cover>().hp = hit.gameObject.GetComponent<Cover>().hp - damage/(distance * distance);
                            hit.GetComponent<Cover>().CheckHp();
                            if (hit.GetComponent<Cover>().hp > 0)
                            {
                                Vector3 direction = hit.transform.position - transform.position;
                                direction = direction.normalized;
                                hit.GameObject().GetComponent<Rigidbody>().AddForce(direction * explosionForce / distance * distance, ForceMode.Impulse);
                            }
                        }
                        break;
                
                    default:
                    
                        break;
                }
            }
        }
        
        //create explosion effect below
        Instantiate(explosion, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
