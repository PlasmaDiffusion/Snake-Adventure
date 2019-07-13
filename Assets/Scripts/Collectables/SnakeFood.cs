using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFood : MonoBehaviour
{

    //Static vars for multipliers
    static int scoreMultiplier = 1;
    static float multiplierTime = 0.0f;

    //Sound effect pitch variable
    static float currentPitch = 1.0f;

    //Delegate for food spawner to react to when this is collected.
    public delegate void OnCollect();
    public OnCollect ExtraCollectEvent;

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
            GlobalStats.AddScore(scoreMultiplier * 10, transform.position);

            //Play the grow sound and up the pitch each time
            SoundManager.PlaySound(SoundManager.Sounds.GROW, currentPitch);
            currentPitch+= 0.1f;
            if (currentPitch > 2.0f) currentPitch = 1.0f;

            //Add a snake segment
            Snake snake = other.GetComponent<Snake>();
            snake.AddSegment();
            snake.ZoomOutCamera();
            snake.IncreaseBoostGuage();

            //If a classic snake collectable then notify the food spawner
            if (ExtraCollectEvent != null)
                ExtraCollectEvent();

            //Notify the gate this was collected
            transform.parent.Find("Gate").GetComponent<SnakeGate>().LowerGate();
            Destroy(gameObject);
        }
        else if (other.name == "Marker") Destroy(other.gameObject); //Destroy a food marker if the food spawned from it.
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
        if (multiplierTime > 0.0f && !Snake.boosting)
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
