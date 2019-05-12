using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class DeathHandler : MonoBehaviour
{
    bool watchedAd;
    bool payedCoins;

    public GameObject player;
    public GameObject adButton;
    public GameObject coinButton;
    public GameObject continueText;

    // Start is called before the first frame update
    void Start()
    {
        watchedAd = false;
        payedCoins = false;
        
    }
    
    //Call Die() to prompt a game over menu. 
    void OnEnable()
    {
        gameObject.SetActive(true);

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
        gameObject.SetActive(false);
        player.GetComponent<DeathCheck>().Respawn();
    }

    // Update is called once per frame
    void EndGame()
    {
        
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

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
        if (11 > 10)
        {

            payedCoins = true;
            Revive();
        }

    }
}
