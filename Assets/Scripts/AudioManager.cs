using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("--- AudioClip ---")]
    //public List<AudioClip> selectedCard;
    //public List<AudioClip> doubleCard;
    //public List<AudioClip> tripleCard;
    //public AudioClip Fail;

    public Sound[] musicSound;
    public Sound[] sfxSound;

    [Header("--- AudioSource ---")]
    public AudioSource musicSouce;
    public AudioSource SFXSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //if (!musicSouce.isPlaying)
        //{
        //    musicSouce.Play();
        //}

        PlayMusic("BackGround");
    }

    //public void PlaySFX(AudioClip clip)
    //{
    //    SFXSource.PlayOneShot(clip);
    //}

    //public void ContinueMusicFrom(float time)
    //{
    //    Debug.Log("Time: " + time);
    //    musicSouce.time = time;

    //    if (!musicSouce.isPlaying)  // Sadece müzik oynamıyorsa başlat
    //    {
    //        musicSouce.Play();
    //    }
    //}

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.soundName == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSouce.clip = s.sound[0];
            musicSouce.Play();
        }
    }
    public void PlaySFX(string name, int index)
    {
        Sound s = Array.Find(sfxSound, x => x.soundName == name);
            
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            SFXSource.PlayOneShot(s.sound[index]);
        }
    }

    public void ToggleMusic()
    {
        musicSouce.mute = !musicSouce.mute;
        ButtonManager.instance.musicButton.GetComponent<Image>().color = (musicSouce.mute) ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 255);
    }
    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSouce.volume = volume / 10;
        ButtonManager.instance.musicValueText.text = volume.ToString("0");
    }
    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume / 10;
        ButtonManager.instance.sfxValueText.text = volume.ToString("0");
    }
}
