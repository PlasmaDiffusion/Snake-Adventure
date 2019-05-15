using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    bool on;

    Vector3 normalScale;
    Vector3 pressedScale;

    //Prevent switch from accidently being pressed rapidly.
    float downTime;

    // Start is called before the first frame update
    void Start()
    {
        on = false;

        downTime = 0.0f;

        normalScale = transform.localScale;
        pressedScale = new Vector3(1.0f, 0.25f, 1.0f);
    }

    private void Update()
    {
        if (downTime > 0.0f) downTime-= Time.deltaTime;
    }

    //Toggle all the switch blocks
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && downTime <= 0.0f)
        {


        on = !on;
        downTime = 0.3f;

        //Move button down/up
        if (on) transform.localScale = pressedScale;
        else transform.localScale = normalScale;

        SwitchBlock[] switchBlocks= transform.parent.GetComponentsInChildren<SwitchBlock>();

        if (switchBlocks.Length > 0)
            {
                foreach (var block in switchBlocks)
                {
                    block.SwitchToggle();
                }

            }
        }

        
    }
}
