using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//UI Script for both the options AND SHOP MENU.
public class OptionsMenu : MonoBehaviour
{
    //Ui variables
    public GameObject confirmObject;
    public Button yesButton;
    public Button noButton;
    public Text titleText;
    public Text descText;

    //Flag if something was deleted
    bool deletedData;
    
    // Start is called before the first frame update
    void Start()
    {
        deletedData = false;
    }

    private void OnEnable()
    {
        deletedData = false;
    }

    public void ShowConfirmation(int buttonID)
    {
        //Ready buttons for result window
        yesButton.gameObject.SetActive(true);
        noButton.transform.GetChild(0).GetComponent<Text>().text = "No";

        switch (buttonID)
        {
            case 0:
                yesButton.onClick.AddListener(DeleteSkins);
                titleText.text = "Delete All Skins";
                descText.text = "This will remove all skins. You'll have to buy all of them again. Are you sure?";
                break;
            case 1:
                yesButton.onClick.AddListener(DeleteThemes);
                titleText.text = "Delete All Themes";
                descText.text = "This will remove all themes. You'll have to buy all of them again. Are you sure?";
                break;
            case 2:
                yesButton.onClick.AddListener(DeleteScores);
                titleText.text = "Delete Hi-Scores";
                descText.text = "This will reset the high score and farthest level. Are you sure?";
                break;
            case 3:
                yesButton.onClick.AddListener(RemoveAds);
                titleText.text = "Remove Ads + X2 Coin Pickups \n $2.99 CAD";
                descText.text = "Support the game and remove the need to watch an ad for a free respawn. As an extra bonus, coin pickups will be worth double!\nWant to buy this?";
                break;
        }

        confirmObject.SetActive(true);
    }

    public void HideConfirmation()
    {
        confirmObject.SetActive(false);
    }

    //Upon exiting, reload the scene if something was deleted.
    public void ExitMenu()
    {
        if (deletedData) SceneManager.LoadScene(0);
    }

    public void DeleteSkins()
    {
        //Remove all unlocked skins
        for (int i = 0; i < Skins.unlockedSnakeSkins.Length; i++)
        {
            Skins.unlockedSnakeSkins[i] = false;
        }
        //Reset to defaults
        Skins.unlockedSnakeSkins[0] = true;
        Skins.snakeSkin = Skins.SnakeSkins.DEFAULT;
        Skins.randomSkin = false;

        GlobalStats.Save();

        deletedData = true;
        ShowResult(0);
    }

    public void DeleteThemes()
    {
        //Remove all unlocked skins
        for (int i = 0; i < Skins.unlockedLevelThemes.Length; i++)
        {
            Skins.unlockedLevelThemes[i] = false;
        }
        //Reset to defaults
        Skins.unlockedLevelThemes[0] = true;
        Skins.levelTheme = Skins.Themes.DEFAULT;
        Skins.randomTheme = false;

        GlobalStats.Save();

        deletedData = true;
        ShowResult(1);
    }

    public void DeleteScores()
    {
        GlobalStats.ResetScores();
        GlobalStats.Save();

        deletedData = true;
        ShowResult(2);
    }

    public void RemoveAds()
    {
        //Don't buy if already owned
        if (GlobalStats.disabledAds)
        {
            ShowResult(5);
        }
        else
        {
            //Buy with IAP
            titleText.text = "Purchasing...";
            GetComponent<ShopPurchaser.Purchaser>().BuyNonConsumable();
        }
        
    }

    //Repurpose confirm menu to show results of what just happened.
    public void ShowResult(int resultID, string error = "")
    {
        confirmObject.SetActive(true);

        //Ready buttons for result window
        yesButton.gameObject.SetActive(false);
        noButton.transform.GetChild(0).GetComponent<Text>().text = "OK";

        switch (resultID)
        {
            case 0:
                titleText.text = "Deleted!";
                descText.text = "All Snake Skins deleted.";
                break;
            case 1:
                titleText.text = "Deleted!";
                descText.text = "All Level Themes deleted.";
                break;
            case 2:
                titleText.text = "Deleted!";
                descText.text = "Best score and level deleted.";
                break;
            case 3:
                titleText.text = "Ads Removed!";
                descText.text = "Purchase successful. Enjoy no ads and double coin pickups!";
                break;
            case 4:
                titleText.text = "Purchase Failed";
                descText.text = error;
                break;
            case 5:
                titleText.text = "Already Bought";
                descText.text = "Ads were already removed.";
                break;
        }
    }

}
