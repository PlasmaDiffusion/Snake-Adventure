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

    //Death animation
    float deathT;
    float increasingT;
    Vector3 minScale;
    Vector3 maxScale;
    Snake snake;


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
        snake = GetComponent<Snake>();

        //Store scale for death animation
        deathT = 0.0f;
        increasingT = 0.0f;
        minScale = new Vector3(0.2f, 0.2f, 0.2f);
        maxScale = transform.localScale;
    }

    //If you're touching nothing you'll gain your time back
    void Update()
    {
        DeathLerpAnimation();

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
            //Debug.Log("Dying because under y level " + deathY.ToString());
            invincibility = 0.0f;
            UpdateColour();
            Die();
        }



        }

    //Pop up the game over screen and make the snake stop moving. Return true if died for other objects to know.
    public bool Die()
    {
        if (died || invincibility > 0.0f) return false;
        deathMenu.SetActive(true);
        gameObject.GetComponent<Snake>().alive = false;
        GlobalStats.hud.DisplayCoins(true);
        GlobalStats.paused = true;

        SoundManager.PlaySound(SoundManager.Sounds.DIE);

        increasingT = 1.0f;
        died = true;
        return true;
    }

    //Call this to make the snake alive and await a swipe to resume the game.
    public void Respawn()
    {
        
        died = false;
        Snake snake = gameObject.GetComponent<Snake>();
        snake.alive = true;
        snake.MakeAlive();

        if (gameObject.transform.position.y < deathY)
        {
            transform.position = snake.lastGroundedPosition;
            snake.PrepRespawnFromFalling();
        }
        //Respawn with some invincibility
        MakeInvincible(4.0f);
        increasingT = -1.0f;
        StopCheck();
    }

    //You can only collide for so long until ya die.
    public void CheckForDeath()
    {
        if (invincibility > 0.0f) return;

        collisionTimeInSegment += Time.deltaTime;
        if (collisionTimeInSegment > collisionTimeLimit * 0.25f) SoundManager.PlaySound(SoundManager.Sounds.HURT, 1.0f - collisionTimeInSegment);
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
        //Tint yellow to show near death (Or another colour if the skin doesn't work with adding to the red value)
        if (Skins.snakeSkin == Skins.SnakeSkins.DOTTED) rend.material.color = new Color(rend.material.color.r,regColor.g + (1.0f - regColor.g) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
        else if(Skins.snakeSkin == Skins.SnakeSkins.DICE) rend.material.color = new Color(regColor.r - (regColor.r) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
        else if (Skins.snakeSkin == Skins.SnakeSkins.CHECKERED) rend.material.color = new Color(regColor.r - (regColor.r) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
        else rend.material.color = new Color(regColor.r + (1.0f - regColor.r) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
    }

    public void MakeInvincible(float time)
    {
        invincibility = time;
        maxInvincibility = time;
    }

    //Flatten or inflate the snake's y scale.
    void DeathLerpAnimation()
    {
        if (increasingT == 0.0f) return;

        deathT += Time.deltaTime * increasingT;

        //If t is over 1 or under 0 then stop the animation
        if (deathT > 1.0f) { deathT = 1.0f; increasingT = 0.0f; }
        else if (deathT < 0.0f) {deathT = 0.0f; increasingT = 0.0f; }

        transform.localScale = Vector3.Lerp(maxScale, minScale, deathT);

        //Resize the snake segments too
        snake.RescaleSegments();
    }
}
