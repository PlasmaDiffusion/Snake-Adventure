using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Function as UI arrow keys
public class ArrowKeyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Snake snake;
    public int directionID;


    bool ispressed = false;

    // Start is called before the first frame update
    void Start()
    {
        ispressed = false;
        snake = GameObject.Find("Player").GetComponent<Snake>();
        
    }

    //Disable this
    public void OnEnable()
    {
        if (GlobalStats.swipeControls) transform.parent.gameObject.SetActive(false);
    }

    //Check constantly if pressed
    void Update()
    {
        //Hide buttons if snake is dead
        if (!snake.alive) transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        else transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);



        if (ispressed) MoveSnake();
    }

    //Emulate a swipe
    public void MoveSnake()
    {
        //This button can unpause the game
        if (GlobalStats.paused) snake.Pause();

        snake.ChangeSnakeDirection(directionID);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        ispressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ispressed = false;
    }

}
