using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The main player class.
public class Snake : SnakeMovement
{
    //Movement vars
    
    public bool alive; //When not alive, the game is forced to be paused

    //Vars for respawning after falling
    [HideInInspector]
    public Vector3 lastSpawnPosition;

    //Touch input
    Vector3 startSwipePos;
    Vector3 endSwipePos;
    float timeSwipeHeld;


    //Boost
    float boostGuage;
    public static bool boosting;
    ParticleSystem boostEmitter;

    public GameObject cam;
    Vector3 camOffset;

    //Skin vars
    Skins skinObject;
    Renderer rend;

    //Menu checks for resuming the game
    [Header("Menu References for pausing/unpausing")]
    public GameObject mainMenuObject;
    public GameObject gameOverMenuObject;
    public GameObject pauseMenuObject;

    // Start is called before the first frame update
    void Start()
    {
        //Snake segment related
        timeBetweenPositions = 0.175f;
        positionRecordTime = 0.0f;

        //Input swipe related
        timeSwipeHeld = 0.0f;

        //Snake segment related
        prevPositions = new List<Vector3>();
        otherSegments = new List<GameObject>();
        segments = 0;

        //Boost mechanic related
        boostGuage = 0.0f;
        boosting = false;
        boostEmitter = GetComponent<ParticleSystem>();

        //Movement related
        rigidbody = GetComponent<Rigidbody>();
        currentDirection = DraggedDirection.Up;
        moveRate = speed;

        alive = true;
        timeRotating = 1.0f;

        InitSnakeMovement();

        lastSpawnPosition = transform.position;

        camOffset = new Vector3(0.0f, 25.0f, 0.0f);

        //Change skins to whatever is set.
        rend = GetComponent<Renderer>();
        //ChangeSnakeSkin(); //Skins will call this instead.

        //Reset food collecting variables
        BonusFood.foodCollected = 0;
        BonusFood.currentFoodIndex = 0;

        transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
    }

    //Update the snake skin and colour variable. Called whenever the player swaps skins beforet the game starts.
    public void ChangeSnakeSkin()
    {
        //Make sure object exists
        if (!skinObject) skinObject = GameObject.Find("SkinHandler").GetComponent<Skins>();
        if (!rend) rend = GetComponent<Renderer>();

        //Update the skin, but not if it's random (index would be out of range)
        if (Skins.snakeSkin != Skins.SnakeSkins.RANDOM)
        {
            Debug.Log("Changing to skin index " + Skins.snakeSkin);
            rend.material = skinObject.snakeSkins[(int)Skins.snakeSkin];
            GetComponent<DeathCheck>().regColor = rend.material.color;

            //Update food objects too
            GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");

            foreach (GameObject item in foodObjects)
            {
                item.GetComponent<SnakeFood>().ChangeSkin();
            }

            //Recolour the link to be a darkened version of this skin
            //0/103
            //190/248
            //118/71
            Renderer linkRend = segmentLinkReference.GetComponent<Renderer>();
            if (Skins.snakeSkin != Skins.SnakeSkins.DEFAULT)
            {
                
                linkRend.material.color = (rend.material.color * new Color(0.5f, 0.5f, 0.5f, 1.0f));
                //Color darkenedCol = linkRend.material.color;
                //darkenedCol = new Color(darkenedCol.r * 1.145f, darkenedCol.g * 0.766f, darkenedCol.b * 1.661f);
                //linkRend.material.color = darkenedCol;

            }
            else //Default colour isn't quite a darkened version of the default snake. Set it here.
            {
                linkRend.material.color = new Color(0.0f, 190.0f / 255.0f, 118.0f / 255.0f);
            }
        }
        else
        {
            Debug.Log("Randomness failed");
            //Out of index Failsafe for when its random but a skin hasnt been picked yet.
            //Skins.CheckForRandomization(true);
            //ChangeSnakeSkin();
        }
    }

    // Input, visual and segment updating happens here!
    void Update()
    {
        //-----------------------------------------------------------------------------------
        //Detect Touch Input here!
        //-----------------------------------------------------------------------------------
        if (Input.touchCount > 0 && GlobalStats.swipeControls)
        {
            Touch touch = Input.GetTouch(0);

            bool disableBottomScreenInput = false;

            //Can't move from where boost button is
            if (boostGuage >= 1.0f && touch.position.y < 300.0f
            && touch.position.x > 400.0f && touch.position.x < 800.0f) disableBottomScreenInput = true;

            if (!disableBottomScreenInput)
            { 
                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        startSwipePos = touch.position;
                        

                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        timeSwipeHeld += Time.deltaTime;
                        touch = ForceEndSwipe(touch);
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        endSwipePos = touch.position;
                        if (timeSwipeHeld > 0.0f )
                        {
                            ChangeDirection();
                            timeSwipeHeld = 0.0f;
                        }
                        break;


                }

                //End the swipe if its too long
                if (touch.deltaPosition.magnitude > 2.0f) ForceEndSwipe(touch);
            }
        }
        //Dont move and turn gravity off when paused
        if (CheckIfPaused())
        {
            rigidbody.useGravity = false;
            return;
        }
        else rigidbody.useGravity = true;

