using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Contains all powerup collectables, calling a specific function for each powerup.
public class Powerup : MonoBehaviour
{
    //0: Coin, 1: Ghost, 2: Score Multiplier
    public int powerupID;

    public delegate void collectFunction(Collider other);
    collectFunction functionToCall;

    // Start is called before the first frame update
    void Start()
    {
        switch(powerupID)
        {
            default:
                functionToCall = new collectFunction(PickupCoin);
                break;
            case 1:
                functionToCall = new collectFunction(PickupGhost);
                break;
            case 2:
                functionToCall = new collectFunction(PickupPointMultiplier);
                break;
            case 3:
                functionToCall = new collectFunction(PickupFire);
                break;
        }
    }

    // Rotate the powerups to give them some life.
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 15.0f * Time.deltaTime, 0.0f));
    }

    void  PickupCoin(Collider other)
    {
        GlobalStats.coins+= 5;
        if(GlobalStats.disabledAds) GlobalStats.coins += 5; //Coin pickups x2 if ads were removed
        GlobalStats.hud.UpdateHUD();
        GlobalStats.hud.DisplayCoins(false);
        SoundManager.PlaySound(SoundManager.Sounds.COIN);
    }

    void PickupGhost(Collider other)
    {
        DeathCheck snake = other.GetComponent<DeathCheck>();
        snake.MakeInvincible(8.0f);
        CoinObjective.CheckForObjective((int)CoinObjective.Objective.FIND_POWERUP, 0);
        SoundManager.PlaySound(SoundManager.Sounds.POWERUP);
    }

    void PickupPointMultiplier(Collider other)
    {
        SnakeFood.AddMultiplier();
        CoinObjective.CheckForObjective((int)CoinObjective.Objective.FIND_POWERUP, 1);
        SoundManager.PlaySound(SoundManager.Sounds.POWERUP, 1.5f);
    }

    void PickupFire(Collider other)
    {
        other.gameObject.transform.Find("Tongue").GetComponent<FireBreathe>().ActivateFire();
        CoinObjective.CheckForObjective((int)CoinObjective.Objective.FIND_POWERUP, 2);
        SoundManager.PlaySound(SoundManager.Sounds.POWERUP, 0.5f);
    }

    //Player collects powerup and a function is called.
    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            //Toggle boost with powerup objective
            if (other.GetComponent<Snake>().GetBoostGuage() > 0.0f) CoinObjective.CheckForObjective((int)CoinObjective.Objective.BOOST_POWERUP);


            functionToCall.Invoke(other);

            Destroy(gameObject);
        }
    }
}
