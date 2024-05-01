using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Rifle : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;
    GameManager gameManager;
    AudioManager audioManager;
    [SerializeField] InputActionReference reloadActionReference;
    [SerializeField] InputActionReference triggerActionReference;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] int magazineCapacity = 30;
    private int remainingBullets;
    private int currentAmmo;
    [SerializeField] float reloadTime = 2.5f;
    private bool isReloading;
    private bool holdingRifle = false;
    XRBaseInteractor firingInteractor;
    private WristCanvas wristCanvas;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        wristCanvas = GameObject.FindGameObjectWithTag("WristCanvas").GetComponent<WristCanvas>();
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Rifle);
        remainingBullets = magazineCapacity;
        muzzleFlash.Stop();
    }
    
    private void OnEnable()
    {
        grabInteractable.activated.AddListener(OnTriggerHeldVibrations);
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grabInteractable.activated.RemoveListener(OnTriggerHeldVibrations);
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private float timer = 0f;
    private float updateInterval = 0.1f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer -= updateInterval;
            OnTriggerHeld();
        }
    }
    
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactor;
        if (interactor != null)
        {
            GameObject grabbedObject = interactor.selectTarget.gameObject;
            Debug.Log("Currently grabbed object: " + grabbedObject.name);
            if (grabbedObject.name == "Rifle")
            {
                UpdateCurrentAmmo();
                wristCanvas.UpdateWristText(remainingBullets + " | " + currentAmmo);
                reloadActionReference.action.performed += OnPrimaryButtonPressed;
                holdingRifle = true;
            }
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        //Debug.Log("Object released. Cancelling code execution.");
        wristCanvas.UpdateWristText("  |  ");
        reloadActionReference.action.performed -= OnPrimaryButtonPressed;
        holdingRifle = false;
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

    private void OnTriggerHeld()
    {
        if (holdingRifle && (triggerActionReference != null) && (triggerActionReference.action != null) && (triggerActionReference.action.ReadValue<float>() == 1))
        {
            if (!isReloading && (remainingBullets > 0)) {
                firingInteractor.GetComponent<XRBaseController>().SendHapticImpulse(.75f, .1f);
                audioManager.PlayRifleSoundClip();
                remainingBullets--;
                Debug.Log("Magazine: " + remainingBullets + "   Reserve: " + currentAmmo);
                FireRaycastIntoScene();

                UpdateCurrentAmmo();
                wristCanvas.UpdateWristText(remainingBullets + " | " + currentAmmo);
 
                muzzleFlash.enableEmission = true;
                muzzleFlash.transform.position = raycastOrigin.position;
                muzzleFlash.Play();

            } else if (!isReloading && (remainingBullets <= 0)) {
                Debug.Log("Magazine: " + remainingBullets + "   Reserve: " + currentAmmo);
                audioManager.PlayEmptyMagazineClip();
            }
        }
    }

    private void OnTriggerHeldVibrations(ActivateEventArgs arg0)
    {
        firingInteractor = arg0.interactor;
    }

    private IEnumerator Reload()
    {
        //Debug.Log("Reloading...");
        isReloading = true;
        audioManager.PlayRifleReloadClip();
        yield return new WaitForSeconds(reloadTime);

        gameManager.IncreaseAmmo(AmmoType.Rifle, remainingBullets);
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Rifle);
        if (currentAmmo >= magazineCapacity) {
            remainingBullets = magazineCapacity;
            gameManager.DecreaseAmmo(AmmoType.Rifle, magazineCapacity);
        } else if ((currentAmmo < magazineCapacity) && (currentAmmo > 0)) {
            remainingBullets = currentAmmo;
            gameManager.DecreaseAmmo(AmmoType.Rifle, currentAmmo);
        }
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Rifle);

        UpdateCurrentAmmo();
        wristCanvas.UpdateWristText(remainingBullets + " | " + currentAmmo);
        isReloading = false;
        //Debug.Log("Reloaded!");
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.TakeDamage(WeaponType.Rifle);
            Debug.Log("Enemy hit -70 HP");
        }
    }

    public void UpdateCurrentAmmo()
    {
        currentAmmo = gameManager.GetCurrentAmmo(AmmoType.Rifle);
    }

}
