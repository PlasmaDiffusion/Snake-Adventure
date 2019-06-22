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

        if (Skins.levelTheme != Skins.Themes.SNOW) //Dont change themes that AREN'T tiled.
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.mainTextureScale = newUVScale;
            rend.material.mainTextureOffset = uvOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
