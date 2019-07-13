using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Call this to play a sound
public class SoundManager : MonoBehaviour
{
    //List all sounds in here
    public AudioClip[] soundAssets;

    public static AudioClip[] soundList;

    //Static copy of array for easy access
    public static AudioSource[] currentSounds;

    //List of all sounds for each id
    public enum Sounds
    {
        GROW,
        EXPLODE,
        DIE,
        POWERUP,
        COIN,
        FOOD_PICKUP,
        OPEN_GATE,
        HURT
    }

    // Start is called before the first frame update
    void Start()
    {


        currentSounds = GetComponents<AudioSource>();
        soundList = soundAssets;
    }


    //Call this whenever a sound needs to be played, and provide the right enum
    public static void PlaySound(Sounds soundToPlay, float pitch = 1.0f, int sourceIndex = 0)
    {
        //Play the sound on the second audio source if the first is used up.
        if (currentSounds[sourceIndex].isPlaying && sourceIndex == 0)
        {
            PlaySound(soundToPlay, pitch, 1);
            return;
        }

        currentSounds[sourceIndex].pitch = pitch;
        currentSounds[sourceIndex].clip = soundList[(int)soundToPlay];
        currentSounds[sourceIndex].Play();
    }
    

}
