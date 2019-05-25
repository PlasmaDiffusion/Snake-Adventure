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

    //

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

        camOffset = new Vector3(0.0f, 12.5f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Can't move if game is paused, game over, etc.
        if (GlobalStats.paused || !alive)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) currentDirection = DraggedDirection.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) currentDirection = DraggedDirection.Right;
        if (Input.GetKeyDown(KeyCode.UpArrow)) currentDirection = DraggedDirection.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) currentDirection = DraggedDirection.Down;

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
                    if (timeSwipeHeld > 0.15f)
                    {
                        //Either the player releases or holds their swipe long enough
                        endSwipePos = touch.position;
                        ChangeDirection();
                        timeSwipeHeld = 0.0f;
                    }
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
        }

            switch (currentDirection)
        {
               
            case DraggedDirection.Up:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, speed* Time.deltaTime));
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

    void RecordPositions()
    {
        //Record positions
        positionRecordTime += Time.deltaTime;

        if (positionRecordTime > timeBetweenPositions)
        {
            positionRecordTime = 0.0f;


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
        if (camOffset.y < 20.0f)
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
        GlobalStats.paused = !GlobalStats.paused;
    }

    void LerpToDirection()
    {
        if (timeRotating < 1.0f)
        {
            timeRotating += Time.deltaTime * 4.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeRotating);
            
        }
    }
}
