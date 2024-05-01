using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    GameManager gameManager;
    AudioManager audioManager;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float sphereRadius = 1f;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        muzzleFlash.Stop();
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
        audioManager.PlayGrenadePinClip();
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3.5f);
        audioManager.PlayGrenadeExplosionClip();
        FireRaycastIntoScene();

        muzzleFlash.enableEmission = true;
        muzzleFlash.transform.position = raycastOrigin.position;
        muzzleFlash.Play();

        yield return new WaitForSeconds(0.2f);
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
