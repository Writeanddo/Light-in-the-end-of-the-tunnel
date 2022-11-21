using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] UnityEngine.UI.Button button1;
    [SerializeField] UnityEngine.UI.Button button2;
    [SerializeField] LightController flicker;
    [SerializeField] LightController menu1;
    [SerializeField] LightController menu2;
    [SerializeField] LightController credit;
    [SerializeField] LightController endingLight;
    [SerializeField] GameObject endingText;
    private bool isCreditOn;
    private bool isFlickering;
    private GameObject lastSelected;

    // Start is called before the first frame update
    void Start()
    {
        isCreditOn = false;
        isFlickering = false;
        endingLight.isOn = LevelManager.GetInstance().isFinished;
        endingText.SetActive(LevelManager.GetInstance().isFinished);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSelected != eventSystem.currentSelectedGameObject)
        {
            AudioManager.GetInstance().PlayClipForce("switch");
            lastSelected = eventSystem.currentSelectedGameObject;
        }
        menu1.isOn = button1.gameObject == lastSelected;
        menu2.isOn = button2.gameObject == lastSelected;
        credit.isOn = isCreditOn;

        if (!isFlickering)
        {
            isFlickering = true;
            StartCoroutine(Flicker());
        }
    }

    public void CreditSwitch()
    {
        AudioManager.GetInstance().PlayClipForce("switch");
        isCreditOn = !isCreditOn;
    }

    public void StartGame()
    {
        LevelManager lm = LevelManager.GetInstance();
        if (lm.hasNextLevel())
        {
            lm.LoadNextLevel();
        }
        else
        {
            lm.LoadEnding();
        }
    }

    IEnumerator Flicker()
    {
        AudioManager.GetInstance().PlayClipForce("flick");

        flicker.isOn = true;
        yield return new WaitForSeconds(0.3f);
        flicker.isOn = false;
        yield return new WaitForSeconds(0.3f);

        flicker.isOn = true;
        yield return new WaitForSeconds(0.2f);
        flicker.isOn = false;
        yield return new WaitForSeconds(0.2f);

        flicker.isOn = true;
        yield return new WaitForSeconds(3f);
        flicker.isOn = false;
        yield return new WaitForSeconds(0.2f);

        flicker.isOn = true;
        yield return new WaitForSeconds(0.4f);
        flicker.isOn = false;
        yield return new WaitForSeconds(1f);

        isFlickering = false;
    }
}
