using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    static bool objectiveSet = false;

    //UI Variables
    public GameObject panelObject;
    public Image progressBar;
    public Text descriptionText;
    public Button claimRewardButton;
    public GameObject coinBonusGroup;

    //Lerp into view vars
    float t;
    Vector3 targetPos;
    Vector3 startingPos;
    float tMultiplier;

    // Start is called before the first frame update

    void Start()
    {

        //Set the objective once upon starting the game. Set it again only when the objective is finished.
        if (!objectiveSet)
        {

            achieved = false;
            PickRandomObjective();
            objectiveSet = true;
            //Debug.Log("Setting objective");
            
        }

        //Debug.Log(((Objective)objectiveID).ToString());

        UpdateUI();

        //Lerp stuff
        targetPos = panelObject.transform.position;
        startingPos =  new Vector3(targetPos.x * -2.0f, targetPos.y, targetPos.z);
        t = 0.0f;
        tMultiplier = 0.0f;
        panelObject.transform.position = startingPos;

        if (GlobalStats.disabledAds) claimRewardButton.transform.GetChild(0).GetComponent<Text>().text = "+80 Coins";

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
            subObjectiveID = (int)themesAvailable[Random.Range(0, themesAvailable.Count)];
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


        objectiveSet = true;
    }

    //Call this to check if an objective is being met or progressed. If that objectiveID is the current task, then increase the progress on it.
    public static void CheckForObjective(int objectiveChecked, int subObjectiveCheked = -1)
    {
        //Is the right objective set?
        if (objectiveID == objectiveChecked)
        {
            //If needed, is the right subobjective set?
            if (subObjectiveCheked == -1 || subObjectiveCheked == subObjectiveID)
            {
                progressAmount++;
                if (progressAmount >= goalAmount)
                {
                    SoundManager.PlaySound(SoundManager.Sounds.OBJECTIVE_ACHIEVED, 1.0f, -1);
                    achieved = true;
                }
                else
                {
                    SoundManager.PlaySound(SoundManager.Sounds.OBJECTIVE_PROGRESSED, 1.0f, -1);
                }
            }
        }
    }

    public void CashInPrize()
    {
        //Debug.Log("PRIZE LOL");
        //coinBonusGroup.SetActive(true);
        SoundManager.PlaySound(SoundManager.Sounds.COIN, 1.5f);
        achieved = false;
        PickRandomObjective();
        UpdateUI();

        if (GlobalStats.disabledAds) GlobalStats.coins += 80;
        else GlobalStats.coins += 40;

        GlobalStats.hud.DisplayCoins(false);

        GlobalStats.Save();
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
            descriptions[objectiveID] = descriptions[objectiveID].Replace("*", ((Skins.Themes)subObjectiveID).ToString().ToLower());

        //Powerup objective
        if (objectiveID == (int)Objective.FIND_POWERUP)
        {
            if (subObjectiveID == 0)
                descriptions[objectiveID] = descriptions[objectiveID].Replace("*", "Ghost");
            else if (subObjectiveID == 1)
                descriptions[objectiveID] = descriptions[objectiveID].Replace("*", "Pellet Multiplier");
            else if (subObjectiveID == 2)
                descriptions[objectiveID] = descriptions[objectiveID].Replace("*", "Fire");

            Debug.Log("Powerup objective picked. Here's the subobjective id: " + subObjectiveID.ToString());
        }


    }

    private void UpdateUI()
    {

        //Add in progress
        progressBar.fillAmount = (float)progressAmount / goalAmount;

        //Prepare description text
        OverwriteDescription();
        descriptionText.text = descriptions[objectiveID] + " " + progressAmount.ToString() + " / " + goalAmount.ToString();

        //achieved = true;
        //Enable button to cash in winnings
        if (achieved)
        {
            claimRewardButton.gameObject.SetActive(true);
        }
        else
        {
            claimRewardButton.gameObject.SetActive(false);
        }
    }

    //Lerp into view
    private void Update()
    {
        if (t < 1.0f && panelObject && tMultiplier != 0.0f)
        {
            t += Time.deltaTime * tMultiplier;
            if (t > 1.0f) t = 1.0f;
            else if (t < 0.0f) { t = 0.0f; tMultiplier = 0.0f; }

            panelObject.transform.position = Vector3.Lerp(startingPos, targetPos, t);
        }
    }

    public void ToggleView()
    {
        if (tMultiplier == 0.0f) tMultiplier = 1.0f;
        else if (tMultiplier == 1.0f) { tMultiplier = -1.0f; t = 0.9f; }
        else if (tMultiplier == -1.0f) tMultiplier = 1.0f;
    }
}
