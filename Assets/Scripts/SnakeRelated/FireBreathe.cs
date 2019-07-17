using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathe : MonoBehaviour
{
    [Header("Variables for powerup duration.")]
    public float activeTime;
    public float maxActiveTime;

    [Header("Variables for spawning projectile.")]
    public GameObject projectileReference;
    public float fireWaitTime;
    float fireTime;
   //ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        activeTime = 0.0f;
        //particleSystem = GetComponent<ParticleSystem>();
        //particleSystem.Stop();
        fireTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStats.paused) return;

        //Powerup duration updating
        if (activeTime > 0.0f && !Snake.boosting)
        {
            activeTime -= Time.deltaTime;

            if (activeTime <= 0.0f)
            {
                activeTime = 0.0f;
                //particleSystem.Stop();
            }

        }

        //Fire projectile updating
        if (activeTime > 0.0f)
        {
            fireTime -= Time.deltaTime;
            if (fireTime <= 0.0f)
            {
                fireTime = fireWaitTime;
                //Spawn in three projectiles
                SoundManager.PlaySound(SoundManager.Sounds.FIRE);
                Instantiate(projectileReference, transform.position, transform.rotation);
                Instantiate(projectileReference, transform.position, transform.rotation * Quaternion.Euler(0, -45, 0));
                Instantiate(projectileReference, transform.position, transform.rotation * Quaternion.Euler(0, 45, 0));
            }
        }
    }
    
    public void ActivateFire()
    {
        //particleSystem.Play();
        activeTime = maxActiveTime;
        fireTime = fireWaitTime;
    }

}
