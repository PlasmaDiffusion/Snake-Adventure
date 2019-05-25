using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalHUD : MonoBehaviour
{
    //Text objects
    public GameObject scoreObject;
    public GameObject requiredFoodObject;
    public GameObject coinObject;

    //Halfcircle powerup objects
    public GameObject bonusMultiplierObject;
    public GameObject fireObject;
    public GameObject ghostObject;

    Text scoreText;
    Text requiredFoodText;
    Text coinText;

    Image bonusMultiplierTime;
    Image fireTime;
    Image ghostTime;
    
    Vector3 defaultCoinPosition;

    public float coinsVisibleTime; //If 10.0f, coins will be constantly visible

    //Gets values for ghost powerup
    DeathCheck playerDeathCheck;

    //Get values for fire powerup
    FireBreathe fireBreathe; 

    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreObject.GetComponent<Text>();
        requiredFoodText = requiredFoodObject.GetComponent<Text>();
        coinText = coinObject.GetComponent<Text>();

        bonusMultiplierTime = bonusMultiplierObject.GetComponent<Image>();
        ghostTime = ghostObject.GetComponent<Image>();
        fireTime = fireObject.GetComponent<Image>();

        defaultCoinPosition = coinText.rectTransform.position;

        playerDeathCheck = GameObject.Find("Player").GetComponent<DeathCheck>();
        fireBreathe = GameObject.Find("Tongue").GetComponent<FireBreathe>();
    }

    // Update is called once per frame
    void Update()
    {
        //Hide coins after a second
        if (coinsVisibleTime > 0 || coinsVisibleTime == 10.0f) coinsVisibleTime -= Time.deltaTime;
        else coinText.rectTransform.Translate(0.0f, 8.0f * Time.deltaTime, 0.0f);

        float scoreTime = SnakeFood.GetMultiplierTime();

        //Update score multiplier circle
        if (scoreTime > 0.0f)
        {
            bonusMultiplierTime.fillAmount = scoreTime / 8.0f;
            bonusMultiplierTime.gameObject.SetActive(true);
        }
        else
        {
            bonusMultiplierTime.gameObject.SetActive(false);
        }

        //Update ghost circle
        if (playerDeathCheck.invincibility > 0.0f)
        {
            ghostTime.fillAmount = playerDeathCheck.invincibility / playerDeathCheck.maxInvincibility;
            ghostTime.gameObject.SetActive(true);
        }
        else
        {
            ghostTime.gameObject.SetActive(false);
        }

        //Update fire circle
        if (fireBreathe.activeTime > 0.0f)
        {
            fireTime.fillAmount = fireBreathe.activeTime / fireBreathe.maxActiveTime;
            fireTime.gameObject.SetActive(true);
        }
        else
        {
            fireTime.gameObject.SetActive(false);
        }
    }

    public void UpdateHUD()
    {
        if (scoreText && requiredFoodText && coinText) //Make sure they all exist
        { 
        scoreText.text = GlobalStats.score.ToString();
        requiredFoodText.text = GlobalStats.requiredFood.ToString();
        coinText.text = GlobalStats.coins.ToString();
        }
    }

    public void DisplayCoins(bool forever)
    {
        if (forever) coinsVisibleTime = 10.0f;
        else coinsVisibleTime = 2.0f;

        coinText.rectTransform.position = defaultCoinPosition;
    }
}
