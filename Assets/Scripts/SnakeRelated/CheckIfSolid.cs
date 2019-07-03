using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks if the tile in front is solid or not
public class CheckIfSolid : MonoBehaviour
{
    bool isSolid;
    [HideInInspector]
    public GameObject owner;

    public Vector3 offset;

    Snake snake;

    //When list contains something, solid flag will be true.
    int overlappingSolidColliders;


    // Start is called before the first frame update
    void Start()
    {
        isSolid = false;
        snake = GameObject.Find("Player").GetComponent<Snake>();
        //overlappingSolidColliders = new List<Collider>();
    }

    //Move relative to the player, to the right, left, in front or behind them.
    void Update() //Maybe try snapping these collision boxes to the grid constantly?
    {
        if (owner)
        {
            transform.position = owner.transform.position + offset;
            transform.position = snake.AlignToGrid(transform.position);
        }

        //When there are no solid objects, solid flag will be false.
        if (overlappingSolidColliders == 0) isSolid = false;
    }

    public bool GetIsSolid() { return isSolid; }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Burnable") 
        {
            isSolid = true;
            overlappingSolidColliders++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Burnable")
        {
            overlappingSolidColliders--;
        }
    }
}
