using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
             Destroy(gameObject);
             gameManager.DecreaseHealth(50);
        }
    }
}
