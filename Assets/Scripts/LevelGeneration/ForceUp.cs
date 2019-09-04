using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hitbox that forces the player up. Used for any glitch failsafes
public class ForceUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            other.transform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        }
    }
}
