using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Depending on the flag, change the mesh or material to match the given theme. Add skins in the SkinManager game object.
public class ChangeByTheme : MonoBehaviour
{
    public bool themeAffectsMaterial;
    //If material changes, is it changing based on the floor material? If not then it's a wall material.
    public bool changesFloorMaterial;
    
    public bool themeAffectsMesh;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;

        UpdateTheme();
    }
    
    public void UpdateTheme()
    {
        if (themeAffectsMaterial)
        {
            if (changesFloorMaterial)
                mat = Skins.levelThemeFloorMaterials[(int)Skins.levelTheme];
            else
                mat = Skins.levelThemeWallMaterials[(int)Skins.levelTheme];
        }


        if(themeAffectsMesh)
        {
            GameObject changedObj = Instantiate(Skins.levelThemeObjects[(int)Skins.levelTheme], transform.parent);

            //Update transform
            changedObj.transform.position = transform.position;
            changedObj.transform.rotation = transform.rotation;
            changedObj.transform.localScale = transform.localScale;
        }
    }
}
