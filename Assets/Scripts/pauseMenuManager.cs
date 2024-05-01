using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


public class pauseMenuManager : MonoBehaviour
{
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown ("p")) {
            if (Time.timeScale != 0) {  
                Pauza ();
            } else {
                Resume ();
            }
        }
    }

    public void Pauza() {
        Time.timeScale = 0;
        panel.SetActive(true);
        GameObject.Find ("Player").GetComponent <CharacterController>().enabled = false;
        GameObject.Find ("Player").GetComponent <MouseLooker>().enabled = false;
        GameObject.Find("Main Camera").GetComponent <Shooter>().enabled = false;
    }

    public void Resume () {
        Time.timeScale = 1; 
        panel.SetActive(false);
        GameObject.Find ("Player").GetComponent <CharacterController>().enabled = true;
        GameObject.Find ("Player").GetComponent <MouseLooker>().enabled = true;
        GameObject.Find("Main Camera").GetComponent <Shooter>().enabled = true;
    }

}
