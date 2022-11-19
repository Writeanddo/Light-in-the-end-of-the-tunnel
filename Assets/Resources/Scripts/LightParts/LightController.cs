
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightController : MonoBehaviour
{
    public bool startsOn = false;
    public bool isOn;
    private bool wasOn;
    public float fadeTime = 1f;

    Collider2D lightCollider;
    private UnityEngine.Rendering.Universal.Light2D lightSource;

    public void Start()
    {
        lightCollider = GetComponent<Collider2D>();
        lightSource = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        lightSource.intensity = startsOn ? 1 : 0;
    }

    public void Update()
    {
        if (wasOn != isOn)
        {
            StopAllCoroutines();
            StartCoroutine(fadeInAndOut(isOn));
        }
        wasOn = isOn;
    }

    IEnumerator fadeInAndOut(bool fadeIn)
    {
        float minLuminosity = 0;
        float maxLuminosity = 1;

        float a, b;
        if (fadeIn)
        {
            a = minLuminosity;
            b = maxLuminosity;
        }
        else
        {
            a = maxLuminosity;
            b = minLuminosity;
        }

        float currentIntensity = lightSource.intensity;
        float timer = fadeTime * (1 - Mathf.Abs((b - currentIntensity)));
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            lightSource.intensity = Mathf.Lerp(a, b, timer / fadeTime);
            lightCollider.enabled = lightSource.intensity > maxLuminosity / 2;
            yield return null;
        }
    }
}