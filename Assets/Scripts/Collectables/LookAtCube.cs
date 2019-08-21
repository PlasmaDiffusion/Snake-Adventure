using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pointer for the randomly placed cubes
public class LookAtCube : MonoBehaviour
{
    GameObject snakeRef;
    public GameObject cubeRef;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        snakeRef = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Face the player or be offscreen
        if (cubeRef && Vector3.Distance(snakeRef.transform.position, cubeRef.transform.position) > 10.0f)
        {
            transform.position = snakeRef.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
            transform.LookAt(cubeRef.transform);
            transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
        }
        else transform.position = startPosition;
    }

    public void RemoveRefernce()
    {
        cubeRef = null;
    }
}
