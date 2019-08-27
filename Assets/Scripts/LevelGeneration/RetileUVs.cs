using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetileUVs : MonoBehaviour
{
    Vector2 newUVScale;
    public Vector2 uvOffset;

    // Start is called before the first frame update
    void Start()
    {
        newUVScale = new Vector2(transform.localScale.x, transform.localScale.z);

        SkrinkUVScale();


        if (Skins.levelTheme != Skins.Themes.SNOW) //Dont change themes that AREN'T tiled.
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.mainTextureScale = newUVScale;
            rend.material.mainTextureOffset = uvOffset;
        }

    }


    void SkrinkUVScale()
    {
        if (Skins.levelTheme == Skins.Themes.DESERT) newUVScale *= 0.5f;
        else if (Skins.levelTheme == Skins.Themes.CITY) newUVScale *= 0.5f;
        else if (Skins.levelTheme == Skins.Themes.DUNGEON) newUVScale *= 0.5f;
        else if (Skins.levelTheme == Skins.Themes.CAVE) newUVScale *= 0.5f;
        else if (Skins.levelTheme == Skins.Themes.TOYBOX) newUVScale *= 0.25f;
    }
}
