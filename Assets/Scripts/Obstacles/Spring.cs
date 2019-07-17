using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    //Bounce impulse
    public float bounce = 10.0f;

    //Lerp vars
    Vector3 initialScale;
    Vector3 bounceScale;
    float t;
    bool increasing;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        bounceScale = transform.localScale * 1.5f;
        increasing = true;
    }

    //Lerp to add some visual flair to the bounce.
    void Update()
    {
        if (t < 1.0f && increasing)
        {
            t += Time.deltaTime * 4.0f;

            transform.localScale = Vector3.Lerp(initialScale, bounceScale, t);
        }
        else if (t > 1.0f) increasing = false;

        if (t > 0.0f && !increasing)
        {
            t -= Time.deltaTime * 2.0f;

            transform.localScale = Vector3.Lerp(initialScale, bounceScale, t);
        }
    }

    //Make the player bounce.
    private void OnTriggerEnter(Collider other)
    {
        if (increasing) return;

        Rigidbody rigidbody = other.GetComponent<Rigidbody>();

        if (rigidbody && other.tag != "SolidCheck" && other.name[0] != 'F')
        {
            rigidbody.velocity = (new Vector3(rigidbody.velocity.x, bounce, rigidbody.velocity.z));

            t = 0.0f;
            increasing = true;
            SoundManager.PlaySound(SoundManager.Sounds.BOUNCE);
        }
    }
}
