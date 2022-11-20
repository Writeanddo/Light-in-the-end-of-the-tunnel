using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    public int curLevel;
    public int totalLevels;
    [HideInInspector]
    public int offsetPlayable;
    [HideInInspector]
    public bool isFinished = false;

    [SerializeField]
    SpriteRenderer fade;

    public static LevelManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        
        offsetPlayable = 2;
        fade = GetComponent<SpriteRenderer>();
        fade.color = Vector4.zero;

        StartCoroutine(LevelLoadingInitial(1));
    }

    IEnumerator LevelLoadingInitial(int loadIdx)
    {
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadSceneAsync(loadIdx, LoadSceneMode.Additive);
        }
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));

        if (Player.GetInstance() != null)
            Player.GetInstance().GetPlayerBody();

        curLevel = SceneManager.GetActiveScene().buildIndex - offsetPlayable;
        totalLevels = SceneManager.sceneCountInBuildSettings - offsetPlayable;
    }

    public void LoadNextLevel()
    {
        int currentBuild = curLevel + offsetPlayable;
        curLevel++;
        if (curLevel + 1 > totalLevels)
        {
            curLevel--;
        }
        int newBuild = curLevel + offsetPlayable;
        StartCoroutine(LevelLoading(newBuild, currentBuild));
    }

    public void LoadLevel(int levelNum, bool noFade)
    {
        int currentBuild = curLevel + offsetPlayable;
        int newBuild = levelNum + offsetPlayable;
        StartCoroutine(LevelLoading(newBuild, currentBuild, noFade));
        curLevel = levelNum;
    }

    IEnumerator LevelLoading(int loadIdx, int unloadIdx, bool noFade=false)
    {
        if (unloadIdx != 1)
        {
            float overloadTimer = 0.5f;
            LightController[] lights = FindObjectsOfType<LightController>();
            foreach (LightController lc in lights)
            {
                lc.Overload(overloadTimer);
            }
            yield return new WaitForSeconds(overloadTimer * 2);
        }
        if (!noFade)
            fade.color = Color.black;
        yield return new WaitForSeconds(1.5f);

        SceneManager.UnloadSceneAsync(unloadIdx);
        SceneManager.LoadScene(loadIdx, LoadSceneMode.Additive);
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }

        if (!noFade)
            fade.color = Vector4.zero;

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        if (Player.GetInstance() != null)
            Player.GetInstance().GetPlayerBody();
    }

    public bool hasNextLevel()
    {
        return curLevel + 1 <= totalLevels;
    }

    public void LoadEning()
    {
        LoadLevel(-1, true);
        isFinished = true;
        Debug.Log("FIN!");
    }
}
