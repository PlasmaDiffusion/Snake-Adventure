using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skins : MonoBehaviour
{
    public enum SnakeSkins
    {
        DEFAULT,
        BLUE,
        CHECKER,
        RANDOM
    };

    public enum Themes
    {
        FOREST,
        DESSERT,
        SNOW,
        SKY,
        SEA,
        CANYON,
        FLOWER,
        TOY,
        RANDOM
    };

    //List of all snake skins
    public static Material[] snakeSkins;
    //List of all wall materials
    public static Material[] levelThemeWallMaterials;
    //List of all floor materials
    public static Material[] levelThemeFloorMaterials;
    //List of all level theme objects.
    public static GameObject[] levelThemeObjects;

    public static SnakeSkins snakeSkin;
    public static Themes levelTheme;

    bool random;

    // Start is called before the first frame update
    void Start()
    {
        //By default randomizing isn't set. Unless the player wants random themes for each level.
        random = false;

        //Default level skins
        snakeSkin = SnakeSkins.DEFAULT;
        levelTheme = Themes.FOREST;

        //Load the last selected theme here.
    }

    //Call this once every level spawns in.
    void Randomize()
    {
        if (levelTheme == Themes.RANDOM || random)
        {
            random = true;
            levelTheme = (Themes)Random.Range(0, (int)Themes.RANDOM);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
