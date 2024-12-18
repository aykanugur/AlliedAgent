using UnityEngine;

public class Molotof : MonoBehaviour
{
    public float fireDPS = 5f;
    public float fireScale = 1f;
    public float fireDuration = 5f;
    public GameObject fireEffect;

    void OnTriggerEnter(Collider other)
    {
        Vector3 firePosition = transform.position;
        RaycastHit hit;
        Physics.Raycast(firePosition, -Vector3.up, out hit, Mathf.Infinity);
        float height = hit.point.y;
        firePosition.y = height;
        
        //instantiate fire effect at hitPosition with appropriate radius. I am going to sleep now.
        
        GameObject currentFire = Instantiate(fireEffect, firePosition, Quaternion.identity);
        currentFire.transform.localScale *= fireScale;
        currentFire.GetComponent<Burn>().fireDPS = fireDPS;
        currentFire.GetComponent<Burn>().fireDuration = fireDuration;
        
        Destroy(gameObject);
    }
}
