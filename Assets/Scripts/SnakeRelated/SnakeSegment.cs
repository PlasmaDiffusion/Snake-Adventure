using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    Renderer rend;
    public GameObject snakeOwner;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (GlobalStats.paused) return;

        //Die if in segment for too long
        if (other.name == "Player" && name != "First Segment")
        {
            //Player boosting onto a snake segment?
            if (name[0] == 'E')
            {
                if (other.GetComponent<Snake>().GetBoosting())
                {
                    //Spawn death particles
                    GameObject emitter = GameObject.Find("DeathParticleEmitter");

                    if (emitter) Instantiate(emitter, other.transform.position, emitter.transform.rotation);
                    
                    Destroy(snakeOwner);
                }
            }

            //Hurt the player
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
