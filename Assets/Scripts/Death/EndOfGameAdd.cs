using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class EndOfGameAdd : MonoBehaviour
{
    public static int gamesPlayed = 0;

    public void ShowAd()
    {
        //If the player supported the game, they skip the ad
        if (!GlobalStats.disabledAds && (gamesPlayed % 3 == 0))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

#if !UNITY_STANDALONE_WIN

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

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");

                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }


#endif
}
