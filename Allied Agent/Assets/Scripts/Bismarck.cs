
using System.Collections;

using UnityEngine;


public class Bismarck : MonoBehaviour
{
    public GameObject[] cameras;
    public GameObject bismarck;
    private bool _start;
    public AudioClip audioClip;
    public GameObject player;
    public int area;
    public GameObject ships;
    public Transform bismarckTarget;
    public GameObject fire;
    private PlayerController _playerController;
    public float shootSpeed = 10;
    public LayerMask layerMask;  
    
    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        StartCoroutine(BismarckStart());
        
    }

    private void FixedUpdate()
    {
        if (_start && bismarck != null)
        {
            bismarck.transform.Translate(new Vector3(0.03f,0,-0.1f));
            Debug.Log("test");
        }

        if (_start && bismarck == null)
        {
            _start = false;
            StartCoroutine(Bumbum());
        }

    }

    IEnumerator BismarckStart()
    {
        yield return new WaitForSeconds(5);
        _playerController.SetAim(false);
        _playerController._currentGun.GetComponent<Gun>().enabled = false;
        _playerController.enabled = false;
        
        _start = true;
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        GetComponent<AudioSource>().Play();
        StartCoroutine(startPlayOtherSound());
        StartCoroutine(BismarckStop());

    }

    IEnumerator startPlayOtherSound()
    {
        yield return new WaitForSeconds(2.7f);
        GetComponent<AudioSource>().clip = audioClip;
        GetComponent<AudioSource>().Play();

    }
    IEnumerator BismarckStop()
    {
        yield return new WaitForSeconds(5);
        cameras[1].SetActive(false);
        cameras[0].SetActive(true);
        player.GetComponent<PlayerController>().enabled = true;
        _playerController._currentGun.GetComponent<Gun>().enabled = true;
        Destroy(ships);
    }
    
    // every 10 second bum bum

    IEnumerator Bumbum()
    {
        yield return new WaitForSeconds(shootSpeed);
        Vector3 centerPosition = player.transform.position; 
        
        Vector3 randomPosition = GetRandomPositionInRegion(centerPosition, area);
        
        bismarckTarget.transform.position = randomPosition;
        StartCoroutine(BumbumTime());

    }

    IEnumerator BumbumTime()
    {
        yield return new WaitForSeconds(2);
        Instantiate(fire, bismarckTarget.transform.position,fire.transform.rotation);
        _start = true;
        Hit();
    }
    
    Vector3 GetRandomPositionInRegion(Vector3 center, float size)
    {
        float randomX = Random.Range(center.x - size / 2, center.x + size / 2);
        
        
        float randomZ = Random.Range(center.z - size / 2, center.z + size / 2);
        
        return new Vector3(randomX, 0.02f, randomZ);
    }
    
    private void Hit()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(bismarckTarget.position, 3, layerMask);

        foreach (Collider collider in hitColliders)
        {
            
            if (collider.CompareTag("cover"))
            {
                Debug.Log("hit");
                Cover coverComponent = collider.GetComponent<Cover>();
                if (coverComponent != null)
                {
                    coverComponent.hp = -1;
                    coverComponent.CheckHp();
                }
            }
        }
    }
    
    
    
    
}
