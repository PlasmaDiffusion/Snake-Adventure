using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

//Class that is basically the game over screen.
public class DeathHandler : MonoBehaviour
{
    int watchedAds;
    bool payedCoins;

    public GameObject player;
    public GameObject adButton;
    public GameObject coinButton;
    public GameObject continueTextObject;
    public GameObject endGameScreen;

    Text continueText;
    Text coinButtonText;

    int coinCostMultiplier;

    int continues;

    // Start is called before the first frame update
    void Start()
    {
        coinCostMultiplier = 1;
        watchedAds = 0;
        continues = 3;
        payedCoins = false;

        coinButtonText = coinButton.transform.GetChild(0).GetComponent<Text>();
        continueText = continueTextObject.GetComponent<Text>();

        if (GlobalStats.disabledAds)
        {
            adButton.transform.GetChild(0).GetComponent<Text>().text = "Free Revive";
        }

        #if UNITY_STANDALONE_WIN
              adButton.gameObject.SetActive(false);
        #endif

    }

    //Call Die() to prompt a game over menu. 
    void OnEnable()
    {
        gameObject.SetActive(true);

        if (GlobalStats.hud)
        GlobalStats.hud.DisplayCoins(true);

        if (!continueText) { continueText = continueTextObject.GetComponent<Text>(); continues = 3; }

        continueText.text = "Watch an ad or pay coins to continue!\n" +
            "Revives Remaining: " + continues.ToString();

        #if UNITY_STANDALONE_WIN
              continueText.text = "Pay coins to continue!\n" +
            "Revives Remaining: " + continues.ToString();
        #endif

        //Give player a button to watch an ad to continue here.
        if (watchedAds > 2 || continues == 0)
        {
            adButton.SetActive(false);
        }

        //Also let them pay coins.
        if(payedCoins || continues == 0)
        {
            coinButton.SetActive(false);
        }

        if (continues == 0)
        {
            continueTextObject.SetActive(false);
        }
    }
     //Continue game and exit menu.
    void Revive()
    {

        continues--;

        //Hide the coins gradually.
        GlobalStats.hud.DisplayCoins(false);
        GlobalStats.hud.UpdateHUD();

        gameObject.SetActive(false);
        DeathCheck deathCheck = player.GetComponent<DeathCheck>();
        deathCheck.AddLife();
        deathCheck.Respawn();
    }

    //End the game here along with highscores, rewards, etc.
    public void EndGame()
    {
        //Hide warning signs so they don't block the screen
        SpikeTrap.hideAllWarningSigns = true;

        //Grant bonus coins
        int coinBonus = GlobalStats.score / 50;
        
        endGameScreen.SetActive(true);
        
        //Coins are to be "tallied" so increase them afterwards.
        GlobalStats.coins += coinBonus;


        //Check for best scores here. Show feedback if the player improved!
        if (GlobalStats.CheckForHiScore())
        {
            GameObject textObj = endGameScreen.transform.Find("NewBestScoreText").gameObject;
            textObj.GetComponent<Text>().text = "New Best! ";
            SoundManager.PlaySound(SoundManager.Sounds.HISCORE);
        }
        if (GlobalStats.CheckForFarthestLevel())
        {
            GameObject textObj = endGameScreen.transform.Find("NewBestLevelText").gameObject;
            textObj.GetComponent<Text>().text = "New Best! ";
            SoundManager.PlaySound(SoundManager.Sounds.HISCORE);

        }
    }

    //When Continue button is pressed
    public void RestartScene()
    {
        //Save the game whenver the game ends
        GlobalStats.Save();
        EndOfGameAdd.gamesPlayed++;
        EndOfGameAdd ad = GetComponent<EndOfGameAdd>();

        if (ad.TimeToShowAd()) ad.ShowAd();
        else SceneManager.LoadScene(0);
    }


#if !UNITY_STANDALONE_WIN


    public void ShowRewardedAd()
    {
        //If the player supported the game, they skip the ad
        if (GlobalStats.disabledAds)
        {
            watchedAds += 2; //1 life has to be with coins
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
                watchedAds++;
                Revive();

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                watchedAds++; //No more chances if they skip :P

                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }


#endif

    public void PayCoins()
    {
        int cost = 10 + (5 * coinCostMultiplier);
        int maxCost = 10 + (5 * 3);

        //Pay up and increase the cost next time.
        if (GlobalStats.coins >= cost)
        {

            GlobalStats.coins -= cost;
            coinCostMultiplier++;


            //Cap this off at 3 revives? We don't want unlimited lives after all..
            if ((cost >= maxCost)) payedCoins = true;

            //Update the button
            cost = 10 + (5 * coinCostMultiplier);
            coinButtonText.text = "Pay " + cost.ToString() + " Coins";
            
            //Actually revive the player too.
            Revive();
        }

    }
}
