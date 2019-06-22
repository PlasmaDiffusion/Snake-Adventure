using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathCheck : MonoBehaviour
{
    //Menu to active upon death
    public GameObject deathMenu;

    public static float deathY;

    Renderer rend;
    [HideInInspector]
    public Color regColor;

    bool colliding;
    bool died;

    //Time it takes to die
    float collisionTimeLimit;
    float collisionTimeInSegment;

    public float invincibility;
    public float maxInvincibility;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        regColor = rend.material.color;
        collisionTimeLimit = 0.75f;
        collisionTimeInSegment = 0.0f;

        colliding = false;
        deathY = -10.0f;
        invincibility = 0.0f;
        maxInvincibility = 1.0f;
    }

    //If you're touching nothing you'll gain your time back
    void Update()
    {
        if (GlobalStats.paused) return;

        if (!colliding && collisionTimeInSegment > 0.0f)
        {
            collisionTimeInSegment -= Time.deltaTime;
            UpdateColour();
        } //Remove invincibility buff
        else if(invincibility > 0.0f)
        {
            //Remove invinibility here but not if snake is boosting
            if(!Snake.boosting) invincibility -= Time.deltaTime;

            //Make transparent
            UpdateColour();
        }

        if (gameObject.transform.position.y < deathY)
        {
            Debug.Log("Dying because under y level " + deathY.ToString());
            Die();
        }



        }

    //Pop up the game over screen and make the snake stop moving.
    public void Die()
    {
        if (died || invincibility > 0.0f) return;
        deathMenu.SetActive(true);
        gameObject.GetComponent<Snake>().alive = false;

        GlobalStats.paused = true;

        died = true;
    }

    //Call this to make the snake alive and await a swipe to resume the game.
    public void Respawn()
    {
        
        died = false;
        Snake snake = gameObject.GetComponent<Snake>();
        snake.alive = true;
        snake.MakeAlive();

        if (gameObject.transform.position.y < deathY) transform.position = snake.lastGroundedPosition;

        //Respawn with some invincibility
        MakeInvincible(4.0f);
        StopCheck();
    }

    //You can only collide for so long until ya die.
    public void CheckForDeath()
    {
        if (invincibility > 0.0f) return;

        collisionTimeInSegment += Time.deltaTime;
        if (collisionTimeInSegment > collisionTimeLimit) Die();

        UpdateColour();

        colliding = true;
    }

    public void StopCheck()
    {
        rend.material.color = regColor;
        colliding = false;
    }

    void UpdateColour()
    {
        //Tint yellow to show near death
        rend.material.color = new Color(regColor.r + (1.0f-regColor.r) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b, 1.0f - (invincibility/maxInvincibility));
    }

    public void MakeInvincible(float time)
    {
        invincibility = time;
        maxInvincibility = time;
    }
}
