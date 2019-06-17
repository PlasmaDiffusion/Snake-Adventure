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

        float alphaModifier = 1.0f;

        if (Skins.levelTheme == Skins.Themes.SNOW) { multiplier *= 0.5f; alphaModifier = 0.1f; }

        rend.material.color += (new Color(0.1f, 0.1f, 0.1f, alphaModifier) * multiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
