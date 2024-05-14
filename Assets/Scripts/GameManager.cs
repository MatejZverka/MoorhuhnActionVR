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
    private int defaultPistolAmmo = 19;
    private int defaultShotgunAmmo = 8;
    private int defaultRifleAmmo = 30;

    public int ChickenOnScene = 0;
    public int Chickenlimit = 2;

    /*
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
    */
    private void Start()
    {
        wristCanvas = GameObject.FindGameObjectWithTag("WristCanvas").GetComponent<WristCanvas>();
        ResetGame();
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
            case AmmoType.All:
                pistolAmmo += 19;
                shotgunAmmo += 8;
                rifleAmmo += 30;
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
        // XR Hands 1.3.0 is broken and will cause Null reference errors on scene reload and even restarting application from script
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("moorhunh");
        RestartApplication();
        ResetGame();
    }

    public void RestartApplication()
    {
        Application.Quit();
        Application.LoadLevel(Application.loadedLevel);
    }

    private void ResetGame()
    {
        playerScore = defaultPlayerScore;
        playerHealth = defaultPlayerHealth;
        pistolAmmo = defaultPistolAmmo;
        shotgunAmmo = defaultShotgunAmmo;
        rifleAmmo = defaultRifleAmmo;
    }

    private WristCanvas wristCanvas;
    private float timerDuration = 90f;
    private float timer;
    private bool isTimerRunning = false;
    private int minutes;
    private int seconds;

    public void StartGame()
    {
        isTimerRunning = true;
        timer = timerDuration;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            minutes = Mathf.FloorToInt(timer / 60);
            seconds = Mathf.FloorToInt(timer % 60);
            wristCanvas.UpdateTimeText(string.Format("{0:00}:{1:00}", minutes, seconds));

            if (timer <= 0)
            {
                RestartGame();
            }
        }
    }

    public void IncreaseChicken(){
        ChickenOnScene +=1;
    }

     public void DecreaseChicken(){
        ChickenOnScene -=1;
    }

    public bool CheckChickenLimit(){
        if (Chickenlimit >= ChickenOnScene){
            return true;
        }else return false; 
    }


}

public enum AmmoType
{
    Pistol,
    Shotgun,
    Rifle,
    All
}
