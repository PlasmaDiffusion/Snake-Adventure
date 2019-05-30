using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSkin : MonoBehaviour
{
    Skins skinObject;

    Button button;
    Image image;
    Image childImage;

    public Skins.Themes themeToActivate;
    public Skins.SnakeSkins skinToActivate;

    public bool isLevelTheme;

    // Start is called before the first frame update
    void Start()
    {

        skinObject = GameObject.Find("SkinHandler").GetComponent<Skins>();

        button = GetComponent<Button>();
        image = GetComponent<Image>();
        childImage = transform.GetChild(1).GetComponent<Image>();

        RecolourIfUnlocked();
    }

    //Grey stuff out if it isn't unlocked yet
    public void RecolourIfUnlocked()
    {
        if (SkinIsUnlocked()) button.onClick.AddListener(ChangeToDifferentSkin);
        else { image.color = Color.grey; childImage.color = Color.grey; }
    }

    //Check if skin is available
    bool SkinIsUnlocked()
    {
        Debug.Log((int)themeToActivate + " index");

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

    void ChangeToDifferentSkin()
    {
        if (isLevelTheme)
            Skins.levelTheme = themeToActivate;
        else
            Skins.snakeSkin = skinToActivate;
    }
}
