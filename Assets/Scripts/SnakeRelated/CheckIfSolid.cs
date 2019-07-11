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
    //List of objects that might die the frame of
    List<GameObject> burnableCollidedObjects;

    // Start is called before the first frame update
    void Start()
    {
        isSolid = false;
        snake = GameObject.Find("Player").GetComponent<Snake>();
        burnableCollidedObjects = new List<GameObject>();
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
        else //Check if a burnable object was destroyed while this object was still collided with it.
        {
            for (int i = burnableCollidedObjects.Count-1; i >= 0; i--)
            {
                //If it was then remove it
                if (!burnableCollidedObjects[i])
                {
                    burnableCollidedObjects.RemoveAt(i);
                    overlappingSolidColliders--;
                }
            }
        }
    }

    public bool GetIsSolid() { return isSolid; }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Burnable") 
        {
            isSolid = true;
            overlappingSolidColliders++;
            if (other.tag == "Burnable") burnableCollidedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Burnable")
        {
            overlappingSolidColliders--;
            if (other.tag == "Burnable") burnableCollidedObjects.Remove(other.gameObject);
        }
    }
}
