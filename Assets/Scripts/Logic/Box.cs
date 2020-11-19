using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private bool _hit = false;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Cop" && !_hit)
        {
            // collision.transform.GetComponent<BoxCollider>().enabled = false;
            collision.transform.GetComponent<Rigidbody>().velocity = Vector3.up * 10.0f;
            _hit = true;            
        }
    }

    private void OnCollisionEnter()
    {
        _rb.velocity = Vector3.zero;
    }
}