        float randomMovePitch = Random.Range(1.0f, 2.5f);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { currentDirection = DraggedDirection.Left; SoundManager.PlaySound(SoundManager.Sounds.SWITCH_OFF, randomMovePitch); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { currentDirection = DraggedDirection.Right; SoundManager.PlaySound(SoundManager.Sounds.SWITCH_OFF, randomMovePitch); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {currentDirection = DraggedDirection.Up;  SoundManager.PlaySound(SoundManager.Sounds.SWITCH_OFF, randomMovePitch); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {currentDirection = DraggedDirection.Down;  SoundManager.PlaySound(SoundManager.Sounds.SWITCH_OFF, randomMovePitch); }




        //Segment updating

        for (int i = 0; i < otherSegments.Count; i++)
        {
            otherSegments[i].transform.position = prevPositions[prevPositions.Count - (1 + i)]; //Error here
        }

        //RecordPositions();

        //Call this to actually move
        MoveAndSnapToGrid();

        LerpToDirection();

        //Camera moves with player except for the y
        cam.transform.position = transform.position + camOffset;

        //Update score multiplier powerup here.
        SnakeFood.CheckScoreMultiplier();

        UpdateBoost();


    }

    //Collision function that detects if you're about to glitch under a wall and forces you back up
    //private void OnTriggerEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Wall")
    //    {
    //        //Must be under the wall
    //            if(rigidbody.velocity.y < 0.0f)
    //            {
    //            Debug.Log("Forced up");
    //             transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
    //            }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name[0] == 'U')
    //    {
    //        //Must be under the wall
    //        //if (transform.position.y < other.transform.position.y)
    //        //{
    //            transform.position = new Vector3(transform.position.x, other.transform.position.y + 2.0f, transform.position.z);
    //            Debug.Log(other.name);
    //            //other.transform.position += new Vector3(1.0f, 1.0f, 1.0f);

    //        //}
    //    }
    //}


    private Touch ForceEndSwipe(Touch touch)
    {
        if (timeSwipeHeld > 0.15f)
        {
            //Either the player releases or holds their swipe long enough
            endSwipePos = touch.position;
            ChangeDirection();
            timeSwipeHeld = 0.0f;
        }

        return touch;
    }

    public void ChangeSnakeDirection(int directionID)
    {
        if (currentDirection != (DraggedDirection)directionID)
        {
            float randomMovePitch = Random.Range(1.0f, 2.5f);
            SoundManager.PlaySound(SoundManager.Sounds.SWITCH_OFF, randomMovePitch);
        }

        currentDirection = (DraggedDirection)directionID;
    }

    //Set vel to 0 if the game is paused. Also check for unpause.
    bool CheckIfPaused()
    {
        //Can't move if game is paused, game over, etc.
        if (GlobalStats.paused && !alive)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            return true;
        }
        else if (GlobalStats.paused) //If paused and the player is alive...
        {
            //Resume the game upon swiping to move (AND on the game over/ pause screen
            if (timeSwipeHeld > 0.10f && (gameOverMenuObject.activeSelf || pauseMenuObject.activeSelf))
            {
                Pause();
            }
            else
            {
                rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                return true;
            }
        }

        return false;
    }

    //Record positions for the snake segments to follow.
    void RecordPositions()
    {
        //Record positions every now and then
        positionRecordTime += Time.deltaTime;

        if (positionRecordTime > timeBetweenPositions)
        {
            positionRecordTime = 0.0f;

            //Record last grounded position for when the player falls off the world... (Position from one of the last segments)
            //if (rigidbody.velocity.y == 0.0f && prevPositions.Count > 2)
            //{
            //    //Ideally 6 positions away would work but the player will need that many segments
            //    for (int i = 6; i > 2; i--)
            //    {
            //        if (prevPositions.Count > i)
            //        {
            //            lastGroundedPosition = prevPositions[prevPositions.Count - i];
            //            break;
            //        }
            //    }
            //}
            
            //Record position for the snake segments
            prevPositions.Add(AlignToGrid(transform.position));

            //Remove older uneeded positions
            if (prevPositions.Count > segments + 1)
            {
                prevPositions.RemoveAt(0);
            }
            
        }
    }

    //Call this when in the middle of the dying animation.
    public void RescaleSegments()
    {
        foreach (GameObject segment in otherSegments)
        {
            segment.transform.localScale = transform.localScale;
        }
    }

    //Call this whenever the snake grows, but not limit it eventually.
    public void ZoomOutCamera()
    {
        if (camOffset.y < 30.0f)
            camOffset += new Vector3(0.0f, 0.1f, 0.0f);
    }

    public void PrepRespawnFromFalling()
    {
        cam.transform.position = transform.position + camOffset;
        ResetLerpPositions();
    }

    //Change swipe directions
    void ChangeDirection()
    {
        Vector3 dragVectorDirection = (endSwipePos - startSwipePos).normalized;
        Debug.Log("norm + " + dragVectorDirection);

        //Swap directions here. If it's a new direction then align to the grid!
        oldDirection = currentDirection;

        Debug.Log(oldDirection);
        currentDirection = GetDragDirection(dragVectorDirection);

        if (currentDirection != oldDirection)
        {
        float randomMovePitch = Random.Range(1.0f, 2.5f);
        SoundManager.PlaySound(SoundManager.Sounds.SWITCH_OFF, randomMovePitch);
        }
        //if (currentDirection != oldDirection) transform.position = AlignToGrid(transform.position);
    }



    DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }


