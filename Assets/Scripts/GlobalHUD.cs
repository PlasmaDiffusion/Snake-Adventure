using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalHUD : MonoBehaviour
{
    public GameObject scoreObject;
    public GameObject requiredFoodObject;
    public GameObject coinObject;

    Text scoreText;
    Text requiredFoodText;
    Text coinText;

    Vector3 defaultCoinPosition;

    public float coinsVisibleTime; //If 10.0f, coins will be constantly visible

    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreObject.GetComponent<Text>();
        requiredFoodText = requiredFoodObject.GetComponent<Text>();
        coinText = coinObject.GetComponent<Text>();

        defaultCoinPosition = coinText.rectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Hide coins after a second
        if (coinsVisibleTime > 0 || coinsVisibleTime == 10.0f) coinsVisibleTime -= Time.deltaTime;
        else coinText.rectTransform.Translate(0.0f, 1.0f * Time.deltaTime, 0.0f);
    }

    public void UpdateHUD()
    {
        scoreText.text = GlobalStats.score.ToString();
        requiredFoodText.text = GlobalStats.requiredFood.ToString();
        coinText.text = GlobalStats.coins.ToString();
    }

    public void DisplayCoins(bool forever)
    {
        if (forever) coinsVisibleTime = 10.0f;
        else coinsVisibleTime = 2.0f;

        coinText.rectTransform.position = defaultCoinPosition;
    }
}
