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

    // Start is called before the first frame update
    void Start()
    {
        isSolid = false;
    }

    //Move relative to the player, to the right, left, in front or behind them.
    void Update() //Maybe try snapping these collision boxes to the grid constantly?
    {
        if(owner)transform.position = owner.transform.position + offset;
    }

    public bool GetIsSolid() { return isSolid; }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Burnable")
        {
            Debug.Log("SOLID ON: " + name);
            isSolid = true;
            //owner.transform.position = owner.GetComponent<SnakeMovement>().AlignToGrid(owner.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Burnable")
        {
            Debug.Log("SOLID OFF: " + name);
            isSolid = false;
        }
    }
}
