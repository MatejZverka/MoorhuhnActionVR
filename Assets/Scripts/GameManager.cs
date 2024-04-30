using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public int playerScore;
    public int playerHealth;
    public int pistolAmmo;
    public int shotgunAmmo;
    public int rifleAmmo;

    private int defaultPlayerScore = 0;
    private int defaultPlayerHealth = 100;
    private int defaultPistolAmmo = 2;
    private int defaultShotgunAmmo = 8;
    private int defaultRifleAmmo = 30;

    public AudioSource backgroundMusic;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    public void IncreaseScore(int amount)
    {
        playerScore += amount;
    }

    public void IncreaseHealth(int amount)
    {
        playerHealth += amount;
        if (playerHealth > defaultPlayerHealth)
        {
            playerHealth = defaultPlayerHealth;
        }
    }

    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            RestartGame();
        }
    }

    public int GetCurrentAmmo(AmmoType type)
    {
        switch (type)
        {
            case AmmoType.Pistol:
                return pistolAmmo;
            case AmmoType.Shotgun:
                return shotgunAmmo;
            case AmmoType.Rifle:
                return rifleAmmo;
            default:
                return 0;
        }
    }

    public void IncreaseAmmo(AmmoType type, int amount)
    {
        switch (type)
        {
            case AmmoType.Pistol:
                pistolAmmo += amount;
                break;
            case AmmoType.Shotgun:
                shotgunAmmo += amount;
                break;
            case AmmoType.Rifle:
                rifleAmmo += amount;
                break;
        }
    }

    public void DecreaseAmmo(AmmoType type, int amount)
    {
        switch (type)
        {
            case AmmoType.Pistol:
                pistolAmmo -= amount;
                break;
            case AmmoType.Shotgun:
                shotgunAmmo -= amount;
                break;
            case AmmoType.Rifle:
                rifleAmmo -= amount;
                break;
        }
    }

    private void RestartGame()
    {
        ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGame()
    {
        playerScore = defaultPlayerScore;
        playerHealth = defaultPlayerHealth;
        pistolAmmo = defaultPistolAmmo;
        shotgunAmmo = defaultShotgunAmmo;
        rifleAmmo = defaultRifleAmmo;
    }
}

public enum AmmoType
{
    Pistol,
    Shotgun,
    Rifle
}