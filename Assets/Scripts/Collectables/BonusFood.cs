using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns in a food mesh and applies a given bonus, set in the prefabs
public class BonusFood : MonoBehaviour
{
    //Variable that determines when food should level up
    public static int foodCollected = 0;
    //Current food level
    public static int currentFoodIndex = 0;

    int pointWorth;

    [Header("Set Meshes here (in order of themes)")]
    public GameObject[] otherFoodBonuses;
    [Header("Set Point Values here")]
    public int[] scoreBonuses;
    [Header("Set food required for next fruit here")]
    public int[] requiredFood;


    //Instantiate an empty child food object
    void Start()
    {
        //Make invisible in game but visible in the editor.
        GetComponent<MeshRenderer>().enabled = false;
        GameObject newFruit = Instantiate(otherFoodBonuses[currentFoodIndex], transform);

        //Angle the fruit a little
        newFruit.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -45.0f);

        //Determine what current fruit is and points are
        pointWorth = scoreBonuses[currentFoodIndex];

        //Some foods have specific angles
        if (currentFoodIndex == 0) newFruit.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        else if (currentFoodIndex == 1) newFruit.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90.0f);
        else if (currentFoodIndex == 3) newFruit.transform.rotation = Quaternion.Euler(45.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        else if (currentFoodIndex == 6) newFruit.transform.rotation = Quaternion.Euler(90.0f, transform.rotation.eulerAngles.y, 90.0f);

    }

    private void Update()
    {
        transform.Rotate(new Vector3(0.0f, 15.0f * Time.deltaTime, 0.0f));
    }

    //Player collects once
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            //Increase food collected. If enough food is collected then change to a better fruit
            foodCollected++;

            GlobalStats.AddScore(pointWorth, transform.position);
            CoinObjective.CheckForObjective((int)CoinObjective.Objective.GET_BONUS_FOOD);
            SoundManager.PlaySound(SoundManager.Sounds.FOOD_PICKUP);

            if (CanLevelUpFood()) LevelUpFood();

            Destroy(gameObject);
        }
    }

    bool CanLevelUpFood()
    {
        if (foodCollected >= requiredFood[currentFoodIndex])
        {
            return true;
        }
        return false;
    }

    void LevelUpFood()
    {
        if (currentFoodIndex < 6)
        {
            currentFoodIndex++;
            foodCollected = 0;
        }
    }
}
