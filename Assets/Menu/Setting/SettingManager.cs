using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance {  get; private set; }

    [Header("“Ù¡ø…Ë÷√")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private float currentBGMVolume = 1f;
    private float currentSFXVolume = 1f;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitBGMSetting();
        InitSFXSetting();
    }

    private void InitBGMSetting()
    {
        currentBGMVolume = AudioManager.Instance.bgmVolume;

        bgmSlider.minValue = 0f;
        bgmSlider.maxValue = 1f;
        bgmSlider.value = currentBGMVolume;
        bgmSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);
    }

    private void InitSFXSetting()
    {
        currentSFXVolume = AudioManager.Instance.sfxVolume;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.value = currentSFXVolume;
        sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
    }

    public void OnBGMSliderValueChanged(float volume)
    {
        currentBGMVolume = Mathf.Clamp01(volume);
        AudioManager.Instance.SetBGMVolume(currentBGMVolume);
    }

    public void OnSFXSliderValueChanged(float volume)
    {
        currentSFXVolume = Mathf.Clamp01(volume);
        AudioManager.Instance.SetSFXVolume(currentSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        currentBGMVolume = Mathf.Clamp01(volume);
        bgmSlider.value = currentBGMVolume;
        AudioManager.Instance.SetBGMVolume(currentBGMVolume);
    }

    public void SetSFXVolume(float volume)
    {
        currentSFXVolume = Mathf.Clamp01(volume);
        sfxSlider.value = currentSFXVolume;
        AudioManager.Instance.SetSFXVolume(currentSFXVolume);
    }
}
