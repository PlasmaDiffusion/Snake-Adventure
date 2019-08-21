
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Make the background move.
public class MoveBackground : MonoBehaviour
{
    [SerializeField] Vector2 speed;
    Vector2 offset;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(0.0f, 0.0f);
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Make treadmill actually move the texture
        offset += (speed * Time.deltaTime);
        //Set actual texture to move with the offset
        image.materialForRendering.SetTextureOffset("_MainTex", offset);

    }
}
