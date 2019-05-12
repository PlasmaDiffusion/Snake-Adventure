using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();

        if (rigidbody)
        {
            rigidbody.velocity = (new Vector3(rigidbody.velocity.x, 10.0f, rigidbody.velocity.z));
        }
    }
}
