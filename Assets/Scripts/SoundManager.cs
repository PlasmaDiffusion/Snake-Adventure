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

    static float soundVolume;

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
        HURT,
        BOUNCE,
        TRAP_COLLIDE,
        ENTER_GATE,
        SWITCH_ON,
        SWITCH_OFF,
        OBJECTIVE_PROGRESSED,
        OBJECTIVE_ACHIEVED,
        HISCORE,
        FIRE,
        BOOST
    }

    // Start is called before the first frame update
    void Start()
    {
        soundVolume = GlobalStats.initialSoundVolume;

        currentSounds = GetComponents<AudioSource>();
        soundList = soundAssets;
    }


    //Call this whenever a sound needs to be played, and provide the right enum. This uses two audio sources to mimic limited sound channels.
    public static void PlaySound(Sounds soundToPlay, float pitch = 1.0f, int sourceIndex = 0)
    {
        //Use -1 to force the sound
        if (sourceIndex == -1)
        {
            currentSounds[0].Stop();
            sourceIndex = 0;
        }

        //Play the sound on the second audio source if the first is used up.
        if (currentSounds[sourceIndex].isPlaying && sourceIndex == 0)
        {
            PlaySound(soundToPlay, pitch, 1);
            return;
        }


        currentSounds[sourceIndex].volume = soundVolume;
        currentSounds[sourceIndex].pitch = pitch;
        currentSounds[sourceIndex].clip = soundList[(int)soundToPlay];
        currentSounds[sourceIndex].Play();
    }
    
    public static void SetVolume(float amount)
    {
        soundVolume = amount;
        GlobalStats.initialSoundVolume = amount;
    }
    public static float GetVolume()
    {
        return soundVolume;
    }
}
