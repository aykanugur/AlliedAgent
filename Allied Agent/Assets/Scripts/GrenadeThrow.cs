
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    public float grenadeMass = 30f;
    public float throwForce = 10f;
    public float throwDelay=1.7f;
    public GameObject grenade;
    public GameObject molotof;

    private float currentDelay = 0f;
    private bool isThrowing = false;
    private bool molotofActive = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && currentDelay <= 0f && !Input.GetKey(KeyCode.LeftControl) && !isThrowing)
        {
            isThrowing = true;
            currentDelay = throwDelay;
        }
        
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G))
        {
            molotofActive = !molotofActive;
        }

        if (isThrowing)
        {
            currentDelay += Time.deltaTime;
            if (currentDelay >= throwDelay)
            {
                Throw();
            }
        }
        
    }

    public void Throw()
    {
        GameObject grenadeInUse;
        if (molotofActive)
        {
            grenadeInUse = molotof;
        }
        else
        {
            grenadeInUse = grenade;
        }
        GameObject currentGrenade = Instantiate(grenadeInUse, transform.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * throwForce, ForceMode.Impulse);
        currentGrenade.GetComponent<Rigidbody>().mass = grenadeMass;
    }

    public void StopThrow()
    {
        isThrowing = false;
        currentDelay = 0f;
    }
}
