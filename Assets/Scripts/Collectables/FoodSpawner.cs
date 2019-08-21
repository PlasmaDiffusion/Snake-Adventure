using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns in food in a specific order. Waits for the player to eat one to spawn the next, like in a classic game of Snake.
public class FoodSpawner : MonoBehaviour
{
    public int maxFood;
    int foodEaten;

    //Make a list of child objects. Whenever something is spawned in, take it from this list.
    List<GameObject> foodLeftToSpawn;

    LookAtCube pointer;

    LevelSpawner levelSpawner;

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Renderer>().enabled = false;

        foodEaten = 0;
        foodLeftToSpawn = new List<GameObject>();

        //Set children in list
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            //Only add food into the list (Not markers!)
            if (child.name[0] == 'F')
            foodLeftToSpawn.Add(child);
        }

        //Disable all children so they can wait for their turn
        for (int i = 0; i < foodLeftToSpawn.Count; i++)
        {
            //Also set them to trigger ActivateFood() upon them being collected.
            foodLeftToSpawn[i].GetComponent<SnakeFood>().ExtraCollectEvent = new SnakeFood.OnCollect(ActivateFood);
            foodLeftToSpawn[i].SetActive(false);
        }

        pointer = GameObject.Find("CubeArrow").GetComponent<LookAtCube>();
        levelSpawner = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>();

        ActivateFood();
    }

    //Spawn a random piece of food
    void ActivateFood()
    {

        if (foodEaten < maxFood)
        {

            int objIndex = Random.Range(0, foodLeftToSpawn.Count);

            //Spawn food in by making it active.
            foodLeftToSpawn[objIndex].SetActive(true);

            //Make arrow point to the new piece of food
            if (levelSpawner.GetLevel() > 0 && levelSpawner.GetLevelObject().name != "EasyLevel3(Clone)")
                pointer.cubeRef = foodLeftToSpawn[objIndex];

            //Set it to the gate parent
            foodLeftToSpawn[objIndex].transform.parent = transform.parent;

            //Remove from list now
            foodLeftToSpawn.RemoveAt(objIndex);

            foodEaten++;

        }
        else Destroy(gameObject); //Destroy self when max food amount is collected
    }
}
