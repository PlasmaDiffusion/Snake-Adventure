using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{

    public GameObject itemSpawnerReference;
    float life;

    Rigidbody rb;
    Vector3 targetVelocity;

    public float speed;

    bool dummy;

    // Start is called before the first frame update
    void Start()
    {
        dummy = false;
        if (name == "FireProjectileReference") dummy = true;

        rb = GetComponent<Rigidbody>();
        life = 5.0f;

        targetVelocity = (transform.forward.normalized) * speed;
        
        targetVelocity = Quaternion.Euler(0, -90, 0) * targetVelocity;

        if (!dummy)rb.velocity = targetVelocity;

    }

    // Update is called once per frame
    void Update()
    {
        if (dummy) return;

        //Pause game and stop this from moving
        if (GlobalStats.paused)
        {
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }
        else rb.velocity = targetVelocity;

        //Die eventually overtime
        life -= Time.deltaTime;

        if (life <= 0.0f) Destroy(gameObject);
    }


    //Burn enemies or anything that's tagged as burnable!
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Segment" && other.name[0] == 'E') //Destroy all of a snake
        {
            SnakeSegment enemySegment = other.GetComponent<SnakeSegment>();

            GlobalStats.AddScore(10, other.transform.position);


            //Spawn death particles
            GameObject emitter = GameObject.Find("DeathParticleEmitter");

            if (emitter) Instantiate(emitter, other.transform.position, emitter.transform.rotation);

            CoinObjective.CheckForObjective((int)CoinObjective.Objective.DESTROY_WITH_FIRE);

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

            CoinObjective.CheckForObjective((int)CoinObjective.Objective.DESTROY_WITH_FIRE);

            //Burn the object! (Spawn a special particle here?)
            Destroy(other.gameObject);


        }
    }
}
