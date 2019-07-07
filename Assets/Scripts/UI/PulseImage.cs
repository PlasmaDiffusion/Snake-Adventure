using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pulse the title screen image or any other image
public class PulseImage : MonoBehaviour
{
    float t;
    float increaseVal;
    Vector3 minScale;
    Vector3 maxScale;

    // Start is called before the first frame update
    void Start()
    {
        t = 0.0f;
        increaseVal = 1.0f;
        minScale = transform.localScale * 0.75f;
        maxScale = transform.localScale * 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        //Increase t
        t += Time.deltaTime * increaseVal;

        //Stop t from overshooting/undershooting, and swap it to decrease/increase.
        if (t > 1.0f)
        {
            increaseVal = -1.0f;
            t = 1.0f;
        }
        else if (t < 0.0f)
        {
            increaseVal = 1.0f;
            t = 0.0f;
        }

        transform.localScale = Vector3.Lerp(minScale, maxScale, t);
    }
}
