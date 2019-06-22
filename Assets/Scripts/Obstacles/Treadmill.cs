using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    Vector3 pushVector;

    Renderer rend;

    Vector2 offset;

    float offsetLimit;

    Snake snake;

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

    //Force player to move in a direction. First just set the player variable.
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            snake = other.GetComponent<Snake>();
        }
    }

    //Then force the player to move constantly if they're still on it.
    private void OnTriggerStay(Collider other)
    {
        if (GlobalStats.paused) return;

        if (other.name == "Player")
        {
            snake.ForceDirection(pushVector);
        }
        else if (other.tag == "Burnable") //Older method that pushes rigid bodies.
        {
            other.transform.position += (pushVector * Time.deltaTime);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            snake.StopForcingDirection();
        }
    }

}
