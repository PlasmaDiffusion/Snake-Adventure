using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Show feedback for collecting new coins at the end of the game
public class CoinTally : MonoBehaviour
{
    int prevCoinCount;
    int coinBonus;

    public Text totalCoinsText;
    public Text bonusCoinsText;
    public GameObject gameOverObject;
    public GameObject continueButton;

    bool alreadyTallying;

    // Start is called before the first frame update
    void Start()
    {
        //Get this before global stats tallys coins
        coinBonus = GlobalStats.score / 50;
        prevCoinCount = GlobalStats.coins - coinBonus;

        alreadyTallying = false;

        UpdateText();
    }

    //Called when finally ending the game
   public void StartTallying()
    {
        if (!alreadyTallying)
        {

        continueButton.transform.GetChild(0).GetComponent<Text>().text = "Skip";
        alreadyTallying = true;


        StartCoroutine(Countdown());

        }
        else
        {
         //Force skip
         gameOverObject.GetComponent<DeathHandler>().RestartScene();
        }
    }

    private IEnumerator Countdown()
    {


        //Keep tallying counts every second until done.
        while (coinBonus > 0 && prevCoinCount < GlobalStats.coins)
        {
        SoundManager.PlaySound(SoundManager.Sounds.COIN);

        prevCoinCount++;
        coinBonus--;
        UpdateText();
        yield return new WaitForSeconds(0.1f);
        }
        gameOverObject.GetComponent<DeathHandler>().RestartScene();

        yield break;
    }

    void UpdateText()
    {
        totalCoinsText.text = prevCoinCount.ToString();
        bonusCoinsText.text = coinBonus.ToString();
    }
    
}
