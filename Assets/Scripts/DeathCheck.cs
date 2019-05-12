using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCheck : MonoBehaviour
{
    //Menu to active upon death
    public GameObject deathMenu;

    Renderer rend;
    Color regColor;

    bool colliding;

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
    }

    //If you're touching nothing you'll gain your time back
    void Update()
    {
        if (!colliding && collisionTimeInSegment > 0.0f)
        {
            collisionTimeInSegment -= Time.deltaTime;
            UpdateColour();
        }

    }

    //You can only collide for so long until ya die.
    public void CheckForDeath()
    {
        collisionTimeInSegment += Time.deltaTime;
        if (collisionTimeInSegment > collisionTimeLimit)
        {
            deathMenu.SetActive(true);
            //gameObject.GetComponent<Snake>().die
        }

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
        rend.material.color += new Color((collisionTimeInSegment / collisionTimeLimit) * Time.deltaTime, 0.0f, 0.0f);
    }
}
