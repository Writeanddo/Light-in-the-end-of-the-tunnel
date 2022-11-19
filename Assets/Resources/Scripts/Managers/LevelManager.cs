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

    public void LoadLevel(int levelNum)
    {
        int currentBuild = curLevel + offsetPlayable;
        int newBuild = levelNum + offsetPlayable;
        StartCoroutine(LevelLoading(newBuild, currentBuild));
        curLevel = levelNum;
    }

    IEnumerator LevelLoading(int loadIdx, int unloadIdx)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.UnloadSceneAsync(unloadIdx);
        SceneManager.LoadScene(loadIdx, LoadSceneMode.Additive);
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));

        if (Player.GetInstance() != null)
            Player.GetInstance().GetPlayerBody();
    }

    public bool hasNextLevel()
    {
        return curLevel + offsetPlayable + 1 <= totalLevels;
    }

    public void LoadEning()
    {
        Debug.Log("FIN!");
    }
}
