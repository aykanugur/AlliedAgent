
using System;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public bool isTracer = false;
    public TrailRenderer trail;
    public float tracerTimeout = 3.5f;
    public GameObject blood;
    private Rigidbody rb;
    private Transform tf;
    public float hp;
    public bool enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        tf=GetComponent<Transform>();
        trail.emitting=isTracer;
    }

    // Update is called once per frame
    void Update()
    {
        tracerTimeout-=Time.deltaTime;
        trail.emitting=isTracer && tracerTimeout>=0;
    }

    void FixedUpdate()
    {
        PointNoseToMovementVector();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                if (enemy == false)
                {
                    GameObject child = Instantiate(blood, other.transform);
                    var transform1 = transform;
                    child.transform.position = new Vector3(other.transform.position.x,transform1.position.y,transform1.position.z);
                    child.transform.eulerAngles = transform1.eulerAngles;
                    child.transform.SetParent(other.transform);
                    if (other.gameObject.GetComponent<HpManager>() != null)
                    {
                        other.gameObject.GetComponent<HpManager>().Decreasehp(hp);
                    }
                    Destroy(this.gameObject);
                }
                break;
            
            case "Reflective":
                ReflectBullet(other.gameObject.transform.forward);
                break;
            
            case  "cover":
                if (other.gameObject.GetComponent<Cover>() != null)
                {
                    if (other.gameObject.GetComponent<HpManager>() != null)
                    {
                        other.gameObject.GetComponent<HpManager>().Decreasehp(hp);
                        other.GetComponent<Cover>().CheckHp();
                    }
                    Destroy(this.gameObject);
                }
                break;
            case "DoNotDestroy":
                break;
            case "ammo":
                break;
            case "CameraDontMoveZone":
                break;
            case "Player":
                if (enemy)
                {
                    GameObject child = Instantiate(blood, other.transform);
                    var transform1 = transform;
                    child.transform.position = new Vector3(other.transform.position.x,transform1.position.y,transform1.position.z);
                    child.transform.eulerAngles = transform1.eulerAngles;
                    child.transform.SetParent(other.transform);
                    if (other.gameObject.GetComponent<HpManager>() != null)
                    {
                        other.gameObject.GetComponent<HpManager>().Decreasehp(hp);
                    }
                    Debug.Log("playerhit");
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private void ReflectBullet(Vector3 normal)
    {
        Vector3 direction = rb.velocity.normalized;
        direction = Vector3.Reflect(direction, normal);
        rb.velocity = direction * rb.velocity.magnitude;
    }
    
    private void PointNoseToMovementVector()
    {
        Vector3 direction = rb.velocity.normalized;
        tf.up = direction;
    }

    public void MakeTracer()
    {
        this.isTracer=true;
    }

    public void makeNonTracer()
    {
        this.isTracer=false;
    }
}

