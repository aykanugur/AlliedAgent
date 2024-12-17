using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzerVisual : MonoBehaviour
{
    private Transform _player;
    private bool _bomb;
    public GameObject[] parts;
    private float _distance;
    public bool random;
    private bool _bum;
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _bomb = false;
        if (random)
        {
            if (Random.Range(0, 5) == 2)
            {
                _bum = true;
            }
            else
            {
                _bum = false;
            }
        }
        else
        {
            _bum = true;
        }
    }

    private void FixedUpdate()
    {
        if (_bomb == false)
        {
            transform.Translate(new Vector3(0f,0,0.15f));
            _distance = Vector3.Distance(_player.position, transform.position);
        }
        //170
        if (_distance < 70 && _bomb == false && _bum)
        {
            _bomb = true;
            parts[0].GetComponent<Rigidbody>().AddForce(new Vector3(100,1000,10));
            parts[1].SetActive(true);
            parts[2].SetActive(true);
        }

        
    }
}
