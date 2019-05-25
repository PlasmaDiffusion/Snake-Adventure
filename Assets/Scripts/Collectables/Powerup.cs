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
        GlobalStats.coins++;
        GlobalStats.hud.UpdateHUD();
        GlobalStats.hud.DisplayCoins(false);
    }

    void PickupGhost(Collider other)
    {
        DeathCheck snake = other.GetComponent<DeathCheck>();
        snake.MakeInvincible(8.0f);
    }

    void PickupPointMultiplier(Collider other)
    {
        SnakeFood.AddMultiplier();
    }

    void PickupFire(Collider other)
    {
        other.gameObject.transform.Find("Tongue").GetComponent<FireBreathe>().ActivateFire();
    }

    //Player collects powerup and a function is called.
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            functionToCall.Invoke(other);

            Destroy(gameObject);
        }
    }
}
