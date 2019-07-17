using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSoundVolume : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ChangeVolume(); });

        slider.value = SoundManager.GetVolume();
    }

    // Update is called once per frame
    void ChangeVolume()
    {
        SoundManager.SetVolume(slider.value);
    }
}
