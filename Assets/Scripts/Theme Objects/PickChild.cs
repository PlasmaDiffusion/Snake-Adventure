using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make only one of the children activated
public class PickChild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int childToPick = Random.Range(0, transform.childCount);

        transform.GetChild(childToPick).gameObject.SetActive(true);
     
    }
}
