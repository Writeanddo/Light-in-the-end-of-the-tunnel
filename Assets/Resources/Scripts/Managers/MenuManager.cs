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
    public bool isCreditOn;
    private bool isFlickering;

    // Start is called before the first frame update
    void Awake()
    {
        isCreditOn = false;
        isFlickering = false;
    }

    // Update is called once per frame
    void Update()
    {
        menu1.isOn = button1.gameObject == eventSystem.currentSelectedGameObject;
        menu2.isOn = button2.gameObject == eventSystem.currentSelectedGameObject;
        credit.isOn = isCreditOn;

        if (!isFlickering)
        {
            isFlickering = true;
            StartCoroutine(Flicker());
        }
    }

    public void CreditSwitch()
    {
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
            lm.LoadEning();
        }
    }

    IEnumerator Flicker()
    {
        flicker.isOn = true;
        yield return new WaitForSeconds(0.2f);
        flicker.isOn = false;
        yield return new WaitForSeconds(0.2f);

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
        yield return new WaitForSeconds(0.2f);

        isFlickering = false;
    }
}
