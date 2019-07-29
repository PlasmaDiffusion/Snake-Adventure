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

        //Prevent any crash if audio source is out of index
        if (sourceIndex >= currentSounds.Length) return;

        //Use -1 to force the sound
        if (sourceIndex == -1)
        {
            if (currentSounds[0])
            currentSounds[0].Stop();
            sourceIndex = 0;
        }

        //Prevent any crash if audio source doesn't exist
        if (!currentSounds[sourceIndex]) return;

        //Play the sound on the second/third audio source if the first is used up.
        if (currentSounds[sourceIndex].isPlaying && sourceIndex < 2)
        {
            PlaySound(soundToPlay, pitch, sourceIndex+1);
            return;
        }

        //Normal play sound code here! ---------------------------------------------------------
        if (currentSounds[sourceIndex])
        {
        currentSounds[sourceIndex].volume = soundVolume;
        currentSounds[sourceIndex].pitch = pitch;
        currentSounds[sourceIndex].clip = soundList[(int)soundToPlay];
        currentSounds[sourceIndex].Play();
        }
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
