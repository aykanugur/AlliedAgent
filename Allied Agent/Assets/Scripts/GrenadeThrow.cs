
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    public float grenadeMass = 30f;
    public float throwForce = 10f;
    public float throwDelay=2.7f;
    public GameObject grenade;
    public GameObject molotof;

    private float currentDelay;
    private bool ableToThrow;
    private bool molotofActive = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        ableToThrow = true;
        currentDelay = throwDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G) && ableToThrow)
        {
            ableToThrow = false;
        }
        
        
        if(currentDelay > 0 && !ableToThrow)
            currentDelay -= Time.deltaTime;
        else if (currentDelay <= 0 && !ableToThrow)
        {
            currentDelay = throwDelay;
            ableToThrow = true;
            Throw();
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
}
