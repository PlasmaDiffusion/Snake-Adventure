using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains a list of prefabbed levels to spawn in. Levels are sorted into different difficulties.
public class LevelSpawner : MonoBehaviour
{
    int level;

    //Pool of levels to remove from to prevent duplicates
    List<GameObject> easyLevelPool;
    List<GameObject> mediumLevelPool;
    List<GameObject> hardLevelPool;

    //Hard coded list of all levels
    public List<GameObject> easyLevels;
    public List<GameObject> mediumLevels;
    public List<GameObject> hardLevels;

    GameObject prevLevel;
    GameObject currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;

        easyLevelPool = new List<GameObject>();
        mediumLevelPool = new List<GameObject>();
        hardLevelPool = new List<GameObject>();

        easyLevelPool.AddRange(easyLevels);
        mediumLevelPool.AddRange(mediumLevels);
        hardLevelPool.AddRange(hardLevels);
    }

    int getLevel() { return level; }

    public void EndLevel()
    {
        level++;
        List<GameObject> levelPool = DetermineLevelPool();

        //Despawn prevLevel if it exists
        if (prevLevel) DespawnOldLevel();


        SpawnInLevel(ref levelPool);

        //In a rare case there might be no levels left in a pool.
        CheckIfPoolIsEmpty();



        }

    //Easy, normal or hard levels?
    List<GameObject> DetermineLevelPool()
    {

        //Determine the difficulty to lean towards
        int difficultyBias = 1;

        if (level > 5) difficultyBias = 2;
        else if (level > 15) difficultyBias = 3;

        //1 in 3 chance for difficulty to be 1 level higher
        if (Random.Range(0, 2) == 2)
        {
            difficultyBias++;

            //...But if already at the max difficulty then lower to an easier one.
            if (difficultyBias == 4) difficultyBias -= Random.Range(2, 3);
        }

        Debug.Log("Difficulty: " + difficultyBias.ToString());

        if (difficultyBias >= 3) return hardLevelPool;
        if (difficultyBias == 2) return mediumLevelPool;
        else return easyLevelPool;
    }

    void CheckIfPoolIsEmpty()
    {
        if (easyLevelPool.Count == 0) easyLevelPool.AddRange(easyLevels);
        if (mediumLevelPool.Count == 0) mediumLevelPool.AddRange(mediumLevels);
        if (hardLevelPool.Count == 0) hardLevelPool.AddRange(hardLevels);
    }

    void SpawnInLevel(ref List<GameObject> levelList)
    {
        GameObject newLevel;

        int levelIndex = Random.Range(0, levelList.Count - 1);



        newLevel = Instantiate(levelList[levelIndex]);


        LevelProperties properties = newLevel.GetComponent<LevelProperties>();

        //Move the new level
        transform.position += properties.size;

        //New level goes a little bit down
        DeathCheck.deathY--;
        newLevel.transform.position = transform.position + new Vector3(0.0f, DeathCheck.deathY + 10, 0.0f);

        //Remove the given level from the pool. We don't want duplicates.
        levelList.RemoveAt(levelIndex);

        //Save the prev level to despawn
        prevLevel = currentLevel;
        currentLevel = newLevel;

    }

    void DespawnOldLevel()
    {
        Destroy(prevLevel);
    }
}
