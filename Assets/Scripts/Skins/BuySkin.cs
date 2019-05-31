using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuySkin : MonoBehaviour
{
    Button button;
    Text text;

    public bool isTheme;
    public bool isSnake;

    //If 0 it's set to the default cost
    public int cost = 0;

    Skins skinObj;

    // Start is called before the first frame update
    void Start()
    {
        skinObj = GameObject.Find("SkinHandler").GetComponent<Skins>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Buy);
        text = transform.GetChild(0).GetComponent<Text>();

        if (cost <= 0)
        {
            if (isTheme) cost = 50;
            else if (isSnake) cost = 40;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Buy()
    {
        if (isSnake)
        {
            if (GlobalStats.coins >= cost)
            {
                GlobalStats.coins -= cost;


                

                Skins.unlockedSnakeSkins[UnlockRandomLevelThemeIndex()] = true;
            }
        }
        else
        {

            if (GlobalStats.coins >= cost)
            {
                GlobalStats.coins -= cost;


                int randomIndex = UnlockRandomLevelThemeIndex();

                Skins.unlockedLevelThemes[randomIndex] = true;

                skinObj.CreateRandomSkinPoolList();

                //Show feedback for what you just bought
                text.text = "New Theme! \n" + ((Skins.Themes)randomIndex);
                //Update coin count on HUD
                GlobalStats.hud.UpdateHUD();

                //Recolour buttons
                UpdateButtons();
            }

        }

    }

    //Go through the list of all themes yet to be unlocked.
    int UnlockRandomLevelThemeIndex()
    {
        //Prepare a list of all new indexes
        List<int> potentialIndexes;
        potentialIndexes = new List<int>();

        for (int i = 0; i < (int)Skins.Themes.RANDOM; i++)
        {
            if (!Skins.unlockedLevelThemes[i])
                potentialIndexes.Add(i);

        }

        return potentialIndexes[Random.Range(0, potentialIndexes.Count)];
    }

    //Go through the list of all skins yet to be unlocked.
    int UnlockRandomSnakeIndex()
    {
        //Prepare a list of all new indexes
        List<int> potentialIndexes;
        potentialIndexes = new List<int>();

        for (int i = 0; i < (int)Skins.Themes.RANDOM; i++)
        {
            if (!Skins.unlockedSnakeSkins[i])
                potentialIndexes.Add(i);

        }

        return potentialIndexes[Random.Range(0, potentialIndexes.Count)];
    }

    void UpdateButtons()
    {
        ChangeSkin[] buttons = GameObject.Find("ButtonPanel").transform.GetComponentsInChildren<ChangeSkin>();

        Debug.Log("Btn Length: " + buttons.Length);

        foreach (ChangeSkin buttonScript in buttons)
        {
            buttonScript.RecolourIfUnlocked();
        }

    }

}
