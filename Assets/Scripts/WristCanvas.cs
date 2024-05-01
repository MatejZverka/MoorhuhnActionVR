using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WristCanvas : MonoBehaviour
{
    [SerializeField] public TMP_Text wristText;
    [SerializeField] public TMP_Text timeText;

    public void UpdateWristText(string newText)
    {
        wristText.text = newText;
    }

    public void UpdateTimeText(string newText)
    {
        timeText.text = newText;
    }

}
