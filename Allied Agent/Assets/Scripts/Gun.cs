using System.Collections;
using EZCameraShake;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
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
    public float bulletDamage;
    
    private float currentCooldown;
    public int currentCapacity;
    private int tracerCount = 0;
    private PlayerController _playerController;
    public float magn, rough, fadeIn, fadeOut;
    
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    
    public AudioClip[] _audioClips;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = timeBetweenShots;
        currentCapacity = magazineCapacity;
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.IsReload() == false)
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
    }

    private void Shoot()
    {
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut); // camera shake val
        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
        currentCapacity--;
        Vector3 direction = bulletSpawnPoint.forward;
        GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.Euler(0, 90, 90));
        currentBullet.GetComponent<BulletCollision>().hp = bulletDamage;
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

    // ReSharper disable Unity.PerformanceAnalysis
    private void Reload()
    {
        currentCapacity = magazineCapacity;
        tracerCount = tracerInterval;
        _playerController.StartReload();
    }

    public float GetRange()
    {
        float v = shootForce / projectileMass;
        float sin2Teta = Mathf.Sin(90 * Mathf.Deg2Rad);
        
        return (v * v * sin2Teta) / Mathf.Abs(Physics.gravity.y);
    }
    
}
