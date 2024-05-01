using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Ammo Pickup")]
    [SerializeField] AudioClip ammoPickup;
    [SerializeField][Range(0f, 1f)] float ammoPickupVolume = 0.5f;

    [Header("Grenade")]
    [SerializeField] AudioClip grenadePin;
    [SerializeField][Range(0f, 1f)] float grenadePinVolume = 1f;
    [SerializeField] AudioClip grenadeExplosion;
    [SerializeField][Range(0f, 1f)] float grenadeExplosionVolume = 0.25f;

    [Header("Shotgun")]
    [SerializeField] AudioClip shotgunSound;
    [SerializeField][Range(0f, 1f)] float shotgunSoundVolume = 0.45f;
    [SerializeField] AudioClip shotgunReload;
    [SerializeField][Range(0f, 1f)] float shotgunReloadVolume = 0.8f;
    [SerializeField] AudioClip shotgunRack;
    [SerializeField][Range(0f, 1f)] float shotgunRackVolume = 0.75f;

    [Header("Rifle")]
    [SerializeField] AudioClip rifleSound;
    [SerializeField][Range(0f, 1f)] float rifleSoundVolume = 0.75f;
    [SerializeField] AudioClip rifleReload;
    [SerializeField][Range(0f, 1f)] float rifleReloadVolume = 0.7f;

    [Header("Misc")]
    [SerializeField] AudioClip emptyMagazine;
    [SerializeField][Range(0f, 1f)] float emptyMagazineVolume = 1f;
    [SerializeField] AudioClip backgroundMusic;
    public AudioSource backgroundMusicSource;
    [SerializeField][Range(0f, 1f)] float backgroundMusicVolume = 0.25f;

    public static AudioManager audioPlayer;
    public AudioMixer audioMixer;

    void Awake()
    {
        if (audioPlayer != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            audioPlayer = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = backgroundMusicVolume;
        backgroundMusicSource.Play();
    }



    public void PlayAmmoPickupClip()
    {
        PlayClip(ammoPickup, ammoPickupVolume);
    }

    public void PlayGrenadePinClip()
    {
        PlayClip(grenadePin, grenadePinVolume);
    }

    public void PlayGrenadeExplosionClip()
    {
        PlayClip(grenadeExplosion, grenadeExplosionVolume);
    }

    public void PlayShotgunSoundClip()
    {
        PlayClip(shotgunSound, shotgunSoundVolume);
    }

    public void PlayShotgunReloadClip()
    {
        PlayClip(shotgunReload, shotgunReloadVolume);
    }

    public void PlayShotgunRackClip()
    {
        PlayClip(shotgunRack, shotgunRackVolume);
    }

    public void PlayRifleSoundClip()
    {
        PlayClip(rifleSound, rifleSoundVolume);
    }

    public void PlayRifleReloadClip()
    {
        PlayClip(rifleReload, rifleReloadVolume);
    }

    public void PlayEmptyMagazineClip()
    {
        PlayClip(emptyMagazine, emptyMagazineVolume);
    }
    


    public void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
    
}
