using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Extra coin objective class. Like random "daily" tasks that reward bonus coins.
public class CoinObjective : MonoBehaviour
{
    //Number for what the current goal is
    static int objectiveID;
    //Number for sub goals, i.e. beating a level with a specific theme
    static int subObjectiveID;

    //Progress amount increases
    static int progressAmount;
    static int goalAmount;
    static bool achieved;
    

    // Start is called before the first frame update
    void Awake()
    {
        achieved = false;
        PickRandomObjective();

    }

    /*
     * 0: Get 10 bonus fruit
     * 1: Beat 10 levels at a random unlocked theme
     * 2: Use the fire powerup to burn 5 enemies
     * 3: Activate boost 5 times
     * 4: Get any powerup while boosting 2 times
     * 5: Find a specific powerup 3 times
     */

    public enum Objective
    {
        GET_BONUS_FOOD,
        BEAT_THEMED_LEVELS,
        DESTROY_WITH_FIRE,
        BOOST,
        BOOST_POWERUP,
        FIND_POWERUP
    }

    static Objective Objectives;

    public string[] descriptions;

    //Call this once when the game opens or the current objective is cleared.
    void PickRandomObjective()
    {
        //Reset progress/goal variables
        progressAmount = 0;
        goalAmount = 0;
        subObjectiveID = -1;

        //Pick a random objective
        objectiveID = Random.Range(0, (int)Objective.FIND_POWERUP +1);

        //Make sure theme is unlocked when using a beat level theme objective
        if (objectiveID == (int)Objective.BEAT_THEMED_LEVELS)
        {
            //Make sure theme exists
            List<Skins.Themes> themesAvailable = Skins.GetThemePool();

            //Now select a random element from that list of unlocked themes
            subObjectiveID = (int)themesAvailable[Random.Range(0, themesAvailable.Capacity)];
        }


        if (objectiveID == (int)Objective.FIND_POWERUP)
            subObjectiveID = Random.Range(0, 3);



        switch (objectiveID)
        {
            case 0:
            case 1:
                goalAmount = 10;
                break;
            case 2:
            case 3:
                goalAmount = 5;
                break;
            case 4:
                goalAmount = 2;
                break;
            case 5:
                goalAmount = 3;
                break;
        }
    }

    //Call this to check if an objective is being met or progressed. If that objectiveID is the current task, then increase the progress on it.
    public static void CheckForObjective(int objectiveChecked, int subObjectiveCheked = -1)
    {
        //Is the right objective set?
        if (objectiveID == objectiveChecked)
        {
            //If needed, is the right subobjective set?
            if (subObjectiveCheked != -1 && subObjectiveCheked == subObjectiveID)
            {
                Debug.Log("Objective progressed!");
                progressAmount++;
                if (progressAmount >= goalAmount)
                {
                    Debug.Log("Objective met!");
                    achieved = true;
                }
            }
        }
    }

    public void CashInPrize()
    {
        GlobalStats.coins += 20;
        achieved = false;
        PickRandomObjective();
    }

    void OverwriteDescription()
    {
        //int asteriskIndex = 0;

        //string currentDesc = descriptions[objectiveID];

        //for(int i = 0; i < currentDesc.Length; i++)
        //{
        //    if (currentDesc[i] == '*')
        //    {
        //        asteriskIndex = i;
        //        break;
        //    }
        //}

        //Replace . with number
        descriptions[objectiveID] = descriptions[objectiveID].Replace(".", goalAmount.ToString());

        //Replace * with name of subobjective

        //Level theme objective
        if (objectiveID == (int)Objective.BEAT_THEMED_LEVELS)
            descriptions[objectiveID] = descriptions[objectiveID].Replace("*", Skins.Themes.BOUNCY.ToString().ToLower());

        //Powerup objective
        if (objectiveID == (int)Objective.FIND_POWERUP)
        {
            if (subObjectiveID == 0)
                descriptions[objectiveID] = descriptions[objectiveID].Replace("*", "Ghost");
            else if (subObjectiveID == 1)
                descriptions[objectiveID] = descriptions[objectiveID].Replace("*", "Pellet Multiplier");
            else if (subObjectiveID == 2)
                descriptions[objectiveID] = descriptions[objectiveID].Replace("*", "Fire");
        }
    }
}
