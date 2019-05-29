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
        SNOW,
        DESSERT,
        SKY,
        SEA,
        CANYON,
        FLOWER,
        TOY,
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

    public static SnakeSkins snakeSkin;
    public static Themes levelTheme;

    bool random;

    // Start is called before the first frame update
    void Start()
    {
        //By default randomizing isn't set. Unless the player wants random themes for each level.
        random = false;

        //Default level skins
        snakeSkin = SnakeSkins.BLUE;
        levelTheme = Themes.SNOW;

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
