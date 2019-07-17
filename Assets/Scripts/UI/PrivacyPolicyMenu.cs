using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrivacyPolicyMenu : MonoBehaviour
{
    private void Start()
    {
        if (GlobalStats.readPrivacyPolicy)
            gameObject.SetActive(false);
    }

    public void Accept()
    {
        GlobalStats.readPrivacyPolicy = true;
        GlobalStats.Save();
        gameObject.SetActive(false);
    }

    public void OpenLink()
    {
        Application.OpenURL("https://unity3d.com/legal/privacy-policy");
    }
}
