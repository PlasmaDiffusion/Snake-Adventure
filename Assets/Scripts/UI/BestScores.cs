using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BestScores : MonoBehaviour
{
    public bool bestScore;
    public bool bestLevel;

    Text numberText;

    // Start is called before the first frame update
    void Start()
    {
        numberText = GetComponent<Text>();

        if (bestLevel)
        {
            numberText.text = GlobalStats.GetFarthestLevel().ToString();
        }
        else if (bestScore)
        {
            numberText.text = GlobalStats.GetHiScore().ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
