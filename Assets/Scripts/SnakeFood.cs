using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFood : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 5.0f * Time.deltaTime, 0.0f));   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            //Add a snake segment
            Snake snake = other.GetComponent<Snake>();
            snake.AddSegment();
            snake.cam.transform.position += new Vector3(0.0f, 0.1f, 0.0f);

            //Notify the gate this was collected
            GameObject.Find("Gate").GetComponent<SnakeGate>().LowerGate();
            Destroy(gameObject);


        }
    }
}
