using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundAudio;
    private AudioSource effectAudio;
    private Dictionary<string, AudioClip> backgrounds;
    private Dictionary<string, AudioClip> effects;

    private bool isOpenSetting = false;
    public GameObject settingCanvas;
    public GameObject settingPanel;
    public Text bgmVolumeText, effectVolumeText;
    public Slider bgmSlider, effectSlider;

    public static AudioManager instance = null;

    private void Awake()
    {
        instance = this;
        instance.LoadFile(ref instance.effects, "Effect/");
        instance.LoadFile(ref instance.backgrounds, "Background/");

        backgroundAudio = transform.GetChild(0).GetComponent<AudioSource>();
        effectAudio = transform.GetChild(1).GetComponent<AudioSource>();

        DontDestroyOnLoad(settingCanvas);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpenSetting = !isOpenSetting;
            OpenSettingScreen(isOpenSetting);
        }
    }

    public void OpenSettingScreen(bool on)
    {
        settingPanel.SetActive(on);

        bgmSlider.value = backgroundAudio.volume;
        effectSlider.value = effectAudio.volume;

        bgmVolumeText.text = bgmSlider.value.ToString();
        effectVolumeText.text = effectSlider.value.ToString();
    }

    public void GoToMenu()
    {
        Destroy(settingCanvas);
        Destroy(gameObject);
        SceneManager.LoadScene("LobbyScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object
    {
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            a.Add(particle.name, particle);
        }
    }

    public void SetEffectVolume(float scale)
    {
        scale = Mathf.RoundToInt(scale * 10) * 0.1f;
        effectAudio.volume = scale;

        effectVolumeText.text = scale.ToString();
    }
    public void SetBackgroundVolume(float scale)
    {
        scale = Mathf.RoundToInt(scale * 10) * 0.1f;
        backgroundAudio.volume = scale;

        bgmVolumeText.text = scale.ToString();
    }

    public void PlayEffect(string name)
    {
        effectAudio.PlayOneShot(effects[name]);
    }
    public void PlayBackground(string name)
    {
        backgroundAudio.Stop();
        backgroundAudio.loop = true;
        backgroundAudio.clip = backgrounds[name];
        backgroundAudio.Play();
    }
    public void StopBackground()
    {
        backgroundAudio.Stop();
    }
}