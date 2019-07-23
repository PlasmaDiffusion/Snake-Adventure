using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopNotifications : MonoBehaviour
{
    public GameObject snakeSkinNotification;
    public GameObject levelThemeNotification;

    bool haveAllSnakeSkins;
    bool haveAllLevelThemes;

    // Start is called before the first frame update
    void Start()
    {
        haveAllSnakeSkins = false;
        haveAllLevelThemes = false;

        CheckForShopNotifications();

    }

    private void OnEnable()
    {
        //CheckForShopNotifications();
    }

    public void CheckForShopNotifications()
    {
        //Check if the player already owns everything.
        haveAllSnakeSkins = Skins.HaveAllSnakeSkins();
        haveAllLevelThemes = Skins.HaveAllLevelThemes();

        //Notify the player if they can buy a new skin
        if (GlobalStats.coins > Skins.skinCost && !haveAllSnakeSkins) snakeSkinNotification.SetActive(true);
        else snakeSkinNotification.SetActive(false);
        if (GlobalStats.coins > Skins.themeCost && !haveAllLevelThemes) levelThemeNotification.SetActive(true);
        else levelThemeNotification.SetActive(false);

    }



    // Update is called once per frame
    void Update()
    {

    }
}
