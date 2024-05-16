using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] public Button startButton;
    [SerializeField] public Button tutorialButton;
    [SerializeField] public Button quitButton;
    [SerializeField] public GameObject menuCanvas;
    [SerializeField] public GameObject tutorialCanvas;
    [SerializeField] public Button backButton;
    
    //private UnityEngine.AI.NavMeshAgent agent;

    private void Start()
    {
        //agent.speed = 0f;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startButton.onClick.AddListener(StartGame);
        tutorialButton.onClick.AddListener(StartTutorial);
        quitButton.onClick.AddListener(QuitGame);

        menuCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
    }

    private void StartGame()
    {
        startButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        menuCanvas.SetActive(false);
        tutorialCanvas.SetActive(false);
        

        //agent.speed = 4f;
        Debug.Log("Game is starting");
        gameManager.StartGame();
        
    }

    private void StartTutorial()
    {
        backButton.onClick.AddListener(MenuReturn);
        menuCanvas.SetActive(false);
        tutorialCanvas.SetActive(true);
    }

    private void MenuReturn()
    {
        backButton.onClick.RemoveAllListeners();
        menuCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

}
