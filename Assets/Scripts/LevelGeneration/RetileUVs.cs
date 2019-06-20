using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetileUVs : MonoBehaviour
{
    public Vector2 newUVScale;
    

    // Start is called before the first frame update
    void Start()
    {
        if (Skins.levelTheme != Skins.Themes.SNOW) //Dont change themes that AREN'T tiled.
        GetComponent<Renderer>().material.mainTextureScale = newUVScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
