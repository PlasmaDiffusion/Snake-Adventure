using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    public Button boostButton;
    public Image boostGuageImage;

    //Reference to boost time
    public GameObject player;
    Snake snake;

    float t;
    float currentGuageFill;

    // Start is called before the first frame update
    void Start()
    {
        snake = player.GetComponent<Snake>();
        t = 0.0f;
        currentGuageFill = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float targetGuageAmount = snake.GetBoostGuage();
        float currentGuageAmount = boostGuageImage.fillAmount;

        //Lerp to boost amount
        if (currentGuageAmount < targetGuageAmount || Snake.boosting)
        {
            t += Time.deltaTime;
            boostGuageImage.fillAmount = Mathf.Lerp(currentGuageAmount, targetGuageAmount, t);
        }
        else //If target fill is reached then do nothing but reset t to 0.
        {
            t = 0.0f;
        }

        //Button is only available when guage is full.
        boostButton.gameObject.SetActive(targetGuageAmount >= 1.0f && !Snake.boosting);
    }
}
