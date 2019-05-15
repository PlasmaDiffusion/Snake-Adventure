using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : MonoBehaviour
{
    public bool solid = false;

    BoxCollider boxCollider;
    Material material;

    Color offColour;
    Color onColour;

    // Start is called before the first frame update
    void Start()
    {

        boxCollider = GetComponent<BoxCollider>();


        material = GetComponent<Renderer>().material;
        offColour = material.color;
        onColour = offColour + new Color(0.0f, 0.0f, 0.0f, 1.0f);


        if (solid) MakeSolid();
        else MakeTrigger();
    }


    public void SwitchToggle()
    {
        solid = !solid;

        if (solid) MakeSolid();
        else if (!solid) MakeTrigger();
        
    }

    void MakeSolid()
    {
        boxCollider.isTrigger = false;
        material.color = onColour;
    }

    void MakeTrigger()
    {
        boxCollider.isTrigger = true;
        material.color = offColour;
    }
}
