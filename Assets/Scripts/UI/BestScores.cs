using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Put this on text to show high scores.
public class BestScores : MonoBehaviour
{

    [Header("Select one score type")]
    public bool bestScore;
    public bool bestLevel;
    public bool coinBonus;

    [Header("Swap to regular score")]
    //Check this to show regular scores instead
    public bool replaceWithRegularScore = false;


    Text numberText;

    void Start()
    {
        numberText = GetComponent<Text>();

        SetText();
    }
    

    //Set text to whatever score it's supposed to be
    public void SetText()
    {
        if (bestLevel)
        {

            if (replaceWithRegularScore)
            {
                numberText.text = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().GetLevel().ToString();
            }
            else numberText.text = GlobalStats.GetFarthestLevel().ToString();

        }
        else if (bestScore)
        {
            if (replaceWithRegularScore) numberText.text = GlobalStats.score.ToString();
            else numberText.text = GlobalStats.GetHiScore().ToString();
        }
        else if (coinBonus)
        {
            int coinBonus = GlobalStats.score / 50;
            numberText.text = coinBonus.ToString();
        }
    }
}
