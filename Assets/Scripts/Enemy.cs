using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Enemy : MonoBehaviour
{
     private GameManager gameManager;
    private int health = 100;
    private int scoreGain = 100;
    private WeaponType weaponType;
    public ParticleSystem Explosion;
    public AudioClip AudioClipChicken;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void TakeDamage(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                health -= 35;
                break;
            case WeaponType.Rifle:
                health -= 70;
                break;
            case WeaponType.Shotgun:
                health -= 100;
                break;
            case WeaponType.Grenade:
                health -= 250;
                break;
            default:
                break;
        }

        if (health <= 0) {
            Die();
        }
    }

    private void Die()
    {
        gameManager.DecreaseChicken();
        gameManager.IncreaseScore(scoreGain);
        Destroy(gameObject);
        Instantiate(Explosion,transform.position,transform.rotation);
        AudioSource.PlayClipAtPoint(AudioClipChicken, gameObject.transform.position);
    }
    /*
    void OnCollisionEnter (Collision newCollision) {// pri kolízii s inými objektami// ak sa zrazil s projektilom
        if (newCollision.gameObject.tag == "Projectile") {
            Debug.Log("FUNGUJE");
            //TakeDamage(weaponType);
        }
    }*/

}

public enum WeaponType
{
    Pistol,
    Shotgun,
    Rifle,
    Grenade
}

