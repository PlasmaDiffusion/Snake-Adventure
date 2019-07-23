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

    [Header("Skin Present Screen")]
    public GameObject skinGetMenu;

    // Start is called before the first frame update
    void Start()
    {
        skinObj = GameObject.Find("SkinHandler").GetComponent<Skins>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Buy);
        text = transform.GetChild(0).GetComponent<Text>();

        //Auto set costs
        if (cost <= 0)
        {
            if (isTheme) cost = Skins.themeCost;
            else if (isSnake) cost = Skins.skinCost;
        }

        //Make sure there are actual skins/themes left to buy!
        if (isTheme)
        {
            if (Skins.HaveAllLevelThemes()) GreyOutButton();
        }
        else
        {
            if (Skins.HaveAllSnakeSkins()) GreyOutButton();
        }

        globalStats = GameObject.Find("LevelSpawner").GetComponent<GlobalStats>();

        //Update price tag
        text.text = text.text.Replace(".", cost.ToString());
    }

    //Disable buying cause the player owns everything already. :(
    void GreyOutButton()
    {
        button.onClick.RemoveAllListeners();
        if (isSnake) text.text = "Own all \nSnakes!";
        else text.text = "Own all \nThemes!";

        //Also actually grey out the button duh
        button.GetComponent<Image>().color = Color.grey;
    }

    //Check if the player has the coins. If so then buy a random skin in Purchase().
    void Buy()
    {
        if (GlobalStats.coins >= cost)
            {
                Purchase();
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


        if (isSnake)
        {
            //text.text = "New Snake!\n";
            text.text = ((Skins.SnakeSkins)randomIndex).ToString();
        }
        else
        {
            //text.text = "New Theme!\n";
            text.text = ((Skins.Themes)randomIndex).ToString();
        }
        
        //Update coin count on HUD
        GlobalStats.hud.UpdateHUD();

        //Recolour buttons
        UpdateButtons();

        //-------------------------------------------------------------------------------------
        //Open up the skin get screen here.
        //Makes some of the above visual stuff kind of irrelevant but keep it just in case.
        
        SkinGet skinGetScript = skinGetMenu.GetComponent<SkinGet>();

        if (isSnake)
        {
           skinGetScript.SetSnakeIcon(text.text, ((Skins.SnakeSkins)randomIndex));
        }
        else
        {
            skinGetScript.SetThemeIcon(text.text, ((Skins.Themes)randomIndex));
        }

        skinGetMenu.SetActive(true);
        //-------------------------------------------------------------------------------------
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

        for (int i = 0; i < (int)Skins.SnakeSkins.RANDOM; i++)
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
