using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A death
public class DeathParticle : MonoBehaviour
{
    ParticleSystem ps;
    bool original;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        //The original object will not destroy itself.
        if (name != "DeathParticleEmitter")
        {
            original = false;
            ps.Play();
        }
        else
        {
            original = true;
            ps.Stop();
        }
    }

    // Destroy emitter once its emitted everything through one cycle.
    void Update()
    {
        if (!original && ps.time >= 1.0f) Destroy(gameObject);
    }
}
