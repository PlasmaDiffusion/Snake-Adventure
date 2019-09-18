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

    GameObject arrowToShow;

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

        if (transform.childCount > 7)
        { 
        arrowToShow = transform.GetChild(7).gameObject;
        arrowToShow.SetActive(false);
        }
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
            if (transform.childCount > 7)arrowToShow.SetActive(true);

            GetComponent<Renderer>().material.color = new Color(0.2f, 0.0f, 0.6f, 0.5f);

            //Make the collision possible and not solid now.
            tag = "Untagged";
            Destroy(transform.GetChild(6).gameObject);

            //Play open sound!
            SoundManager.PlaySound(SoundManager.Sounds.OPEN_GATE, 1.0f, -1);
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
                //Add score depending on what the theme difficulty is
                if (Skins.levelTheme == Skins.Themes.RUINS) GlobalStats.AddScore(150, other.transform.position);
                else if (Skins.levelTheme == Skins.Themes.FACTORY) GlobalStats.AddScore(250, other.transform.position);
                else if (Skins.levelTheme == Skins.Themes.DUNGEON) GlobalStats.AddScore(150, other.transform.position);
                else GlobalStats.AddScore(100, other.transform.position);

                CoinObjective.CheckForObjective((int)CoinObjective.Objective.BEAT_THEMED_LEVELS, (int)Skins.levelTheme);
            }

            //Remove the arrow cube reference if it exists
            GameObject arrowObject = GameObject.Find("CubeArrow");
            if (arrowObject) arrowObject.GetComponent<LookAtCube>().RemoveRefernce();

            SoundManager.PlaySound(SoundManager.Sounds.ENTER_GATE);


            //Remove any obstacles so they stop with the damn spike trap sounds :P
            GameObject[] hazardsToKill = GameObject.FindGameObjectsWithTag("BurnableNotSolid");
            for (int i = hazardsToKill.Length - 1; i >= 0; i--)
            {
                Destroy(hazardsToKill[i]);
            }

            //Ranzomide level theme if that is set
            Skins.CheckForRandomization(true);

            Snake snake = other.GetComponent<Snake>();
            snake.IncreaseSpeed();
            snake.lastSpawnPosition = transform.position;

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
