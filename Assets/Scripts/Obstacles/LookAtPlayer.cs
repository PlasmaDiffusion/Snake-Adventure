using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform playerTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        if (!playerTransform) Destroy(gameObject);
    }

    //Look at the player every frame
    void Update()
    {

        if (playerTransform.position.y <= transform.position.y) transform.LookAt(playerTransform);

    }
}
