using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpMaterial : MonoBehaviour
{
    Renderer rend;

    public float multiplier;


    // Light up the material
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        float rModifier = 0.1f;
        float gModifier = 0.1f;
        float bModifier = 0.1f;

        CheckForSpecificThemeModifiers(ref rModifier, ref gModifier, ref bModifier);

        rend.material.color += (new Color(rModifier, gModifier, bModifier) * multiplier);
    }

    //Use this function to hardcode how much the material changes depending on what the theme is.
    void CheckForSpecificThemeModifiers(ref float rModifier, ref float gModifier, ref float bModifier)
    {

        switch (Skins.levelTheme)
        {
            case Skins.Themes.DEFAULT:
                bModifier = 0.2f;
                break;

            case Skins.Themes.SNOW:
                multiplier *= -2.0f;
                break;
        }
    }
}
