using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    //Movement vars
    public float speed;
    public bool alive; //When not alive, the game is forced to be paused
    DraggedDirection currentDirection;
    DraggedDirection oldDirection;
    Rigidbody rigidbody;

    [HideInInspector]
    public Vector3 lastGroundedPosition;
    
    //Touch input
    Vector3 startSwipePos;
    Vector3 endSwipePos;
    float timeSwipeHeld;

    //Var for rotating
    Quaternion targetRotation;
    float timeRotating;

    //Other snake segments
    int segments;
    float positionRecordTime;
    float timeBetweenPositions;
    public List<Vector3> prevPositions;
    public GameObject segmentReference;
    public GameObject segmentLinkReference;
    List<GameObject> otherSegments;

    public GameObject cam;
    Vector3 camOffset;

    //Skin vars
    Skins skinObject;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenPositions = 0.275f;
        positionRecordTime = 0.0f;
        timeSwipeHeld = 0.0f;

        prevPositions = new List<Vector3>();
        otherSegments = new List<GameObject>();
        segments = 0;

        rigidbody = GetComponent<Rigidbody>();
        currentDirection = DraggedDirection.Up;

        alive = true;
        timeRotating = 1.0f;

        camOffset = new Vector3(0.0f, 20.0f, 0.0f);

        rend = GetComponent<Renderer>();
        skinObject = GameObject.Find("SkinHandler").GetComponent<Skins>();
        ChangeSnakeSkin();
    }

    //Update the snake skin and colour variable. Called whenever the player swaps skins beforet the game starts.
    public void ChangeSnakeSkin()
    {
        Skins.CheckForRandomization();
        rend.material = skinObject.snakeSkins[(int)Skins.snakeSkin];
        GetComponent<DeathCheck>().regColor = rend.material.color;
    }

    // Input, visual and segment updating happens here!
    void Update()
    {
        //-----------------------------------------------------------------------------------
        //Detect Touch Input here!
        //-----------------------------------------------------------------------------------
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
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
                    if (timeSwipeHeld > 0.0f)
                    {
                        ChangeDirection();
                        timeSwipeHeld = 0.0f;
                    }
                    break;


            }

            //End the swipe if its too long
            if (touch.deltaPosition.magnitude > 2.0f) ForceEndSwipe(touch);
        }

        if (CheckIfPaused()) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) currentDirection = DraggedDirection.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) currentDirection = DraggedDirection.Right;
        if (Input.GetKeyDown(KeyCode.UpArrow)) currentDirection = DraggedDirection.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) currentDirection = DraggedDirection.Down;

        //Camera moves with player except for the y
        cam.transform.position = transform.position + camOffset;
        

        LerpToDirection();

        //Segment updating

        for (int i = 0; i < otherSegments.Count; i++)
        {
            otherSegments[i].transform.position = prevPositions[prevPositions.Count - (1 + i)]; //Error here
        }

        RecordPositions();

        //Update score multiplier powerup here.
        SnakeFood.CheckScoreMultiplier();


    }

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
    
    //Movement updating
    void FixedUpdate()
    {

        if(CheckIfPaused()) return;

        switch (currentDirection)
        {

            case DraggedDirection.Up:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, speed * Time.deltaTime));
                targetRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case DraggedDirection.Down:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, -speed * Time.deltaTime));
                targetRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case DraggedDirection.Left:
                rigidbody.velocity = (new Vector3(-speed * Time.deltaTime, rigidbody.velocity.y, 0.0f));
                targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case DraggedDirection.Right:
                rigidbody.velocity = (new Vector3(speed * Time.deltaTime, rigidbody.velocity.y, 0.0f));
                targetRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                timeRotating = 0.0f;
                break;
        }

    }

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
            //Resume the game upon swiping to move
            if (timeSwipeHeld > 0.10f)
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

    void RecordPositions()
    {
        //Record positions every now and then
        positionRecordTime += Time.deltaTime;

        if (positionRecordTime > timeBetweenPositions)
        {
            positionRecordTime = 0.0f;

            //Record last grounded position for when the player falls off the world... (Position from one of the last segments)
            if (rigidbody.velocity.y == 0.0f && prevPositions.Count > 2)
            {
                    //Ideally 6 positions away would work but the player will need that many segments
                    for (int i = 6; i > 2; i--)
                    {
                        if (prevPositions.Count > i)
                        {
                            lastGroundedPosition = prevPositions[prevPositions.Count - i];
                            break;
                        }
                    }
            }

            //Record position for the snake segments
            prevPositions.Add(AlignToGrid(transform.position));

            //Remove older uneeded positions
            if (prevPositions.Count > segments + 1)
            {
                prevPositions.RemoveAt(0);
            }
        }
    }

    public void AddSegment()
    {
        if (segments >= 100) return;

        //Make the new segment have the same material!
        segmentReference.GetComponent<Renderer>().material = GetComponent<Renderer>().material;

        segments++;
        otherSegments.Add(Instantiate(segmentReference));

        //This segment can refer to the owner when the owner dies.
        otherSegments[otherSegments.Count - 1].GetComponent<SnakeSegment>().snakeOwner = gameObject;

        //The first segment you cant collide with cause collision problems
        if (segments == 1)
        {
            otherSegments[0].name = "First Segment";
        }

        //Link segments together visually
        SegmentLink sl = Instantiate(segmentLinkReference).GetComponent<SegmentLink>();

        if (segments > 1) //For actual segments
        {
        sl.targetObject = otherSegments[otherSegments.Count - 1];
        sl.targetObject2 = otherSegments[otherSegments.Count - 2];
        }
        else //For linking a segment to this object.
        {
            sl.targetObject = otherSegments[otherSegments.Count - 1];
            sl.targetObject2 = gameObject;
        }

        //Force prevPositions to update
        positionRecordTime = 99.0f;
        RecordPositions();
    }

    //Call this whenever the snake grows, but not limit it eventually.
    public void ZoomOutCamera()
    {
        if (camOffset.y < 30.0f)
        camOffset += new Vector3(0.0f, 0.1f, 0.0f);
    }
    void ChangeDirection()
    {
        Vector3 dragVectorDirection = (endSwipePos - startSwipePos).normalized;
        Debug.Log("norm + " + dragVectorDirection);

        //Swap directions here. If it's a new direction then align to the grid!
        oldDirection = currentDirection;

        Debug.Log(oldDirection);
        currentDirection = GetDragDirection(dragVectorDirection);

        if (currentDirection != oldDirection) transform.position = AlignToGrid(transform.position);
    }
    
    //private void OnMouseUp() //For PC testing. Disable on release.
    //{
    //    endSwipePos = Input.mousePosition;

    //    ChangeDirection();
    //}

    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    
    private DraggedDirection GetDragDirection(Vector3 dragVector)
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

    //Snap in place to grid so its like traditional snake
    public Vector3 AlignToGrid(Vector3 oldPos)
    {
        return oldPos;

        //return new Vector3(Mathf.Round(oldPos.x), transform.position.y, Mathf.Round(oldPos.z));
    }
    
    public void Pause()
    {
        if (alive)
        {
            GlobalStats.paused = !GlobalStats.paused;
            GlobalStats.hud.ShowChildren();
        }
    }

    public void MakeAlive() { alive = true; }

    void LerpToDirection()
    {
        if (timeRotating < 1.0f)
        {
            timeRotating += Time.deltaTime * 8.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeRotating);
            
        }
    }


    //public GUIStyle style;

    //private void OnGUI()
    //{
    //    if (Input.touchCount > 0)
    //    {


    //    Touch touch =Input.GetTouch(0);

    //        GUI.color = Color.red;
    //        GUI.Label(new Rect(new Vector2(32.0f, 128.0f), new Vector2(100.0f, 32.0f)), "Swipe Length: " + touch.deltaPosition.magnitude.ToString(), style);

    //    }

    //    GUI.color = Color.blue;
    //    GUI.Label(new Rect(new Vector2(32.0f, 160.0f), new Vector2(100.0f, 32.0f)), "FPS: " + (60.0f * (1.0f - Time.deltaTime)).ToString(), style);
    //    GUI.Label(new Rect(new Vector2(32.0f, 192.0f), new Vector2(100.0f, 32.0f)), "DT : " + Time.deltaTime.ToString(), style);

    //}
}
