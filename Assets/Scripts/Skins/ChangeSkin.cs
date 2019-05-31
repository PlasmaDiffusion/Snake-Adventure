using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    //Grey stuff out if it isn't unlocked yet
    public void RecolourIfUnlocked()
    {
        if (SkinIsUnlocked())
        {
            button.onClick.AddListener(ChangeToDifferentSkin);
            image.color = unlockedColour; childImage.color = Color.white;
        }
        else { image.color = Color.grey; childImage.color = Color.grey; }
    }

    //Check if skin is available
    bool SkinIsUnlocked()
    {
        //Debug.Log((int)themeToActivate + " index");

        if (isLevelTheme)
        {
            if (Skins.unlockedLevelThemes[(int)themeToActivate]) return true;
            else return false;
        }
        else
        {
            if (Skins.unlockedSnakeSkins[(int)skinToActivate]) return true;
            else return false;
        }
    }

    //Called whenever the button is pressed. The button can't be pressed if it's locked.
    void ChangeToDifferentSkin()
    {
        if (isLevelTheme)
            Skins.levelTheme = themeToActivate;
        else
            Skins.snakeSkin = skinToActivate;

        //If this isn't button for random, turn it off.
        if (themeToActivate != Skins.Themes.RANDOM || skinToActivate != Skins.SnakeSkins.RANDOM) Skins.CheckToTurnOffRandom();

        //If random was just picked, then turn random on.
        Skins.CheckForRandomization();
    }
}
