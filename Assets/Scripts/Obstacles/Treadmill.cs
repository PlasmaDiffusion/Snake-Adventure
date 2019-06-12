using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    Vector3 pushVector;

    Renderer rend;

    Vector2 offset;

    float offsetLimit;

    // Start is called before the first frame update
    void Start()
    {
        pushVector = transform.forward.normalized;
        pushVector *= 2.0f;

        rend = GetComponent<Renderer>();

        offset = new Vector2(0.0f, 0.0f);
        offsetLimit = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        //Make treadmill actually move the texture
        offset += new Vector2(0.0f, Time.deltaTime);

        //Reset treadmill when over 1.0
        if (offset.y > offsetLimit) offset = new Vector2(0.0f, -offsetLimit);

        //Set actual texture to move with the offset
        rend.material.SetTextureOffset("_MainTex", offset);
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (GlobalStats.paused) return;

        if (other.name == "Player" || other.tag == "Burnable")
        {
            other.transform.position += (pushVector * Time.deltaTime);
        }
    }
}
