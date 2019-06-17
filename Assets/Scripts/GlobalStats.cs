using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GlobalStats : MonoBehaviour
{
    public static bool paused;

    public static int score, requiredFood;

    public static int coins = 35;

    static bool loadedSave = false;

    //Records
    static int hiscore, farthestLevel;

    public static GlobalHUD hud;
  

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        paused = true;
        hud = GameObject.Find("HUD_Panel").GetComponent<GlobalHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool CheckForHiScore()
    {
        if (score > hiscore)
        {
            hiscore = score;
            return true;
        }
        return false;
    }
    
    public static int GetHiScore() { return hiscore; }
    public static int GetFarthestLevel() { return farthestLevel; }

    public static bool CheckForFarthestLevel()
    {
        //Lot of level spawners here...
        LevelSpawner levelSpawner = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>();
        if (levelSpawner.GetLevel() > farthestLevel)
        {
            farthestLevel = levelSpawner.GetLevel();
            return true;
        }
        return false;
    }

    public static void Save()
    {
        Debug.Log("Saving...");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/snakeData.dat");
        GameSave data = new GameSave();


        //Save data involving best scores and skins
        data.coins = coins;
        data.hiScore = hiscore;
        data.farthestLevel = farthestLevel;

        data.currentSnakeIndex = (int)Skins.snakeSkin;
        data.currentThemeIndex = (int)Skins.levelTheme;

        data.unlockedSnakeSkins = Skins.unlockedSnakeSkins;
        data.unlockedLevelThemes = Skins.unlockedLevelThemes;

        data.randomSnake = Skins.randomSkin;
        data.randomTheme = Skins.randomTheme;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {
        Debug.Log("Loading...");

        if (File.Exists(Application.persistentDataPath + "/snakeData.dat"))
        {


            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/snakeData.dat", FileMode.Open);
            GameSave data = (GameSave)bf.Deserialize(file);

            file.Close();

            coins = data.coins;
            hiscore = data.hiScore;
            farthestLevel = data.farthestLevel;

            Skins.snakeSkin = (Skins.SnakeSkins)data.currentSnakeIndex;
            Skins.levelTheme = (Skins.Themes)data.currentThemeIndex;

            Skins.unlockedSnakeSkins = data.unlockedSnakeSkins;
            Skins.unlockedLevelThemes = data.unlockedLevelThemes;

            Skins.randomSkin = data.randomSnake;
            Skins.randomTheme = data.randomTheme;
        }
        else
        {
            Debug.LogWarning("File not found.");
        }
    }

    //Class for saving data
    [System.Serializable]
    class GameSave
    {
        //High scores and coins
        public int coins;
        public int hiScore;
        public int farthestLevel;

        //Skins
        public int currentSnakeIndex;
        public int currentThemeIndex;

        public bool[] unlockedSnakeSkins;
        public bool[] unlockedLevelThemes;

        public bool randomSnake;
        public bool randomTheme;
    }
}
