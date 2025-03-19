using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    [Header(" --- Music Settings --- ")]
    [SerializeField] Slider _musicSlider = null;
    public TMP_Text musicValueText = null;
    
    AudioSource _musicSource;
    
    [Header(" --- SFX Settings --- ")]
    [SerializeField] Slider _sfxSlider = null;
    public TMP_Text sfxValueText = null;
    
    AudioSource _sfxSource;

    [Header(" --- GameOver --- ")]
    public GameObject GameOver;
    public TextMeshProUGUI GameOverScoreText;

    [Header(" --- Settings Button --- ")]
    public Button musicButton;
    public Button sfxButton;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _musicSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        _sfxSource = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();

        //LoadSoundData();
        musicValueText.text = _musicSlider.value.ToString("0");
        sfxValueText.text = _sfxSlider.value.ToString("0");
    }

    //private void LoadSoundData()
    //{
    //    _musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
    //    _sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume");
    //}

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);

    }public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("musicVolume", _musicSource.volume);
        PlayerPrefs.SetFloat("sfxVolume", _sfxSource.volume);

        Debug.Log("musicVolume: " + _musicSource.volume);
        Debug.Log("sfxVolume: " + _sfxSource.volume);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
