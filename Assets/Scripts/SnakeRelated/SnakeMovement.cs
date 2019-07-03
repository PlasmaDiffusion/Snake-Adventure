using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    //Movement vars

    public float speed;
    protected DraggedDirection currentDirection;
    protected DraggedDirection oldDirection;
    protected Rigidbody rigidbody;

    //Force direction variables for treadmills
    DraggedDirection forcedDirection;
    bool forcingDirection;

    //Other snake segments
    protected int segments;
    protected float positionRecordTime;
    protected float timeBetweenPositions;
    public List<Vector3> prevPositions;
    [Header("Snake References to Copy")]
    public GameObject segmentReference;
    public GameObject segmentLinkReference;
    protected List<GameObject> otherSegments;


    //Var for lerping movement/rotating
    protected Quaternion targetRotation;
    protected float timeRotating;
    Vector3 targetPosition;
    Vector3 preTargetPosition;
    protected float positionT;
    protected float moveRate;

    //Solid wall collision check boxes
    public GameObject[] solidCheckObjects;
    CheckIfSolid[] solidCheck;
    

    protected enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    // Start is called before the first frame update
    protected void InitSnakeMovement()
    {
        transform.position = AlignToGrid(transform.position);
        targetPosition = transform.position;
        positionT = 1.0f;
        moveRate = 8.0f;

        forcingDirection = false;

        //Set collision boxes
        solidCheck = new CheckIfSolid[4];
        for (int i = 0; i < 4; i++)
        {
            solidCheck[i] = solidCheckObjects[i].GetComponent<CheckIfSolid>();
            solidCheck[i].owner = gameObject;
        }
    }

    //Add a snake segment. Capped at 100.
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

    //Record positions for the snake segments to follow.
    void RecordPositions()
    {
        //Record positions every now and then
        positionRecordTime += Time.deltaTime;

        //if (positionRecordTime > timeBetweenPositions)
        {
            positionRecordTime = 0.0f;

            //Record position for the snake segments
            prevPositions.Add(AlignToGrid(transform.position));

            //Remove older uneeded positions
            if (prevPositions.Count > segments + 1)
            {
                prevPositions.RemoveAt(0);
            }

            //Now move!
            //if (positionT >= 1.0f)
            MoveAndSnapToGrid();
        }
    }

    protected void MoveAndSnapToGrid()
    {
        //Reset targetLerp position when needed
        if (positionT >= 1.0f) preTargetPosition = transform.position;


        //Force a direction?
        if (forcingDirection)
        {
            currentDirection = forcedDirection;
        }

        //Every update check the direction and update the velocity. Also rotate to that direction.
        switch (currentDirection)
        {

            case DraggedDirection.Up:

                if (positionT >= 1.0f) { positionT = 0.0f; targetPosition = transform.position + (new Vector3(0.0f, 0.0f, 1.0f));}
                targetRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case DraggedDirection.Down:

                if (positionT >= 1.0f) {positionT = 0.0f; targetPosition = transform.position + (new Vector3(0.0f, 0.0f, -1.0f)); }
                targetRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                timeRotating = 0.0f;
                if (positionT >= 1.0f) positionT = 0.0f;
                break;
            case DraggedDirection.Left:
                if (positionT >= 1.0f) { positionT = 0.0f; targetPosition = transform.position + (new Vector3(-1.0f, 0.0f, 0.0f)); }
                targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case DraggedDirection.Right:
                if (positionT >= 1.0f) { positionT = 0.0f; targetPosition = transform.position + (new Vector3(1.0f, 0.0f, 0.0f)); }
                targetRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                timeRotating = 0.0f;
                if (positionT >= 1.0f) positionT = 0.0f;
                break;
        }


        //transform.position = AlignToGrid(transform.position);
    }

    //Snap in place to grid so its like traditional snake
    public Vector3 AlignToGrid(Vector3 oldPos)
    {
        //Change this to go back to snapping
        //return oldPos;

        return new Vector3(Mathf.Round(oldPos.x), transform.position.y, Mathf.Round(oldPos.z));
        //return new Vector3(Mathf.Floor(oldPos.x), transform.position.y, Mathf.Floor(oldPos.z));
    }

    protected void LerpToDirection()
    {
        if (timeRotating < 1.0f)
        {
            timeRotating += Time.deltaTime * 12.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeRotating);
            
        }
        if (positionT < 1.0f && !solidCheck[(int)currentDirection].GetIsSolid())
        {

            positionT += Time.deltaTime * moveRate;
            if (positionT > 1.0f) positionT = 1.0f;

            transform.position = Vector3.Lerp(new Vector3(preTargetPosition.x, transform.position.y, preTargetPosition.z), new Vector3(targetPosition.x, transform.position.y, targetPosition.z), positionT);
            
            //Snap when done lerping to the tile
            if (positionT >= 1.0f)
            {
                transform.position = AlignToGrid(transform.position);
                RecordPositions();
            }
        }

    }

    //Force movement from objects like treadmills
    public void ForceDirection(Vector3 direction)
    {
        DraggedDirection newDirection = DraggedDirection.Up;

        Debug.Log("Given the following direction vector: " + direction.ToString());

        if (direction.x > 1.0f) newDirection = DraggedDirection.Right;
        if (direction.x < -1.0f) newDirection = DraggedDirection.Left;
        if (direction.z > 1.0f) newDirection = DraggedDirection.Up;
        if (direction.z < -1.0f) newDirection = DraggedDirection.Down;

        forcedDirection = newDirection;
        forcingDirection = true;
    }

    public void StopForcingDirection()
    {
        forcingDirection = false;
    }

    //For when the character needs to teleport somewhere else
    protected void ResetLerpPositions()
    {
        targetPosition = transform.position;
        preTargetPosition = transform.position;
    }
}
