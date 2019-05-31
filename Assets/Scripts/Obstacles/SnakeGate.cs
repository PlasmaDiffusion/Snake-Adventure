using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGate : MonoBehaviour
{
    int requirement;
    bool open;
    bool entered;

    TextMesh requirementText;
    TextMesh requirementText2;

    public int minFoodRequired = 5;
    public int maxFoodRequired = 10;

    // Start is called before the first frame update
    void Start()
    {
        //Determine how much food is required to open this door. Also count the food in the level to ensure it's possible.
        int amountOfPossibleFood = CountSnakeFood();

        //Debug.Log("Possible food: " + amountOfPossibleFood.ToString());
                                                                          //If amount possible < max, dont exceed amount possible
        requirement = Random.Range(minFoodRequired, (amountOfPossibleFood < maxFoodRequired) ? amountOfPossibleFood : maxFoodRequired);
        open = false;
        entered = false;

        requirementText = transform.GetChild(0).GetComponent<TextMesh>();
        requirementText2 = transform.GetChild(1).GetComponent<TextMesh>();

        //Update gate text
        requirementText.text = requirement.ToString();
        requirementText2.text = requirement.ToString();

        GlobalStats.requiredFood = requirement;
        if (GlobalStats.hud)GlobalStats.hud.UpdateHUD();
    }

    //Called whenever snake food is destroyed
    public void LowerGate()
    {
        requirement--;
        GlobalStats.requiredFood = requirement;
        if (requirement < 0) GlobalStats.requiredFood = 0;

        //Update gate text
        requirementText.text = requirement.ToString();
        requirementText2.text = requirement.ToString();

        if (requirement == 0)
        {
            //Now the door shall open!
            GetComponent<BoxCollider>().isTrigger = true;
            open = true;

            GetComponent<Renderer>().material.color = new Color(0.2f, 0.0f, 0.6f, 0.5f);



        }

        GlobalStats.hud.UpdateHUD();

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
        if (other.name == "Player" && open && !entered)
        {
            if (transform.parent.name != "StartingLevel")
            {
                GlobalStats.score += 10;
            }


            //Ranzomide level theme if that is set
            Skins.CheckForRandomization();

            //Spawn in more level here. Despawn the oldest level, but not the one being exited.
            entered = true;
            LevelSpawner spawner = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>();
            spawner.EndLevel(transform.position);


        }
        else if (other.tag == "Wall") //Alternatively if a level is spawned in, despawn any walls that would  be in the way of the door.
        {
            Destroy(other.gameObject);
        }

        Debug.Log(other.tag);

    }
    
    int CountSnakeFood() {return transform.parent.GetComponentsInChildren<SnakeFood>().Length;}
}
