using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwitchHands : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    [SerializeField]
    Transform leftHandAttach;
    [SerializeField]
    Transform rightHandAttach;
    [SerializeField]
    GameObject leftHandInteractor;
    [SerializeField]
    GameObject rightHandInteractor;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void SwapHands()
    {
        if (grabInteractable.selectingInteractor.name == leftHandInteractor.name)
        {
            grabInteractable.attachTransform = leftHandAttach;
        }
        if (grabInteractable.selectingInteractor.name == rightHandInteractor.name)
        {
            grabInteractable.attachTransform = rightHandAttach;
        }
    }
}
