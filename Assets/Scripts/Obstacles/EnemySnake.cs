using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : MonoBehaviour
{
    //Movement vars
    public float speed;
    public int startingDirection;
    bool canMove;
    SnakeDirection currentDirection;
    SnakeDirection oldDirection;
    Rigidbody rigidbody;

    //Change direction markers
    public GameObject[] directionMarkers;



    //Var for rotating
    Quaternion targetRotation;
    float timeRotating;

    //Other snake segments
    public int segments;
    float positionRecordTime;
    float timeBetweenPositions;
    List<Vector3> prevPositions;
    GameObject segmentReference;
    GameObject segmentLinkReference;
    List<GameObject> otherSegments;

    private enum SnakeDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    // Pre create all other segments
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        canMove = true;

        currentDirection = (SnakeDirection)startingDirection;

        otherSegments = new List<GameObject>();
        prevPositions = new List<Vector3>();

        timeBetweenPositions = 0.275f;
        positionRecordTime = 0.0f;

        segmentReference = GameObject.Find("EnemySegment");
        segmentLinkReference = GameObject.Find("EnemySegmentLink");
        

        for (int i = 0; i < segments; i++) AddSegment();

    }

    // Movement update
    void FixedUpdate()
    {

        if (!canMove || GlobalStats.paused)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            if (GlobalStats.paused) rigidbody.useGravity = false;
            return;
        }

        rigidbody.useGravity = true;


        switch (currentDirection)
        {

            case SnakeDirection.Up:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, speed * Time.deltaTime));
                targetRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case SnakeDirection.Down:
                rigidbody.velocity = (new Vector3(0.0f, rigidbody.velocity.y, -speed * Time.deltaTime));
                targetRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case SnakeDirection.Left:
                rigidbody.velocity = (new Vector3(-speed * Time.deltaTime, rigidbody.velocity.y, 0.0f));
                targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                timeRotating = 0.0f;
                break;
            case SnakeDirection.Right:
                rigidbody.velocity = (new Vector3(speed * Time.deltaTime, rigidbody.velocity.y, 0.0f));
                targetRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                timeRotating = 0.0f;
                break;
        }


    }

    //Visual and segment updating
    void Update()
    {
        if (!canMove || GlobalStats.paused)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }

        LerpToDirection();

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


            prevPositions.Add(transform.position);

            //Remove older uneeded positions
            if (prevPositions.Count > segments + 1)
            {
                prevPositions.RemoveAt(0);
            }
        }
    }


    void LerpToDirection()
    {
        if (timeRotating < 1.0f)
        {
            timeRotating += Time.deltaTime * 4.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeRotating);

        }
    }

    void AddSegment()
    {
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

        if (otherSegments.Count > 1) //For actual segments
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

    private void OnTriggerEnter(Collider other)
    {
        //Change directions if the marker is touched.
        if (other.tag == "DirectionChanger")
        {
            for (int i = 0; i < 4; i++)
            {
                if (other.gameObject.transform.position == directionMarkers[i].transform.position)
                {
                    switch (i)
                    {
                        default:
                            break;
                        case 0://Left
                            currentDirection = SnakeDirection.Left;
                            break;
                        case 1://Down
                            currentDirection = SnakeDirection.Down;
                            break;
                        case 2://Right
                            currentDirection = SnakeDirection.Right;
                            break;
                        case 3://Up
                            currentDirection = SnakeDirection.Up;
                            break;
                    }
                }
            }
        }
        else if (other.tag == "Segment" && other.name[0] == 'S') //Get blocked by the player if touched by one  of their segments.
        {
            canMove = false;
        }
    }

    //Get unblocked by the player when done touching a segment.
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Segment" && other.name[0] == 'S') //The 'S' stops this snake from blocking itself
        {
            canMove = true;
        }
    }

    //Hurt the player upon colliding with the head
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {

            //Destroy when boosting
            if (collision.gameObject.GetComponent<Snake>().GetBoosting())
            {
                //Spawn death particles
                GameObject emitter = GameObject.Find("DeathParticleEmitter");

                if (emitter) Instantiate(emitter, transform.position, emitter.transform.rotation);
                

                Destroy(gameObject);
            }

            collision.gameObject.GetComponent<DeathCheck>().Die();


        }
    }

    //Remove child segments when dead
    private void OnDestroy()
    {
        for (int i = otherSegments.Count-1; i >= 0; i--)
        {
            //Insert death particle here
            Destroy(otherSegments[i]);
            otherSegments.RemoveAt(i);
        }
    }

}

