using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStats : MonoBehaviour
{
    public static bool paused;

    public static int score;
    public static int requiredFood;
    public static int coins = 60;

    //Records
    static int hiscore;
    static int farthestLevel;

    public static GlobalHUD hud;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        paused = false;
        hud = GameObject.Find("HUD_Panel").GetComponent<GlobalHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(int level)
    {
        CheckForFarthestLevel(0);
        CheckForHiScore();
    }

    void CheckForHiScore()
    {

    }

    void CheckForFarthestLevel(int level)
    {
        hiscore = 0;
    }
}
