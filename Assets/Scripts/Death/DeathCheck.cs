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

    int lives;

    //Extra lives gained
    int oneUps;

    //Var for rainbow skin
    Color rainbowColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        regColor = rend.material.color;
        collisionTimeLimit = 1.0f;
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

        lives = 3;
        oneUps = 0;

        rainbowColor = rend.material.color;
    }

    //If you're touching nothing you'll gain your time back
    void Update()
    {


        DeathLerpAnimation();

        if (GlobalStats.paused) return;

        //Dye if you fall down too far
        if (gameObject.transform.position.y < deathY)
        {
            //Debug.Log("Dying because under y level " + deathY.ToString());
            invincibility = 0.0f;
            UpdateColour();
            Die(true);
        }
        
        //Can't run over self while boosting
        if (snake.GetBoosting()) { BoostColourLerp(); return; }

            //Change colour and record when colliding
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
        else if (Skins.snakeSkin == Skins.SnakeSkins.RAINBOW && collisionTimeInSegment < 0.1f)
        {
            RainbowLerp();
        }




        }

    //Pop up the game over screen and make the snake stop moving. Return true if died for other objects to know.
    public bool Die(bool forceDeath = false)
    {
        if (forceDeath) { } //Skip the check below
        else if (died || invincibility > 0.0f || snake.GetBoosting()) return false;

        gameObject.GetComponent<Snake>().alive = false;
        GlobalStats.hud.DisplayCoins(true);
        GlobalStats.paused = true;

        SoundManager.PlaySound(SoundManager.Sounds.DIE);

        increasingT = 1.0f;
        died = true;

        return true;
    }

    private void RespawnOrGameOver()
    {
        //Die -> respawn or game over?
        RemoveLife();
        if (lives > 0)
        {
            Respawn();
        }
        else
        {
            deathMenu.SetActive(true);
        }
    }

    //Life management. Can only increment or decrement. ------------------------------------------
    //Bonus life
    public void AddLifeFromScore()
    {
        //This only happens at 5000, 10000, 25000. (3 extra lives total)
        if (oneUps < 3 && GlobalStats.score > ((oneUps*oneUps)+1) * 5000)
        {
        oneUps++;
        lives++;
        GlobalStats.hud.UpdateLifeHUD();
        SoundManager.PlaySound(SoundManager.Sounds.ONEUP, 1.0f, -1);
        }
    }
    //Simple add life function, for respawning.
    public void AddLife()
    {
        lives++;
        GlobalStats.hud.UpdateLifeHUD();
    }
    void RemoveLife() { lives--; GlobalStats.hud.UpdateLifeHUD(); }
    public int GetLives() { return lives; }

    public float GetCollisionTimeInSegment() { return collisionTimeInSegment; }

    //Call this to make the snake alive and await a swipe to resume the game.
    public void Respawn()
    {
        
        died = false;
        Snake snake = gameObject.GetComponent<Snake>();
        snake.alive = true;
        snake.MakeAlive();

        if (gameObject.transform.position.y < deathY)
        {
            transform.position = snake.lastSpawnPosition;
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
        if (invincibility > 0.0f || snake.GetBoosting()) return;

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
        else if(Skins.snakeSkin == Skins.SnakeSkins.DICE || Skins.snakeSkin == Skins.SnakeSkins.BRICK) rend.material.color = new Color(regColor.r - (regColor.r) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
        else if (Skins.snakeSkin == Skins.SnakeSkins.CHECKERED || Skins.snakeSkin == Skins.SnakeSkins.GOLD) rend.material.color = new Color(regColor.r, regColor.g - (regColor.g) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
        else if (Skins.snakeSkin == Skins.SnakeSkins.SPOTS || Skins.snakeSkin == Skins.SnakeSkins.GLAMOUR) rend.material.color = new Color(regColor.r - (regColor.r) * (collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b, 1.0f - (invincibility / maxInvincibility));
        else if (Skins.snakeSkin == Skins.SnakeSkins.RAINBOW) rend.material.color = new Color(rainbowColor.r - (rainbowColor.r) * (collisionTimeInSegment / collisionTimeLimit), rainbowColor.g + (rainbowColor.g) * (collisionTimeInSegment / collisionTimeLimit), rainbowColor.b + (rainbowColor.b) * (collisionTimeInSegment / collisionTimeLimit), 1.0f - (invincibility / maxInvincibility));
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
        if (deathT > 1.0f) { deathT = 1.0f; increasingT = 0.0f;

            RespawnOrGameOver();
        }
        else if (deathT < 0.0f) {deathT = 0.0f; increasingT = 0.0f; }

        transform.localScale = Vector3.Lerp(maxScale, minScale, deathT);

        //Resize the snake segments too
        snake.RescaleSegments();
    }

    //Snake glows colours when boosting
    void BoostColourLerp()
    {
        Color colorToLerpTo = Color.blue;
        float boostAmount = snake.GetBoostGuage();

        //Blue snake needs a different colour
        if (Skins.snakeSkin == Skins.SnakeSkins.BLUE) colorToLerpTo = Color.yellow;


        if (boostAmount > 0.5f)
        rend.material.color = Color.Lerp(colorToLerpTo, regColor, boostAmount);
        else
        rend.material.color = Color.Lerp(regColor, colorToLerpTo, boostAmount);
    }

    void RainbowLerp()
    {
        float hue;
        float sat;
        float value;


        Color.RGBToHSV(rainbowColor, out hue, out sat, out value);

        hue += Time.deltaTime * 0.25f;

        rainbowColor = Color.HSVToRGB(hue, sat, value);
        
        rend.material.color = rainbowColor;
    }
}
