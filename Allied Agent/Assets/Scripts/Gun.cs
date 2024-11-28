using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public UnityEvent onGunShoot;
    public float timeBetweenShots;
    public bool isAuto;
    public float shootForce;
    public float projectileMass;
    public int magazineCapacity;
    public int currentCapacity;
    public Material[] materials;

    private float currentCooldown;

    public GameObject bullet;
    public Transform bulletSpawnPoint;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = timeBetweenShots;
        currentCapacity = magazineCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAuto)
        {
            if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && currentCooldown <= 0f && currentCapacity>0)
            {
                onGunShoot?.Invoke();
                currentCooldown = timeBetweenShots;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire2") && currentCooldown <= 0f && currentCapacity>0)
            {
                onGunShoot?.Invoke();
                currentCooldown = timeBetweenShots;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Reload();
        }
        
        currentCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        currentCapacity--;
        Vector3 direction = bulletSpawnPoint.forward;
        GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.Euler(0, 90, 90));
        if (Random.Range(0, 5) == 0) // aykan kod
        {
            int random = Random.Range(0, 2);
            currentBullet.GetComponent<Renderer>().material = materials[random];
        }
        currentBullet.GetComponent<Rigidbody>().mass = projectileMass;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
    }

    private void Reload()
    {
        currentCapacity = magazineCapacity;
    }
}
