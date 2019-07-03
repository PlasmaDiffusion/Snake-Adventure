using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathe : MonoBehaviour
{
    public float activeTime;
    public float maxActiveTime;

    public GameObject itemSpawnerReference;

   ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        activeTime = 0.0f;
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStats.paused) return;

        if (activeTime > 0.0f && !Snake.boosting)
        {
            activeTime -= Time.deltaTime;

            if (activeTime <= 0.0f)
            {
                activeTime = 0.0f;
                particleSystem.Stop();
            }
        }
    }
    
    public void ActivateFire()
    {
        particleSystem.Play();
        activeTime = maxActiveTime;
    }

    //Burn enemies or anything that's tagged as burnable!
    private void OnTriggerEnter(Collider other)
    {
        if (activeTime <= 0.0f) return;
        
        if (other.tag == "Segment" && other.name[0] == 'E') //Destroy all of a snake
        {
            SnakeSegment enemySegment = other.GetComponent<SnakeSegment>();

            GlobalStats.AddScore(10, other.transform.position);


            //Spawn death particles
            GameObject emitter = GameObject.Find("DeathParticleEmitter");

            if (emitter) Instantiate(emitter, other.transform.position, emitter.transform.rotation);


            Destroy(enemySegment.snakeOwner);
            
        }
        else if (other.tag == "Burnable" || other.tag == "BurnableNotSolid")
        {
            //Small chance for iceblocks to drop items
            if (other.name[0] == 'I')
            {
                if (Random.Range(0, 3) == 0)
                {
                    GameObject newCollectableSpawner = Instantiate(itemSpawnerReference);
                    newCollectableSpawner.transform.position = other.transform.position;
                    Debug.Log("Thing spawned");
                }
            }

            //Bonus points
            GlobalStats.AddScore(30, other.transform.position);

            //Spawn death particles
            GameObject emitter = GameObject.Find("DeathParticleEmitter");

            if (emitter) Instantiate(emitter, other.transform.position, emitter.transform.rotation);


            //Burn the object! (Spawn a special particle here?)
            Destroy(other.gameObject);


        }
    }
}