    //Call to toggle pause
    public void Pause()
    {
        if (alive)
        {
            GlobalStats.paused = !GlobalStats.paused;
            GlobalStats.hud.ShowChildren();

            GlobalStats.hud.boostMenuObject.SetActive(true);

            if (GlobalStats.paused)
                pauseMenuObject.SetActive(true);
            else
                pauseMenuObject.SetActive(false);
        }
    }

    public void MakeAlive() { alive = true; pauseMenuObject.SetActive(true); }

    //Every level the snake can slightly speed up. Cap the speed though to make it not impossible to control. 
    public void IncreaseSpeed()
    {
        if (speed < 8.0f)
        {
            speed += 0.1f;
            if (speed > 8.0f) speed = 8.0f;
            if(!boosting) moveRate = speed;
        }
    }


    //------------------------------------------------
    // Boost functions below.
    //------------------------------------------------

    //Tap the boost button to toggle boosting
    public void ToggleBoost()
    {
        if (boostGuage >= 1.0f) //Turn on boost
        {
            GlobalStats.AddScore(50, transform.position);
            moveRate = 10.0f;
            boosting = true;
            
            boostEmitter.Play();
            CoinObjective.CheckForObjective((int)CoinObjective.Objective.BOOST);
            SoundManager.PlaySound(SoundManager.Sounds.BOOST);
        }
        else //Turn off boost
        {
            moveRate = speed;

            boostEmitter.Stop();
            boosting = false;
        }
    }

    //Call every frame to remove the boost if its activated
    void UpdateBoost()
    {
        if (boosting)
        {
            boostGuage -= Time.deltaTime * 0.1f;

            //Check to turn boosting off
            if (boostGuage <= 0.0f)
            {
                ToggleBoost();
            }
        }
    }

    public void IncreaseBoostGuage()
    {
        boostGuage += 0.1f;
        if (boostGuage > 1.0f) boostGuage = 1.0f;
    }

    public float GetBoostGuage() { return boostGuage; }
    public bool GetBoosting() { return boosting; }

    //public GUIStyle style;

    //private void OnGUI()
    //{


    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);
    //        GUI.Label(new Rect(new Vector2(32.0f, 192.0f), new Vector2(100.0f, 32.0f)), touch.position.x.ToString(), style);
    //    }

    //    //GUI.color = Color.red;
    //    //GUI.Label(new Rect(new Vector2(32.0f, 128.0f), new Vector2(100.0f, 32.0f)), "Swipe Length: " + touch.deltaPosition.magnitude.ToString(), style);
    //    //GUI.color = Color.blue;
    //    //GUI.Label(new Rect(new Vector2(32.0f, 160.0f), new Vector2(100.0f, 32.0f)), "FPS: " + ((1.0f / Time.deltaTime)).ToString(), style);
    //    //GUI.Label(new Rect(new Vector2(32.0f, 192.0f), new Vector2(100.0f, 32.0f)), "DT : " + Time.deltaTime.ToString(), style);

    //}
}
