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
    [SerializeField] public Button quitButton;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        Debug.Log("Game is starting");
        startButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        startButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        gameManager.StartGame();
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

}
