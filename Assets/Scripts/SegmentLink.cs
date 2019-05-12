using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scale a link towards another snake segment.
public class SegmentLink : MonoBehaviour
{

    public GameObject targetObject;
    public GameObject targetObject2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Vector3 difference = targetObject2.transform.position - targetObject.transform.position;
       Vector3 direction = difference / difference.magnitude;
        
        transform.position = (targetObject2.transform.position + targetObject.transform.position) / 2.0f;
        transform.rotation = Quaternion.Euler(Mathf.Rad2Deg*direction);
        transform.localScale = new Vector3(0.25f, 0.25f, 0.25f) + difference;

        //Debug.Log(difference);
    }
}
