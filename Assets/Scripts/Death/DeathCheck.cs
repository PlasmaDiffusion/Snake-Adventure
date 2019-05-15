using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathCheck : MonoBehaviour
{
    //Menu to active upon death
    public GameObject deathMenu;

    public static float deathY;

    Renderer rend;
    Color regColor;

    bool colliding;
    bool died;

    //Time it takes to die
    float collisionTimeLimit;
    float collisionTimeInSegment;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        regColor = rend.material.color;
        collisionTimeLimit = 1.0f;
        collisionTimeInSegment = 0.0f;

        colliding = false;
        deathY = -10.0f;
    }

    //If you're touching nothing you'll gain your time back
    void Update()
    {
        if (!colliding && collisionTimeInSegment > 0.0f)
        {
            collisionTimeInSegment -= Time.deltaTime;
            UpdateColour();
        }

        if (gameObject.transform.position.y < deathY)
        {
            Die();
        }

    }

    //Pop up the game over screen and make the snake stop moving.
    void Die()
    {
        if (died) return;
        deathMenu.SetActive(true);
        gameObject.GetComponent<Snake>().canMove = false;

        died = true;
    }

    public void Respawn()
    {
        died = false;
        gameObject.GetComponent<Snake>().canMove = true;
    }

    //You can only collide for so long until ya die.
    public void CheckForDeath()
    {
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
        rend.material.color = new Color((collisionTimeInSegment / collisionTimeLimit), rend.material.color.g, rend.material.color.b);
    }
}
