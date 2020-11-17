using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : MonoBehaviour
{
    private bool _caught = false;

    private void FixedUpdate()
    {
        Watch();
    }

    void Watch()
    {
        if (Physics.Raycast(transform.position ,transform.forward, out RaycastHit hitInfo, 1.0f, LayerMask.NameToLayer("Player")) && !_caught)
        {
            hitInfo.transform.GetComponent<Player>().Stop();
            _caught = true;
            Debug.Log("Caught!");
        }
    }   
}
