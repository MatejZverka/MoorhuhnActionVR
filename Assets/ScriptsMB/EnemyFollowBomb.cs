using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowBomb : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent enemy;
    public Transform player;
    public ParticleSystem Explosion;

    public GameManager gameManager;

    void Start()
    {
    }

    
    void Update()
    {
        enemy.SetDestination(player.position);
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(enemy);
            Instantiate(Explosion,transform.position,transform.rotation);
            gameManager.DecreaseHealth(100);
        }
    }
}
