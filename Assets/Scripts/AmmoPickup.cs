using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class AmmoPickup : MonoBehaviour
{
    GameManager gameManager;
    AudioManager audioManager;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] AudioClip pickupSound;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {  
        audioManager.PlayAmmoPickupClip();
        gameManager.IncreaseAmmo(AmmoType.All, 0);
        Destroy(gameObject);
    }

}
