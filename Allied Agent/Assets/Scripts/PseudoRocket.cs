using UnityEngine;
using UnityEngine.Events;

public class PseudoRocket : MonoBehaviour
{
    public UnityEvent onRocketShoot;
    public float shootForce = 125f;
    public float rocketMass = 2.5f;
    public GameObject projectileRocket;
    public GameObject pseudoRocket;
    public float reloadTime = 1.5f;

    private bool needsReload = false;
    private float currentReloadTime = 0f;
    private bool isReloading = false;
    private Transform tf;

    void Start()
    {
        tf = pseudoRocket.GetComponent<Transform>();
    }
    
    void Update()
    {
        if (isReloading)
        {
            currentReloadTime -= Time.deltaTime;

            if (currentReloadTime <= 0)
            {
                Reload();
            }
        }
        
        if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire2") && !needsReload && !isReloading)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.Q) && needsReload && !isReloading)
        {
            isReloading = true;
            currentReloadTime = reloadTime;
        }
    }
    
    private void Fire()
    {
        needsReload = true;
        onRocketShoot?.Invoke();
        GameObject currentRocket = Instantiate(projectileRocket, transform.position, transform.rotation);
        Rigidbody rb = currentRocket.GetComponent<Rigidbody>();
        rb.mass = rocketMass;
        rb.AddForce(-tf.right.normalized * shootForce, ForceMode.Impulse);
        pseudoRocket.SetActive(false);
    }

    private void Reload()
    {
        isReloading = false;
        needsReload = false;
        pseudoRocket.SetActive(true);
    }
}
