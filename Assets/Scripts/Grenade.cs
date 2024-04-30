using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float sphereRadius = 1f;
    [SerializeField] AudioClip pinSound;
    [SerializeField] AudioClip explodeSound;
    private AudioSource audioSource;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        grabInteractable.activated.AddListener(TriggerPulled);
    }

    private void OnDisable()
    {
        grabInteractable.activated.RemoveListener(TriggerPulled);
    }

    public void TriggerPulled(ActivateEventArgs arg0)
    {
        audioSource.clip = pinSound;
        audioSource.Play();
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        audioSource.clip = explodeSound;
        audioSource.Play();
        FireRaycastIntoScene();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit[] hits = Physics.SphereCastAll(raycastOrigin.position, sphereRadius, raycastOrigin.forward, Mathf.Infinity, targetLayer);

        foreach (RaycastHit hit in hits)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(WeaponType.Grenade);
                Debug.Log("Enemy hit -250 HP");
            }
        }
    }

}
