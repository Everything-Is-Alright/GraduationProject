using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    [SerializeField] private AudioSource bgmSource; 
    [SerializeField] private AudioSource sfxSource;

    [Header("预设音频")]
    public AudioClip bgmSound;
    public AudioClip attackSound;

    [Header("音量调节（0-10）")]
    [Range(0f, 1f)] public float bgmVolume = 1f; 
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 

        InitAudioSources();
    }

    private void Start()
    {
        PlayBGM();
        UpdateBGMVolume();
        UpdateSFXVolume();
    }

    private void InitAudioSources()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }
    }

    private void PlayBGM()
    {
        if (bgmSound == null)
        {
            Debug.LogWarning("没有BGM音频文件！");
            return;
        }
        bgmSource.clip = bgmSound;
        bgmSource.Play();
    }

    public void PlayAttackSound()
    {
        if (attackSound == null)
        {
            Debug.LogWarning("攻击音效文件未赋值！");
            return;
        }
        
        sfxSource.PlayOneShot(attackSound,sfxVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        UpdateBGMVolume();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSFXVolume();
    }

    private void UpdateBGMVolume()
    {
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume; 
        }
    }

    private void UpdateSFXVolume()
    {
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
}