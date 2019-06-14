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
    

    // Start is called before the first frame update
    void Start()
    {
        //Get this before global stats tallys coins
        coinBonus = GlobalStats.score / 5;
        prevCoinCount = GlobalStats.coins - coinBonus;

        UpdateText();
    }

    //Called when finally ending the game
   public void StartTallying()
    {
        continueButton.SetActive(false); //Prevent repeats

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {


        //Keep tallying counts every second until done.
        while (coinBonus > 0)
        {
            //Insert some coin sound here!!!!

        prevCoinCount++;
        coinBonus--;
        UpdateText();
        yield return new WaitForSeconds(0.1f);
        }


        gameOverObject.GetComponent<DeathHandler>().RestartScene();
         

    }

    void UpdateText()
    {
        totalCoinsText.text = prevCoinCount.ToString();
        bonusCoinsText.text = coinBonus.ToString();
    }
    
}
