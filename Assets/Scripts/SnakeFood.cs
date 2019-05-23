using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFood : MonoBehaviour
{

    public static int objCount;

    // Start is called before the first frame update
    void Start()
    {
        objCount++;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 15.0f * Time.deltaTime, 0.0f));   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GlobalStats.score++;
            GlobalStats.hud.UpdateHUD();

            //Add a snake segment
            Snake snake = other.GetComponent<Snake>();
            snake.AddSegment();
            snake.ZoomOutCamera();


            //Notify the gate this was collected
            transform.parent.Find("Gate").GetComponent<SnakeGate>().LowerGate();
            Destroy(gameObject);


        }
    }

    private void OnDestroy()
    {
        objCount--;
    }
}
