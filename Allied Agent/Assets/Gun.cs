using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public UnityEvent onGunShoot;
    public float timeBetweenShots;
    public bool isAuto;
    public float shootForce;
    public float projectileMass;

    private float currentCooldown;

    public GameObject bullet;
    public Transform bulletSpawnPoint;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAuto)
        {
            if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && currentCooldown <= 0f)
            {
                onGunShoot?.Invoke();
                currentCooldown = timeBetweenShots;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire2") && currentCooldown <= 0f)
            {
                onGunShoot?.Invoke();
                currentCooldown = timeBetweenShots;
                Shoot();
            }
        }
        
        currentCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        Vector3 direction = bulletSpawnPoint.forward;
        GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(-bulletSpawnPoint.up));
        currentBullet.GetComponent<Rigidbody>().mass = projectileMass;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
    }
}
