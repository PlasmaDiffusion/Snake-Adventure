using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    public Vector3 maxScale;
    public Vector3 minScale;

    // Start is called before the first frame update
    void Start()
    {
        float scaleX = Random.Range(minScale.x, maxScale.x);
        float scaleY = Random.Range(minScale.y, maxScale.y);
        float scaleZ = Random.Range(minScale.z, maxScale.z);

        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
    
}
