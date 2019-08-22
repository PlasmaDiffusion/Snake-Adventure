using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (GlobalStats.paused) return;

        transform.Rotate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
    }
}
