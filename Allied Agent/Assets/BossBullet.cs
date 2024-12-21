using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage;

    private Rigidbody rb;
    private Transform tf;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (other.gameObject.GetComponent<HpManager>() != null)
                {
                    other.gameObject.GetComponent<HpManager>().Decreasehp(damage);
                    Destroy(this.gameObject);
                }
                break;
            
            case  "cover":
                if (other.gameObject.GetComponent<Cover>() != null)
                {
                    if (other.gameObject.GetComponent<HpManager>() != null)
                    {
                        other.gameObject.GetComponent<HpManager>().Decreasehp(damage);
                        other.GetComponent<Cover>().CheckHp();
                        Destroy(this.gameObject);
                    }
                }
                break;
            
            default:
                Destroy(gameObject);
                break;
        }
    }
}
