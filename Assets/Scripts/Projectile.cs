using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float cas = 3f;

    private void Start()
    {
        Destroy(gameObject, cas);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}