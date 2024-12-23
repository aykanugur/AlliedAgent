
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    public float grenadeMass = 30f;
    public float throwForce = 10f;
    public float throwDelay=2.7f;
    public GameObject[] bombs;
    public int maxGrenadeCount = 6;

    private float currentDelay;
    private bool ableToThrow;
    public int bombIndex; // 0 grenade 1 molotof 2 flash
    public int currentGrenadeCount;

    public Manager manager;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        ableToThrow = true;
        currentDelay = throwDelay;
        currentGrenadeCount = maxGrenadeCount;
    }

    // Update is called once per frame
    void Update()
    {   manager.ChangeCurrentBombCount(currentGrenadeCount);
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (bombIndex != 2)
                {
                    bombIndex++;
                }
                else
                {
                    bombIndex = 0;
                }
            }
            manager.ChangeBomb(bombIndex);
        }
        
        if (Input.GetKeyUp(KeyCode.G) && ableToThrow && Input.GetKey(KeyCode.LeftControl)== false && currentGrenadeCount > 0)
        {
            ableToThrow = false;
            currentGrenadeCount--;
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
        Debug.Log("throw");
        GameObject grenadeInUse;
        grenadeInUse = bombs[bombIndex];
        GameObject currentGrenade = Instantiate(grenadeInUse, transform.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().AddForce(transform.right.normalized * throwForce, ForceMode.Impulse);
        currentGrenade.GetComponent<Rigidbody>().mass = grenadeMass;
    }
}
