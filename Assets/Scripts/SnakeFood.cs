using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFood : MonoBehaviour
{

    //Static vars for multipliers
    static int scoreMultiplier = 1;
    static float multiplierTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
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
            //Add onto the score! There could be a score multiplier too.
            GlobalStats.score+= scoreMultiplier;
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

    //Call this to add a multiplier for a certain number of seconds. Can stack.
    public static void AddMultiplier()
    {
        scoreMultiplier++;
        multiplierTime = 8.0f;
    }

    public static int GetScoreMultiplier() { return scoreMultiplier; }
    public static float GetMultiplierTime() { return multiplierTime; }

    //Snake class calls this every update.
    public static void CheckScoreMultiplier()
    {
        if (multiplierTime > 0.0f)
        {
            multiplierTime -= Time.deltaTime;
            if (multiplierTime <= 0.0f)
            {
                multiplierTime = 0.0f;
                scoreMultiplier = 1;
            }
        }

    }

}
