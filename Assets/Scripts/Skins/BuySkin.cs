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

    GlobalStats globalStats;

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

        globalStats = GameObject.Find("LevelSpawner").GetComponent<GlobalStats>();
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
                Purchase();
            }
        }
        else
        {

            if (GlobalStats.coins >= cost)
            {
                Purchase();
            }

        }

    }

    private void Purchase()
    {
        GlobalStats.coins -= cost;


        int randomIndex;

        //Get a random unlockable.
        if (isSnake) randomIndex = UnlockRandomSnakeIndex();
        else randomIndex = UnlockRandomLevelThemeIndex();

        if (isSnake ) Skins.unlockedSnakeSkins[randomIndex] = true;
        else Skins.unlockedLevelThemes[randomIndex] = true;

        //Update both pools
        skinObj.CreateRandomSkinPoolList();

        //Show feedback for what you just bought
        if (isSnake) text.text = "New Snake! \n";
        else text.text = "New Theme! \n";
        text.text += ((Skins.Themes)randomIndex);
        
        //Update coin count on HUD
        GlobalStats.hud.UpdateHUD();

        //Recolour buttons
        UpdateButtons();

        //Save game so skin is here for good.
        GlobalStats.Save();
        //globalStats.Save();
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
