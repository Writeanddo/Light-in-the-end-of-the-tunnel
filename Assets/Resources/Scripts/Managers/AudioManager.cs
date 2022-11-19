using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public c_AudioClip[] audioClips;

    private static AudioManager instance;

    public static AudioManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(c_AudioClip ac in audioClips)
        {
            GameObject audioSourceObject = new GameObject(ac.name ??= "New AudioClip");
            audioSourceObject.transform.SetParent(transform);
            ac.source = audioSourceObject.AddComponent<AudioSource>();
            ac.source.clip = ac.clip;
            ac.source.playOnAwake = ac.playOnAwake;
            ac.source.volume = ac.volume;
            ac.source.pitch = ac.pitch;
            ac.source.loop = ac.loop;
            ac.source.outputAudioMixerGroup = ac.mixerGroup;
        }
    }

    private void Start()
    {
        foreach(c_AudioClip ac in audioClips)
        {
            if (ac.playOnAwake)
            {
                ac.source.volume = 0f;
                ac.source.Play();
                StartCoroutine(FadeSound(ac, 5f, true));
            }
        }
    }

    public void PlayClip(string name, bool fading = false)
    {
        c_AudioClip ac = Array.Find(audioClips, audioClip => audioClip.name == name);
        if (ac == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        if ((!ac.source.isPlaying) && (ac.source.time != 0))
        {
            ac.source.UnPause();
        }
        else
        {
            ac.source.Play();
        }
        if (fading)
        {
            StopCoroutine(FadeSound(ac, 2.5f));
        }
    }

    public void PlayClip(string name, float randomPitchStart, float randomPitchEnd = 1f, float fading = 0f)
    {
        randomPitchStart = Mathf.Clamp01(randomPitchStart);
        randomPitchEnd = Mathf.Clamp01(randomPitchEnd);
        c_AudioClip ac = Array.Find(audioClips, audioClip => audioClip.name == name);
        if (ac == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        if ((!ac.source.isPlaying) && (ac.source.time != 0))
        {
            ac.source.UnPause();
        }
        else
        {
            ac.source.pitch = UnityEngine.Random.Range(randomPitchStart, randomPitchEnd);
            ac.source.Play();
        }
        if (fading != 0f)
        {
            StopCoroutine(FadeSound(ac, fading));
        }
    }

    public void PauseClip(string name)
    {
        c_AudioClip ac = Array.Find(audioClips, audioClip => audioClip.name == name);
        if (ac == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        ac.source.Pause();
    }

    public void StopClip(string name)
    {
        c_AudioClip ac = Array.Find(audioClips, audioClip => audioClip.name == name);
        if (ac == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        ac.source.Stop();
    }

    public void PauseAll()
    {
        foreach (c_AudioClip ac in audioClips)
        {
            if (!ac.unpausable)
            {
                ac.source.Pause();
            }
        }
    }

    public void PauseAllForce()
    {
        foreach (c_AudioClip ac in audioClips)
        {
            ac.source.Pause();
        }
    }

    public void ResumeAll()
    {
        foreach (c_AudioClip ac in audioClips)
        {
            if ((!ac.source.isPlaying) && (ac.source.time != 0))
            {
                ac.source.UnPause();
            }
        }
    }

    public void StopAll(bool leaveUnpausable = false)
    {
        foreach (c_AudioClip ac in audioClips)
        {
            if (leaveUnpausable && ac.unpausable) continue;
            ac.source.Stop();
        }
    }

    public void FadeChangeSound(string oldSound, string newSound, float fadeDuration, bool mix)
    {
        c_AudioClip acOld = Array.Find(audioClips, audioClip => audioClip.name == oldSound);
        c_AudioClip acNew = Array.Find(audioClips, audioClip => audioClip.name == newSound);
        acNew.source.Play();
        StartCoroutine(FadeSound(acOld, fadeDuration, false));
        StartCoroutine(FadeSound(acNew, fadeDuration, true));
    }

    IEnumerator FadeSound(c_AudioClip ac, float duration, bool fadeUp = false)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            ac.source.volume = Mathf.Lerp(fadeUp ? 0f : ac.volume , fadeUp ? ac.volume : 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ac.source.volume = fadeUp ? ac.volume : 0f;
        if (ac.source.volume == 0f)
        {
            ac.source.Stop();
        }
    }
}
