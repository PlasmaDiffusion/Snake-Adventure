using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGate : MonoBehaviour
{
    int requirement;
    bool open;

    // Start is called before the first frame update
    void Start()
    {
        requirement = 10;
        open = false;
    }

    //Called whenever snake food is destroyed
    void LowerGate()
    {
        requirement--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && open)
        {
            //Spawn in more level here. Despawn the older level.
        }
    }
}
