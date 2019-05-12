using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    public float speed;


    DraggedDirection currentDirection;
    DraggedDirection oldDirection;
    Rigidbody rigidbody;

    Vector3 startSwipePos;
    Vector3 endSwipePos;

    //Other snake segments
    int segments;
    float positionRecordTime;
    float timeBetweenPositions;
    public List<Vector3> prevPositions;
    public GameObject segmentReference;
    public GameObject segmentLinkReference;
    List<GameObject> otherSegments;

    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenPositions = 0.275f;
        positionRecordTime = 0.0f;

        prevPositions = new List<Vector3>();
        otherSegments = new List<GameObject>();
        segments = 0;

        rigidbody = GetComponent<Rigidbody>();
        currentDirection = DraggedDirection.Up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) currentDirection = DraggedDirection.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) currentDirection = DraggedDirection.Right;
        if (Input.GetKeyDown(KeyCode.UpArrow)) currentDirection = DraggedDirection.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) currentDirection = DraggedDirection.Down;

        if (Input.touchCount > 0)
        {
            startSwipePos = Input.GetTouch(0).position;
        }

            switch (currentDirection)
        {
               
            case DraggedDirection.Up:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, speed* Time.deltaTime));
                break;
            case DraggedDirection.Down:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, -speed * Time.deltaTime));
                break;
            case DraggedDirection.Left:
                rigidbody.velocity = (new Vector3(-speed * Time.deltaTime, rigidbody.velocity.y, 0.0f));
                break;
            case DraggedDirection.Right:
                rigidbody.velocity = (new Vector3(speed * Time.deltaTime, rigidbody.velocity.y, 0.0f));
                break;
        }

        //Segment updating

        for (int i = 0; i < otherSegments.Count; i++)
        {
            otherSegments[i].transform.position = prevPositions[prevPositions.Count - (1 + i)];
        }

        RecordPositions();
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
        segments++;
        otherSegments.Add(Instantiate(segmentReference));

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


    }

    
    private void OnMouseDown()
    {
        startSwipePos = Input.mousePosition;
    }

    
    private void OnMouseUp()
    {
        endSwipePos = Input.mousePosition;
        
        Vector3 dragVectorDirection = (endSwipePos -  startSwipePos).normalized;
        Debug.Log("norm + " + dragVectorDirection);

        //Swap directions here. If it's a new direction then align to the grid!
        oldDirection = currentDirection;

        Debug.Log(oldDirection);
        currentDirection = GetDragDirection(dragVectorDirection);

        if (currentDirection != oldDirection) transform.position = AlignToGrid(transform.position);
    }

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
    
}
