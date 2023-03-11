using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public int health = 3;
    public float moveSpeed;
    public SpaceBoyController player;
    
    //public SphereCollider awarenessSphere;
    public float radius;
    public Animator anim;
    public bool foundPlayer;
    public float attackRate;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }

    public void AttackPlayer()
    {
        player.health--;
        //play attack anim here
    }
}
