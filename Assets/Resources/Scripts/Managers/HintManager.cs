using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class HintManager : MonoBehaviour
{
    [ListDrawerSettings]
    public c_Hint[] hints;
    [SerializeField]
    private GameObject HintHelpText;

    private static HintManager instance;

    public static HintManager GetInstance()
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

        foreach(c_Hint h in hints)
        {
            h.isOn = false;
        }

        HintHelpText.SetActive(false);
    }

    public bool canHint(int curLevel) => (hints.Where(h => h.levelNumber == curLevel).Count() > 0);

    public void SwitchHint(int curLevel)
    {
        GameObject fg = GameObject.FindGameObjectWithTag("Foreground");
        if (fg != null && canHint(curLevel))
        {
            c_Hint hint = hints.Where(h => h.levelNumber == curLevel).First();
            hint.isOn = !hint.isOn;
            fg.GetComponent<SpriteRenderer>().sprite = hint.isOn ? hint.hinted : hint.casual;
        }
        else
        {
            Debug.Log(curLevel + ", can hint: " + canHint(curLevel) + "; fg is null: " + fg == null);
        }
    }

    public void ShowHintHelpText(bool isOn)
    {
        HintHelpText.SetActive(isOn && canHint(LevelManager.GetInstance().curLevel + 1));
    }
}
