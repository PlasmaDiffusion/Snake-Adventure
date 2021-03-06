﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Class with functions called when a skin is selected in the menu
public class ChangeSkin : MonoBehaviour
{
    Skins skinObject;

    Button button;
    Image image;
    Image childImage;
    public Skins.Themes themeToActivate;
    public Skins.SnakeSkins skinToActivate;

    public bool isLevelTheme;

    Color unlockedColour;

    bool initialized =false;

    // Start is called before the first frame update
    void Start()
    {
        skinObject = GameObject.Find("SkinHandler").GetComponent<Skins>();

        button = GetComponent<Button>();
        image = GetComponent<Image>();
        childImage = transform.GetChild(1).GetComponent<Image>();

        //Record unlock colour in case it's unlocked later on this screen
        unlockedColour = image.color;
        RecolourIfUnlocked();
        

        initialized = true;
    }

    //Recolour but not if initialized already
    private void OnEnable()
    {
        if (initialized)
        RecolourIfUnlocked();
    }

    //Grey stuff out if it isn't unlocked yet
    public void RecolourIfUnlocked()
    {
        if (SkinIsUnlocked())
        {
            button.onClick.AddListener(ChangeToDifferentSkin);
            image.color = unlockedColour;
        }
        else { image.color = Color.grey; childImage.color = Color.grey; }
    }

    //Check if skin is available
    bool SkinIsUnlocked()
    {
        //Debug.Log((int)themeToActivate + " index");

        if (isLevelTheme)
        {
            if (themeToActivate == Skins.Themes.RANDOM) return true;
            if (Skins.unlockedLevelThemes[(int)themeToActivate]) return true;
            else return false;
        }
        else
        {
            if (skinToActivate == Skins.SnakeSkins.RANDOM) return true;
            if (Skins.unlockedSnakeSkins[(int)skinToActivate]) return true;
            else return false;
        }
    }

    //Called whenever the button is pressed. The button can't be pressed if it's locked.
    void ChangeToDifferentSkin()
    {
        if (isLevelTheme)
        {
            Skins.levelTheme = themeToActivate;
            if (themeToActivate != Skins.Themes.RANDOM) Skins.randomTheme = false;
        }
        else
        {
            Skins.snakeSkin = skinToActivate;
            if (skinToActivate != Skins.SnakeSkins.RANDOM) Skins.randomSkin = false;

            //Change the player skin if not randomizing
            GameObject.Find("Player").GetComponent<Snake>().ChangeSnakeSkin();
        }

        //If random was just picked, then turn random on.
        Skins.CheckForRandomization();


        //Save new skin
        GlobalStats.Save();

        //Force the theme to change
        if (isLevelTheme) SceneManager.LoadScene(0);
        else
        {
            //Force out of skin menu
            GlobalStats.hud.ChangeMenuMode(1);
        }
    }
}
