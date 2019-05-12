using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGate : MonoBehaviour
{
    int requirement;
    bool open;

    TextMesh requirementText;
    TextMesh requirementText2;

    // Start is called before the first frame update
    void Start()
    {
        requirement = 10;
        open = false;

        requirementText = transform.GetChild(0).GetComponent<TextMesh>();
        requirementText2 = transform.GetChild(1).GetComponent<TextMesh>();
    }

    //Called whenever snake food is destroyed
    public void LowerGate()
    {
        requirement--;

        //Update gate text
        requirementText.text = requirement.ToString();
        requirementText2.text = requirement.ToString();

        if (requirement == 0)
        {
            //Now the door shall open!
            GetComponent<BoxCollider>().isTrigger = true;
        }

        //Hide the 3d text when it's open.
        if (requirement < 1)
        {
            requirementText.text = "";
            requirementText2.text = "";

        }
    }

    //Player is about to finish this "level", and the game will generate another one.
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && open)
        {
            //Spawn in more level here. Despawn the older level.
        }
    }
}
