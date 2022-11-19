
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class c_AudioClip
{
    public AudioClip clip;

    public string name;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;
    public bool loop;
    public bool playOnAwake;
    public bool unpausable;
    public AudioMixerGroup mixerGroup;

    [HideInInspector] public AudioSource source;
}
