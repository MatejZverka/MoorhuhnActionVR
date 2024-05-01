using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Shotgun : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    GameManager gameManager;
    AudioManager audioManager;
    [SerializeField] InputActionReference reloadActionReference;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] int magazineCapacity = 8;
    private int remainingBullets;
    private int currentAmmo;
    [SerializeField] float reloadTime = 0.7f;
    [SerializeField] float reloadFinishTime = 0.5f;
    private bool isReloading;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Shotgun);
        remainingBullets = magazineCapacity;
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
        if (!isReloading && (remainingBullets > 0)) {
            arg0.interactor.GetComponent<XRBaseController>().SendHapticImpulse(1f, .5f);
            audioManager.PlayShotgunSoundClip();
            remainingBullets--;
            Debug.Log("Magazine: " + remainingBullets + "   Reserve: " + currentAmmo);
            FireRaycastIntoScene();

            muzzleFlash.enableEmission = true;
            muzzleFlash.transform.position = raycastOrigin.position;
            muzzleFlash.Play();

        } else if (!isReloading && (remainingBullets <= 0)) {
            Debug.Log("Magazine: " + remainingBullets + "   Reserve: " + currentAmmo);
            audioManager.PlayEmptyMagazineClip();
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactor;
        if (interactor != null)
        {
            GameObject grabbedObject = interactor.selectTarget.gameObject;
            Debug.Log("Currently grabbed object: " + grabbedObject.name);
            if (grabbedObject.name == "Shotgun")
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

        gameManager.IncreaseAmmo(AmmoType.Shotgun, remainingBullets);
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Shotgun);
        if (currentAmmo >= magazineCapacity) {

            for (int i = 0; i < (magazineCapacity - remainingBullets); i++)
            {
                audioManager.PlayShotgunReloadClip();
                yield return new WaitForSeconds(reloadTime);
            }

            remainingBullets = magazineCapacity;
            gameManager.DecreaseAmmo(AmmoType.Shotgun, magazineCapacity);
        } else if ((currentAmmo < magazineCapacity) && (currentAmmo > 0)) {

            for (int i = 0; i < (currentAmmo - remainingBullets); i++)
            {
                audioManager.PlayShotgunReloadClip();
                yield return new WaitForSeconds(reloadTime);
            }

            remainingBullets = currentAmmo;
            gameManager.DecreaseAmmo(AmmoType.Shotgun, currentAmmo);
        }
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Shotgun);

        audioManager.PlayShotgunRackClip();
        yield return new WaitForSeconds(reloadFinishTime);

        isReloading = false;
        //Debug.Log("Reloaded!");
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.TakeDamage(WeaponType.Shotgun);
            Debug.Log("Enemy hit -100 HP");
        }
    }

    public void UpdateCurrentAmmo()
    {
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Shotgun);
    }

}
