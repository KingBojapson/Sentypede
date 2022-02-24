using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// self explanatory all the sound effects come from here
public class SoundManager : MonoBehaviour
{

    public static SoundManager soundInstance;

    GameObject Player;
    PlayerProperties playerProps;
    [Header("Enablers")]
    [SerializeField]
    private bool fX, music;

    [Header("Volume")]
    [SerializeField]
    [Range(0, 1)]
    public float musicVolume, fxVolume;

    [Header("Game Songs")]
    public AudioClip gameMusic;
    public AudioClip menuMusic;
    public AudioClip endMusic;


    [Header("FX Sounds")]
    public AudioClip death;
    public AudioClip podCollected;
    public AudioClip reverseSound;
    public AudioClip carStartUp;
    public AudioClip carExplosion;
    public AudioClip carDriveOff;
    public AudioClip carHorn;
    public AudioClip starCollected;
    public AudioClip tntExplosion;
    public AudioClip spikeSound;
    public AudioClip splats;
    public AudioClip landExplosion;
    public AudioClip resurrection;
    public AudioClip beeStingClip;

    [Header("VoiceSounds")]
    public AudioClip[] voiceOvers;

    [Header("Music Source")]
    public AudioSource gameMusicSource;
    public AudioSource menuMusicSource;
    public AudioSource playerSounds;
    public AudioSource vehicleSounds;
    public AudioSource podSounds;
    public AudioSource voiceSounds;
    public AudioSource splatSounds;
    public AudioSource explosionSounds;
    public AudioSource starSounds;
    public AudioSource landSound;
    public AudioSource beeSound;

    private void Awake()
    {
        MakeInstance();
        PlayMenuMusic();
    }

    void MakeInstance()
    {
        if (soundInstance == null)
        {
            soundInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (soundInstance != null)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        musicVolume = DataController.instance.musicPlay;
        fxVolume = DataController.instance.fxPlay;
        menuMusicSource.volume = musicVolume;
        gameMusicSource.volume = musicVolume;
        playerSounds.volume = fxVolume;
        vehicleSounds.volume = fxVolume;
        podSounds.volume = fxVolume;
        voiceSounds.volume = fxVolume;
        splatSounds.volume = fxVolume;
        explosionSounds.volume = fxVolume;
        starSounds.volume = fxVolume;

    }

    public void PlayMenuMusic()
    {
        gameMusicSource.Stop();
        menuMusicSource.Stop();

        menuMusicSource.clip = menuMusic;

        menuMusicSource.volume = musicVolume;

        menuMusicSource.loop = true;

        menuMusicSource.Play();
    }

    public void PlayBackgroundMusic()
    {
        menuMusicSource.Stop();
        gameMusicSource.Stop();

        gameMusicSource.clip = gameMusic;

        gameMusicSource.volume = musicVolume;

        gameMusicSource.loop = true;

        gameMusicSource.Play();

    }

    public void Die()
    {
        playerSounds.Stop();
        playerSounds.loop = false;
        playerSounds.clip = death;

        playerSounds.volume = fxVolume;
        playerSounds.PlayOneShot(playerSounds.clip);
        playerSounds.clip = null;
    }

    public void ReversedSound()
    {
        playerSounds.Stop();
        playerSounds.loop = false;
        playerSounds.clip = reverseSound;
        playerSounds.volume = fxVolume;
        playerSounds.PlayOneShot(playerSounds.clip);

        playerSounds.clip = null;
    }
    public void StarSound()
    {
        starSounds.Stop();
        starSounds.loop = false;
        starSounds.clip = starCollected;
        starSounds.volume = fxVolume;
        starSounds.PlayOneShot(starSounds.clip);

        starSounds.clip = null;
    }

    public void SpikeHit()
    {
        playerSounds.Stop();
        playerSounds.loop = false;
        playerSounds.clip = spikeSound;
        playerSounds.volume = fxVolume;
        playerSounds.PlayOneShot(playerSounds.clip);

        playerSounds.clip = null;
    }

    public void EndGame()
    {
        gameMusicSource.Stop();
        playerSounds.Stop();
        gameMusicSource.clip = endMusic;
        gameMusicSource.volume = musicVolume;
        gameMusicSource.loop = true;
        gameMusicSource.Play();
    }

    public void PodCollectSound()
    {
        playerSounds.Stop();
        playerSounds.loop = false;
        playerSounds.clip = podCollected;
        playerSounds.volume = fxVolume;
        playerSounds.PlayOneShot(playerSounds.clip);

        playerSounds.clip = null;
    }

    public void TNTSound()
    {
        explosionSounds.Stop();
        explosionSounds.loop = false;
        explosionSounds.clip = tntExplosion;
        explosionSounds.volume = fxVolume;
        explosionSounds.PlayOneShot(explosionSounds.clip);

        explosionSounds.clip = null;
    }

    public void VehicleExplosion()
    {
        vehicleSounds.Stop();
        vehicleSounds.loop = false;
        vehicleSounds.clip = tntExplosion;
        vehicleSounds.volume = fxVolume;

        vehicleSounds.PlayOneShot(vehicleSounds.clip);

        vehicleSounds.clip = null;
    }

    public void CarStartUp()
    {
        playerSounds.Stop();
        playerSounds.loop = false;
        playerSounds.clip = carStartUp;
        playerSounds.volume = fxVolume;

        playerSounds.PlayOneShot(playerSounds.clip);

        playerSounds.clip = null;
    }

    public void Splatters()
    {
        splatSounds.Stop();
        splatSounds.loop = false;
        splatSounds.clip = splats;
        splatSounds.volume = fxVolume;

        splatSounds.PlayOneShot(splatSounds.clip);

        splatSounds.clip = null;
    }


    public void BeeStings()
    {
        beeSound.Stop();
        beeSound.loop = false;
        beeSound.clip = beeStingClip;
        beeSound.volume = fxVolume;

        beeSound.PlayOneShot(beeSound.clip);
        splatSounds.clip = null;
    }
    public void FruitSound(int fNum)
    {
        switch (fNum)
        {
            case 0:
                voiceSounds.Stop();
                voiceSounds.loop = false;
                voiceSounds.clip = voiceOvers[0];
                voiceSounds.volume = fxVolume;
                voiceSounds.PlayOneShot(voiceSounds.clip);

                voiceSounds.clip = null;
                break;

            case 1:
                voiceSounds.Stop();
                voiceSounds.loop = false;
                voiceSounds.clip = voiceOvers[1];
                voiceSounds.volume = fxVolume;
                voiceSounds.PlayOneShot(voiceSounds.clip);

                voiceSounds.clip = null;
                break;

            case 2:
                voiceSounds.Stop();
                voiceSounds.loop = false;
                voiceSounds.clip = voiceOvers[2];
                voiceSounds.volume = fxVolume;
                voiceSounds.PlayOneShot(voiceSounds.clip);

                voiceSounds.clip = null;
                break;
        }
    }

}
