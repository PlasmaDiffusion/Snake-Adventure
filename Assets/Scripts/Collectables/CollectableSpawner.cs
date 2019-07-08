using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Randomly spawns in the given checked collectable
public class CollectableSpawner : MonoBehaviour
{
    public bool[] possiblePowerup;
    public GameObject[] collectables;

    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        //Hide yourself!
        rend = GetComponent<Renderer>();
        rend.enabled = false;

        List<GameObject> canSpawn = new List<GameObject>();

        //Make a list of all possible powerups
        for (int i = 0; i < possiblePowerup.Length; i++)
            if (possiblePowerup[i]) canSpawn.Add(collectables[i]);

        //Spawn a random object from that list.
        GameObject newCollectable = Instantiate(canSpawn[Random.Range(0, canSpawn.Count)], transform.parent);
        newCollectable.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
