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

    //Colour the bar!
    Color defaultBarColour;
    Color glowyBarColour;
    float colourT;
    float colourT_Delta;

    // Start is called before the first frame update
    void Start()
    {
        snake = player.GetComponent<Snake>();
        t = 0.0f;
        currentGuageFill = 0.0f;
        defaultBarColour = boostButton.image.color;
        glowyBarColour = Color.cyan;

        colourT = 0.0f;
        colourT_Delta = 1.2f;
    }

    private void OnEnable()
    {
        //Move boost ui to the right if arrow buttons exist
        //if (!GlobalStats.swipeControls)
        //{
        //    RectTransform rectTransform = GetComponent<RectTransform>();
        //    rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
        //    rectTransform.anchorMax = new Vector2(1.0f, 0.0f);
        //    rectTransform.pivot = new Vector2(1.0f, 0.0f);
        //   transform.position = new Vector2(transform.position.x, 0.0f);

        //    RectTransform boostRect = boostButton.GetComponent<RectTransform>();
        //    boostRect.anchorMin = new Vector3(1.0f, 0.5f);
        //    boostRect.anchorMax = new Vector3(1.0f, 0.5f);
        //    boostRect.pivot = new Vector3(1.0f, 0.5f);
        //}
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

        if (targetGuageAmount >= 1.0f)
        {
            boostButton.image.color = Color.Lerp(defaultBarColour, glowyBarColour, colourT);
            colourT += Time.deltaTime * colourT_Delta;

            if (colourT > 1.0f) colourT_Delta = -1.2f;
            else if (colourT < 0.0f) colourT_Delta = 1.0f;
        }
        else
            boostButton.image.color = defaultBarColour;
    }
}
