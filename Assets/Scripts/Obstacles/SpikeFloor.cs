using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFloor : MonoBehaviour
{
    float lerpT;
    float outTime;
    bool poppedOut;
    Vector3 tuckedOutPosition;
    Vector3 tuckedInPosition;

    // Start is called before the first frame update
    void Start()
    {
        outTime = 0.0f;
        lerpT = 0.0f;
        poppedOut = false;
        tuckedInPosition = transform.position - new Vector3(0.0f, 1.0f, 0.0f);
        tuckedOutPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Axe ignores this update
        if (name[0] == 'A' || GlobalStats.paused) return;


        //Spikes are popped out for a brief time. If done popping out lerp back in our lerp out.
        if (outTime <= 0.0f)
        {
            lerpT += Time.deltaTime;
            if (lerpT > 1.0f)
            {
                lerpT = 0.0f;
                outTime = 2.0f;
                poppedOut = !poppedOut;
            }
            
        }
        else
        {
            outTime -= Time.deltaTime;
        }

        if(poppedOut)
        {
            transform.position = Vector3.Lerp(tuckedOutPosition, tuckedInPosition, lerpT);
        }
        else
        {
            transform.position = Vector3.Lerp(tuckedInPosition, tuckedOutPosition, lerpT);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            other.GetComponent<DeathCheck>().Die();
        }
    }


}
