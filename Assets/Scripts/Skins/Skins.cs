﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skins : MonoBehaviour
{
    public enum SnakeSkins
    {
        DEFAULT,
        BLUE,
        MECHANICAL,
        SCALES,
        SPOTS,
        DICE,
        DOTTED,
        CHECKERED,
        RANDOM
    };

    public enum Themes
    {
        DEFAULT,
        SNOW,
        FACTORY,
        RUINS,
        BOUNCY,
        DESERT,
        TOYBOX,
        MOON,
        CITY,
        CAVE,
        DUNGEON,
        WATER,
        RANDOM
    };

    //List of all snake skins
    public Material[] snakeSkins;
    //List of all wall materials
    public Material[] levelThemeWallMaterials;
    //List of all floor materials
    public Material[] levelThemeFloorMaterials;
    //List of all level theme objects.
    public GameObject[] levelThemeObjects;


    //Currently select Skins
    public static SnakeSkins snakeSkin;
    public static Themes levelTheme;

    //Something that determines what has and hasn't been bought. (Needs to be saved)
    public static bool[] unlockedSnakeSkins;
    public static bool[] unlockedLevelThemes;


    static List<Themes> themePool;
    static List<SnakeSkins> skinPool;

    public static bool randomTheme;
    public static bool randomSkin;

    static bool firstTimeStartup;

    public static int skinCost;
    public static int themeCost;

    private void Awake()
    {
        firstTimeStartup = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize everything here but only the first time because static variables
        if (!firstTimeStartup)
        {
            Debug.Log(Application.persistentDataPath);
            //By default randomizing isn't set. Unless the player wants random themes for each level.
            randomTheme = false;

            //Default level skins
            snakeSkin = SnakeSkins.DEFAULT;
            levelTheme = Themes.DEFAULT;

            //Initialize unlock skin arrays
            unlockedSnakeSkins = new bool[(int)SnakeSkins.RANDOM + 1];
            unlockedLevelThemes = new bool[(int)Themes.RANDOM + 1];

            unlockedSnakeSkins[(int)SnakeSkins.RANDOM] = true;
            unlockedSnakeSkins[(int)SnakeSkins.DEFAULT] = true;

            unlockedLevelThemes[(int)Themes.RANDOM] = true;
            unlockedLevelThemes[(int)Themes.DEFAULT] = true;


            //Load in all unlocked skins here (And everything else)
            GlobalStats.Load();

            //Make a list of the unlocked skins for randomization
            themePool = new List<Themes>();
            skinPool = new List<SnakeSkins>();
            CreateRandomSkinPoolList();
            CheckForRandomization();
            GlobalStats.hud.UpdateHUD();
            firstTimeStartup = true;
            UpdatePlayerSkin();
        }
        else
        {
            CheckForRandomization();
            UpdatePlayerSkin();
        }

        CalculateCosts();
    }

    void CalculateCosts()
    {
        //int[] costs = { 40, 50, 60, 80, 100, 150, 200, 300, 500};

        skinCost = 40;
        themeCost = 40;

        for (int i = 0; i < (int)Themes.RANDOM; i++) if (unlockedLevelThemes[i]) themeCost += (int)(themeCost * 0.5);

        for (int i = 0; i < (int)SnakeSkins.RANDOM; i++) if (unlockedSnakeSkins[i]) skinCost += (int)(skinCost * 0.5);
    }

    void UpdatePlayerSkin()
    {
        ////Make sure the snake is actually randomized
        Snake snake = GameObject.Find("Player").GetComponent<Snake>();
        if (snake) snake.ChangeSnakeSkin();
    }

    //Get a list of all unlocked skins to choose from when randomizing skins.
    public void CreateRandomSkinPoolList()
    {
        skinPool.Clear();
        themePool.Clear();

        for (int i = 0; i < (int)Themes.RANDOM; i++) if (unlockedLevelThemes[i]) themePool.Add((Themes)i);

        for (int i = 0; i < (int)SnakeSkins.RANDOM; i++) if (unlockedSnakeSkins[i]) skinPool.Add((SnakeSkins)i);
    }

    //Call this once every level spawns in.
    public static void CheckForRandomization(bool themesOnly = false)
    {
        //Randomize level themes if needed. Snake can turn this off.
        if ((levelTheme == Themes.RANDOM || randomTheme))
        {
            randomTheme = true;
            levelTheme = themePool[Random.Range(0, themePool.Count)];
            Debug.Log("Themes should be random now.");
        }
        //Randomize snake skins if needed
        if (snakeSkin == SnakeSkins.RANDOM || randomSkin && !themesOnly)
        {
            randomSkin = true;
            snakeSkin =  skinPool[Random.Range(0, skinPool.Count)];
            Debug.Log("Skins should be random now.");
        }
    }

    public static void CheckToTurnOffRandom()
    {

        if (snakeSkin != SnakeSkins.RANDOM) randomSkin = false;
        if (levelTheme != Themes.RANDOM) randomTheme = false;


        Debug.Log("Checking to turn off random. Random Snakes: " + randomSkin.ToString() + " Random Themes: " + randomTheme.ToString());
    }

    public static bool HaveAllSnakeSkins()
    {

        foreach (bool unlockFlag in unlockedSnakeSkins)
        {
            if (!unlockFlag)
            {
                return false;
            }
        }

        return true;
    }

    public static bool HaveAllLevelThemes()
    {

        foreach (bool unlockFlag in unlockedLevelThemes)
        {
            if (!unlockFlag)
            {
                return false;
            }
        }

        return true;
    }

    public static List<Themes> GetThemePool() { return themePool; }
}
