using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns in a food mesh and applies a given bonus, set in the prefabs
public class BonusFood : MonoBehaviour
{
    [Header("Set Meshes here (in order of themes)")]
    public GameObject[] otherFoodBonuses;
    [Header("Set Point Values here")]
    public int[] scoreBonuses;

    //Instantiate an empty child food object
    void Start()
    {
        //Make invisible in game but visible in the editor.
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(otherFoodBonuses[(int)Skins.levelTheme], transform);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0.0f, 15.0f * Time.deltaTime, 0.0f));
    }

    //Player collects once
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GlobalStats.AddScore(scoreBonuses[(int)Skins.levelTheme], transform.position);
            Destroy(gameObject);
        }
    }
}
