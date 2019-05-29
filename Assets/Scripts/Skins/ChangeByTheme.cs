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


    Renderer rend;

    //Skin object
    Skins skinChangerObject;

    // Start is called before the first frame update
    void Start()
    {
        skinChangerObject = GameObject.Find("SkinHandler").GetComponent<Skins>();

        rend = GetComponent<Renderer>();

        UpdateTheme();
    }
    
    public void UpdateTheme()
    {
        if (themeAffectsMaterial || changesFloorMaterial)
        {
            if (changesFloorMaterial)
                rend.material = skinChangerObject.levelThemeFloorMaterials[(int)Skins.levelTheme];
            else
                rend.material = skinChangerObject.levelThemeWallMaterials[(int)Skins.levelTheme];
        }


        if(themeAffectsMesh)
        {
            GameObject changedObj = Instantiate(skinChangerObject.levelThemeObjects[(int)Skins.levelTheme], transform.parent);

            //Update transform
            changedObj.transform.position = transform.position;
            changedObj.transform.rotation = transform.rotation;
            //changedObj.transform.localScale = transform.localScale;

            //Destroy self
            Destroy(gameObject);
        }
    }
}
