using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowBomb : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent enemy;
    private Transform player;
    public ParticleSystem Explosion;

    public GameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    
    void Update()
    {
        enemy.SetDestination(player.position);
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Instantiate(Explosion,transform.position,transform.rotation);

            
            gameManager.DecreaseHealth(100);
        }
    }
}
