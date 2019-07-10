using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Put this on any part of the UI to disable it until the player has reached a certain level.
public class HideIfNewGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalStats.GetFarthestLevel() < 2) gameObject.SetActive(false);
    }
}
