using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpikeTrap : MonoBehaviour
{
    Rigidbody rigidbody;
    bool hideWarningText;

    //Alternate between these two velocities.
    public Vector3 startingVel;
    bool opposite;
    Vector3 oppositeVel;

    //"!" Text that shows when a nearby trap is offscreen.
    Text warningText;
    Camera cam;

    //Prevent constant collisions
    float collisionDebounce;

    //Vel to save for pausing purposes
    Vector3 currentVel;
    bool wasPaused;

    // Start is called before the first frame update
    void Start()
    {
        hideWarningText = false;
        warningText = Instantiate(GameObject.Find("WarningText"), GameObject.Find("Canvas").transform).GetComponent<Text>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        //If a child then use the parent rigid body
        if (startingVel.x == 0.0f && startingVel.z == 0.0f)
        {
            rigidbody = transform.parent.GetComponent<Rigidbody>();
            hideWarningText = true; //Children don't have their own warning text
        }
        else
        { 
        rigidbody = GetComponent<Rigidbody>();
        }

        opposite = true;
        collisionDebounce = 0.0f;

        rigidbody.velocity = startingVel;
        oppositeVel = -startingVel;

        currentVel = -startingVel;
        wasPaused = false;
    }

    //Pausing code happens here
    private void Update()
    {
        if (GlobalStats.paused)
        {
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            wasPaused = true;
            return;
        }
        else if (wasPaused && !hideWarningText)
        {
            rigidbody.velocity = currentVel;
            wasPaused = false;
        }

    }

    // Movement updating happens here
    void FixedUpdate()
    {
        if (GlobalStats.paused) return;
        
        if (collisionDebounce > 0.0f) collisionDebounce -= Time.deltaTime;

        //Make sure a spike doesn't randomly get stuck.
        if (rigidbody.velocity.x == 0.0f && rigidbody.velocity.z == 0.0f && rigidbody.velocity.y == 0.0f)
        {
            //Debug.Log("Spike not moving for some reason");
            BounceOffWall();
        }


        if (!hideWarningText) MoveWarningText();
    }

    void MoveWarningText()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        Vector3 newRectTransform = new Vector3(0.0f, 0.0f, 0.0f);

        bool visible = true;

        if (screenPos.x > cam.pixelWidth) { newRectTransform += new Vector3(cam.pixelWidth - 32.0f, screenPos.y, 0.0f); visible = true;}
        else if (screenPos.y > cam.pixelHeight) {newRectTransform += new Vector3(screenPos.x, cam.pixelHeight - 32.0f, 0.0f); ; visible = true;}
        if (screenPos.x < 0.0f) { newRectTransform += new Vector3(0.0f + 32.0f, screenPos.y, 0.0f); visible = true; }
        else if (screenPos.y < 0.0f) { newRectTransform += new Vector3(screenPos.x, 0.0f - 32.0f, 0.0f); visible = true; }

        if (visible)
        {
            warningText.rectTransform.position = newRectTransform;

            
        }
        else
        {
            warningText.rectTransform.position = new Vector3(1000.0f, 0.0f);
        }
    }
    
    void KillPlayer(DeathCheck deathCheck)
    {
        deathCheck.Die();

        rigidbody.velocity = new Vector3(0.0f, 10.0f, 0.0f);

        hideWarningText = true;
    }


    void BounceOffWall()
    {
        if (collisionDebounce > 0.0f) return;

        //Reverse the velocity, also store the new velocity for pausing purposes
        if (!opposite) { rigidbody.velocity = oppositeVel; currentVel = oppositeVel; }
        else { rigidbody.velocity = startingVel; currentVel = startingVel; }
        

        //Don't let collision happen constantly
        collisionDebounce = 0.5f;

        opposite = !opposite;
    }

    //Collision with square (parent object)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            KillPlayer(collision.gameObject.GetComponent<DeathCheck>());
        }
        else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Segment")
        {
            BounceOffWall();
        }
    }

    //Collision with spikes
    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            KillPlayer(other.GetComponent<DeathCheck>());
        }
        else if (other.tag == "Wall" || other.tag == "Segment")
        {
            BounceOffWall();
        }
    }

    private void OnDestroy()
    {
        Destroy(warningText);
    }
}
