using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    float yLimit;
    bool objReference;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        //Convert position to screen space

        if (name == "ScoreBonusText") { objReference = true; }
        else
        {
            objReference = false;
        }

        text = GetComponent<Text>();
        yLimit = text.rectTransform.position.y + 4.0f;


    }

    // Update is called once per frame
    void Update()
    {
        if (objReference) return;

        text.rectTransform.position += new Vector3(0.0f, 4.0f*Time.deltaTime, 0.0f);
        
        if (text.rectTransform.position.y > yLimit) Destroy(gameObject);
    }
}
