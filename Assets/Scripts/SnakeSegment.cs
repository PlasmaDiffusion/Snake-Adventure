using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    

    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        //Die if in segment for too long
        if (other.name == "Player" && name != "First Segment")
        {
            other.GetComponent<DeathCheck>().CheckForDeath();
            //rend.material.color = rend.material.color - new Color(0.0f, 0.0f, 0.0f, 0.5f);
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            other.GetComponent<DeathCheck>().StopCheck();

            //rend.material.color = rend.material.color + new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
