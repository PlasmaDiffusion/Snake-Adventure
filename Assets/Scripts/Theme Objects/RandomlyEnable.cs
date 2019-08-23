using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyEnable : MonoBehaviour
{
    
    public int chance;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

     if (Random.Range(0, chance) == chance-1)
        {
            gameObject.SetActive(true);
        }
    }
}
