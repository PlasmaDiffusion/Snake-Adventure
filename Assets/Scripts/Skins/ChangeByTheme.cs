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
        
        ChangeTheme();
    }

    public void ChangeTheme()
    {

        if (themeAffectsMaterial || changesFloorMaterial)
        {

            //Random colours for specific themes go here
            if (Skins.levelTheme == Skins.Themes.TOYBOX && !changesFloorMaterial) PickRandomColour();
            else
            { 

            if (changesFloorMaterial)
                rend.material = skinChangerObject.levelThemeFloorMaterials[(int)Skins.levelTheme];
            else
                rend.material = skinChangerObject.levelThemeWallMaterials[(int)Skins.levelTheme];
            }
        }

        if(themeAffectsMesh && Skins.levelTheme != Skins.Themes.DEFAULT)
        {

            //Debug.Log((int)Skins.levelTheme);

            GameObject changedObj = Instantiate(skinChangerObject.levelThemeObjects[(int)Skins.levelTheme], transform.parent);
            
            //Update transform
            changedObj.transform.position = transform.position;
            if(Skins.levelTheme != Skins.Themes.SNOW) changedObj.transform.rotation = transform.rotation;
            //changedObj.transform.localScale = transform.localScale;

            //Destroy self
            Destroy(gameObject);
        }
    }

    private void PickRandomColour()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                rend.material.color = Color.red;
                break;
            case 1:
                rend.material.color = Color.green;
                break;
            case 2:
                rend.material.color = Color.blue;
                break;
            case 3:
                rend.material.color = Color.yellow;
                break;
        }
        rend.material.color *= 0.5f;
    }
}
