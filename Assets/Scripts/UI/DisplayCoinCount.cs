using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Just a plain way to display coins for shop menus.
public class DisplayCoinCount : MonoBehaviour
{

    // Update is called once per frame
    void OnEnable()
    {
        GetComponent<Text>().text = GlobalStats.coins.ToString();
    }
}
