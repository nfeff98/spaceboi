using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehavior : MonoBehaviour
{
    private int maxHealth;
    private float maxSpeed;
    public int health = 3;
    public enum AnimalType { Golem, Penguin }
    public AnimalType type;
    public SpaceBoyController player;
    public NavMeshAgent agent;
    private float runSpeed;
    //public SphereCollider awarenessSphere;
    public Transform target;
    public Animator anim;
    public bool foundPlayer;
    public float attackRate;
    public float attackRange;
    public float damage;
    public bool wandering;
    public bool chasing;
    public float wanderRadius = 5f;
    private bool revealed;
    public bool hostile;
    public float attackCooldown;
    private bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        if (type == AnimalType.Penguin)
            StartWander();
        maxHealth = health;
        runSpeed = agent.speed + 1;
        maxSpeed = agent.speed;
    }

    public void StartWander()
    {
        wandering = true;
        StartCoroutine(Wander());
    }

    public IEnumerator Wander()
    {
        while (wandering)
        {
            if (agent.isOnNavMesh)
            {
                float delay = Random.Range(5f, 8f);
                Vector3 newDirection = Random.insideUnitSphere;
                agent.destination = this.transform.position + newDirection * Random.Range(0.5f, wanderRadius);
                yield return new WaitForSeconds(delay);
            }

        }
    }

    public IEnumerator ChasePlayer()
    {
        chasing = true;
        while (chasing)
        {
            if (hostile && canAttack)
            {
                if (Vector3.Distance(this.transform.position, player.transform.position) < attackRange)
                {
                    agent.destination = this.transform.position;
                    AttackPlayer();
                }
            }

            if (Vector3.Distance(this.transform.position, player.transform.position) > 2* attackRange)
            {
                chasing = false;
                if (type == AnimalType.Golem)
                {
                    anim.SetBool("reveal", false);
                    anim.Play("hidden");
                    this.GetComponent<Rigidbody>().isKinematic = true;
                    
                    revealed = false;
                }
            } else
            {
                agent.destination = player.transform.position;

            }
            yield return new WaitForEndOfFrame();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        if(agent.velocity.magnitude > 1f)
        {
            anim.SetBool("walking", true);
        } else
        {
            anim.SetBool("walking", false);
        }
        if (agent.velocity.magnitude > 0)
            agent.transform.forward = agent.velocity.normalized;

       
        
    }

    public void Hit()
    {
        health--;
        if (type == AnimalType.Penguin)
        {
            foreach (ParticleSystem ps in this.gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                ps.transform.localScale = Vector3.one * Random.Range(0.3f, 1f);
                ps.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == AnimalType.Golem)
        {
            if (other.tag == "Player")
            {
                if (!revealed) {
                    anim.SetBool("reveal", true);
                    revealed = true;
                    
                    //chase player 
                    player = other.GetComponent<SpaceBoyController>();
                    StartCoroutine(ChasePlayer());
                    StartCoroutine(AttackCooldown());
                    this.GetComponent<Rigidbody>().isKinematic = false;
                   }
            }
        }

        if (other.GetComponentInParent<SimpleCarController>() != null)
        {
            SimpleCarController vehicle = other.GetComponentInParent<SimpleCarController>();
            if (vehicle.miningBits.Contains(other.gameObject) && vehicle.toolsOn)
                {
                    StartCoroutine(VehicleBladeCoroutine());
                } 
            // if vehicle.wheels.contains(other.gameobject) //if run over by vehicle
        }

       
    }

    public IEnumerator VehicleBladeCoroutine()
    {
        while (true)
        {
            this.Hit();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForEndOfFrame();
        agent.speed = maxSpeed;
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        
    }

    public void AttackPlayer()
    {
        //player.health--;
        agent.speed = 0;
        anim.SetBool("attacking", true);
        canAttack = false;
        StartCoroutine(AttackCooldown());
        //play attack anim here
    }
}
