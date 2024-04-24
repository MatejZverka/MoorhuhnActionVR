using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : MonoBehaviour
{
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    //[SerializeField] Transform target;
    [SerializeField] LayerMask targetLayer;

    private void OnEnable() => grabInteractable.activated.AddListener(TriggerPulled);
    private void OnDisable() => grabInteractable.activated.RemoveListener(TriggerPulled);


    public void TriggerPulled(ActivateEventArgs arg0)
    {
        arg0.interactor.GetComponent<XRBaseController>().SendHapticImpulse(.5f, .25f);
        //Debug.Log("It's vibrating, alright");
        FireRaycastIntoScene();
    }

    private void FireRaycastIntoScene()
    {
        RaycastHit hit;
        Debug.Log("fire");

        //Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + 1f * transform.forward); 

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
            Debug.Log("The impossible condition");
        }
    }

}
