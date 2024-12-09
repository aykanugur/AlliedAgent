
using UnityEngine;
using UnityEngine.Events;

public class EnemyGun : MonoBehaviour
{
    public UnityEvent onGunShoot;
    public float timeBetweenShots;
    public bool isAuto;
    public float shootForce;
    public float projectileMass;
    public int magazineCapacity;
    public Material[] materials;
    public int tracerInterval = 5;
    public float time = 0.05f;
    
    private float currentCooldown;
    public int currentCapacity;
    private int tracerCount = 0;
    
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
        
    }
    
    public void Shoot()
    {
        currentCapacity--;
        Vector3 direction = bulletSpawnPoint.forward;
        GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.Euler(0, 90, 90));
        if (tracerCount <= 0)
        {
            tracerCount = tracerInterval;
            currentBullet.gameObject.GetComponent<BulletCollision>().MakeTracer();
        }
        else
        {
            currentBullet.gameObject.GetComponent<BulletCollision>().makeNonTracer();
        }
        
        if (Random.Range(0, 5) == 0) // aykan kod
        {
            int random = Random.Range(0, 2);
            currentBullet.GetComponent<Renderer>().material = materials[random];
        }
        currentBullet.GetComponent<Rigidbody>().mass = projectileMass;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        tracerCount--;
    }
    
    public void Reload()
    {
        currentCapacity = magazineCapacity;
        tracerCount = tracerInterval;
    }

    public float CalculateShootAngle(Transform target)
    {
        Transform origin = transform;
        float h = origin.position.y - target.position.y;
        float v = shootForce / projectileMass;
        float x = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.position.x, 0, target.position.z));
        float g = Mathf.Abs(Physics.gravity.y);

        float angle;
        
        float firstEquationResult = (g * x * x / (v * v) - h) / Mathf.Sqrt(h * h + x * x);
        if (firstEquationResult > 1 || firstEquationResult < -1)
        {
            //this means the target point is out of range, so we will just throw to the furthest possible distance, which is the throw at 45 degrees.
            
            angle = 45f;
        }
        else
        {
            //target point is in range, proceed with formula you took from that physics video that took hours to understand
            
            float teta = Mathf.Rad2Deg * Mathf.Atan(x / h);
            angle = (Mathf.Rad2Deg * Mathf.Acos(firstEquationResult) + teta) / 2;
            if (angle < 0)
            {
                angle += 90;
            }
        }

        return angle;
    }
    
    public float GetRange()
    {
        float v = shootForce / projectileMass;
        float sin2Teta = Mathf.Sin(90 * Mathf.Deg2Rad);
        
        return (v * v * sin2Teta) / Mathf.Abs(Physics.gravity.y);
    }
}
