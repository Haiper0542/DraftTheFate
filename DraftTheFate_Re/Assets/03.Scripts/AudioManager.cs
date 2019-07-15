using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundAudio;
    private AudioSource effectAudio;
    private Dictionary<string, AudioClip> backgrounds;
    private Dictionary<string, AudioClip> effects;

    public static AudioManager instance = null;

    private void Awake()
    {
        instance = this;
        instance.LoadFile(ref instance.effects, "Effect/");
        instance.LoadFile(ref instance.backgrounds, "Background/");
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
        if (effectAudio == null)
            effectAudio = transform.GetChild(1).GetComponent<AudioSource>();
        effectAudio.volume = scale;
    }
    public void SetBackgroundVolume(float scale)
    {
        if (backgroundAudio != null)
            backgroundAudio.volume = scale;
        else
            backgroundAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public void PlayEffect(string name)
    {
        if (effectAudio == null)
            effectAudio = transform.GetChild(1).GetComponent<AudioSource>();
        effectAudio.PlayOneShot(effects[name]);
    }
    public void PlayBackground(string name)
    {
        if (backgroundAudio == null)
            backgroundAudio = transform.GetChild(0).GetComponent<AudioSource>();
        backgroundAudio.Stop();
        backgroundAudio.loop = true;
        backgroundAudio.clip = backgrounds[name];
        backgroundAudio.Play();
    }
    public void StopBackground()
    {
        if (backgroundAudio != null)
            backgroundAudio.Stop();
        else
            backgroundAudio = transform.GetChild(0).GetComponent<AudioSource>();
    }
}