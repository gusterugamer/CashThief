using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : MonoBehaviour
{
    // Start is called before the first frame update

    private void FixedUpdate()
    {
        Watch();
    }

    void Watch()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1.0f, LayerMask.NameToLayer("Player")))
        {
            Debug.Log("HIT");
        }
    }
}
