using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    GameManager gameManager;
    private int cycler = 0;
    private int currentAmmo;
    [SerializeField] InputActionReference reloadActionReference;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] AudioClip gunshotSound;
    [SerializeField] AudioClip emptySound;
    [SerializeField] AudioClip reloadSound;
    private AudioSource audioSource;

    [SerializeField] int magazineCapacity = 19;
    private int remainingBullets;

    [SerializeField] float reloadTime = 2.5f;
    private bool isReloading;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Pistol);
        remainingBullets = magazineCapacity;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
        muzzleFlash.Stop();
    }

    private void OnEnable()
    {
        grabInteractable.activated.AddListener(TriggerPulled);
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grabInteractable.activated.RemoveListener(TriggerPulled);
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    public void TriggerPulled(ActivateEventArgs arg0)
    {
        cycler++;
        if (!isReloading && (remainingBullets > 0) && (cycler >= 2)) { 
            cycler = 0;
            arg0.interactor.GetComponent<XRBaseController>().SendHapticImpulse(.5f, .25f);
            audioSource.clip = gunshotSound;
            audioSource.Play();
            remainingBullets--;
            Debug.Log("Magazine: " + remainingBullets + "   Reserve: " + currentAmmo);
            FireRaycastIntoScene();

            muzzleFlash.enableEmission = true;
            muzzleFlash.transform.position = raycastOrigin.position;
            muzzleFlash.Play();

        } else if (!isReloading && (remainingBullets <= 0) && (cycler >= 2)) {
            Debug.Log("Magazine: " + remainingBullets + "   Reserve: " + currentAmmo);
            cycler = 0;
            audioSource.clip = emptySound;
            audioSource.Play();
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactor;
        if (interactor != null)
        {
            GameObject grabbedObject = interactor.selectTarget.gameObject;
            Debug.Log("Currently grabbed object: " + grabbedObject.name);
            if (grabbedObject.name == "Pistol")
            {
                reloadActionReference.action.performed += OnPrimaryButtonPressed;
            }
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        //Debug.Log("Object released. Cancelling code execution.");
        reloadActionReference.action.performed -= OnPrimaryButtonPressed;
    }

    public void OnPrimaryButtonPressed(InputAction.CallbackContext reload)
    {
        UpdateCurrentAmmo();
        //Debug.Log("Congrats, you wasted 6 hours of your life on this");
        if (!isReloading && (remainingBullets < magazineCapacity) && (currentAmmo > 0))
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        //Debug.Log("Reloading...");
        isReloading = true;
        audioSource.clip = reloadSound;
        audioSource.Play();
        yield return new WaitForSeconds(reloadTime);

        gameManager.IncreaseAmmo(AmmoType.Pistol, remainingBullets);
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Pistol);
        if (currentAmmo >= magazineCapacity) {
            remainingBullets = magazineCapacity;
            gameManager.DecreaseAmmo(AmmoType.Pistol, magazineCapacity);
        } else if ((currentAmmo < magazineCapacity) && (currentAmmo > 0)) {
            remainingBullets = currentAmmo;
            gameManager.DecreaseAmmo(AmmoType.Pistol, currentAmmo);
        }
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Pistol);

        isReloading = false;
        //Debug.Log("Reloaded!");
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.TakeDamage(WeaponType.Pistol);
            Debug.Log("Enemy hit -35 HP");
        }
    }

    public void UpdateCurrentAmmo()
    {
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Pistol);
    }

}
