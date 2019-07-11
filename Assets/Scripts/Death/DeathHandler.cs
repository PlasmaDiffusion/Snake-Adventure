using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

//Class that is basically the game over screen.
public class DeathHandler : MonoBehaviour
{
    bool watchedAd;
    bool payedCoins;

    public GameObject player;
    public GameObject adButton;
    public GameObject coinButton;
    public GameObject continueText;
    public GameObject endGameScreen;
    
    Text coinButtonText;

    int coinCostMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        coinCostMultiplier = 1;
        watchedAd = false;
        payedCoins = false;

        coinButtonText = coinButton.transform.GetChild(0).GetComponent<Text>();

        if (GlobalStats.disabledAds)
        {
            adButton.transform.GetChild(0).GetComponent<Text>().text = "Free Revive";
        }
    }
    
    //Call Die() to prompt a game over menu. 
    void OnEnable()
    {
        gameObject.SetActive(true);


        GlobalStats.hud.DisplayCoins(true);

        //Give player a button to watch an ad to continue here. (Life 2)
        if (watchedAd)
        {
            adButton.SetActive(false);
        }

        //Also let them pay 10 coins. (Life 3)
        if(payedCoins)
        {
            coinButton.SetActive(false);
        }

        if (watchedAd && payedCoins)
        {
            continueText.SetActive(false);
        }
    }
     //Continue game and exit menu.
    void Revive()
    {
        //Hide the coins gradually.
        GlobalStats.hud.DisplayCoins(false);
        GlobalStats.hud.UpdateHUD();

        gameObject.SetActive(false);
        player.GetComponent<DeathCheck>().Respawn();
    }

    //End the game here along with highscores, rewards, etc.
    public void EndGame()
    {
        //Grant bonus coins
        int coinBonus = GlobalStats.score / 50;
        
        endGameScreen.SetActive(true);
        
        //Coins are to be "tallied" so increase them afterwards.
        GlobalStats.coins += coinBonus;


        //Check for best scores here. Show feedback if the player improved!
        if (GlobalStats.CheckForHiScore())
        {
            GameObject textObj = endGameScreen.transform.Find("NewBestScoreText").gameObject;
            textObj.GetComponent<Text>().text = "New Best!";
        }
        if (GlobalStats.CheckForFarthestLevel())
        {
            GameObject textObj = endGameScreen.transform.Find("NewBestLevelText").gameObject;
            textObj.GetComponent<Text>().text = "New Best!";

        }
    }

    //When Continue button is pressed
    public void RestartScene()
    {
        //Save the game whenver the game ends
        GlobalStats.Save();

        SceneManager.LoadScene(0);
    }

    public void ShowRewardedAd()
    {
        //If the player supported the game, they skip the ad
        if (GlobalStats.disabledAds)
        {
            watchedAd = true;
            Revive();
        }
        else if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    //Show a single ad to revive the player
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                watchedAd = true;
                Revive();

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                watchedAd = true; //No more chances if they skip :P

                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void PayCoins()
    {
        int cost = 5 * coinCostMultiplier;

        //Pay up and increase the cost next time.
        if (GlobalStats.coins >= cost)
        {

            GlobalStats.coins -= cost;
            coinCostMultiplier++;


            //Cap this off at 3 revives? We don't want unlimited lives after all..
            if ((cost >= 15)) payedCoins = true;

            //Update the button
            cost = 5 * coinCostMultiplier;
            coinButtonText.text = "Pay " + cost.ToString() + " Coins";
            
            //Actually revive the player too.
            Revive();
        }

    }
}
