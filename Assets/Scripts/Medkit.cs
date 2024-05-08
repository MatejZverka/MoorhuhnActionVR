using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


public class Medkit : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] XRGrabInteractable grabInteractable;

    void Start()
    {
     gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();   
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
        gameManager.IncreaseHealth(20);
        Destroy(gameObject);
    }
}
