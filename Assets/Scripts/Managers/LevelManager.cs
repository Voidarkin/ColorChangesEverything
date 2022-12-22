using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager Instance;
    public int LoadingScreenScene;
    public int MaxLevel = 20;

    void Awake()
    {    
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        m_LevelReached = 1;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == LoadingScreenScene)
        {

        }
    }

    public void LoadLevel(int currentScene, int newScene, bool fromSave = false)
    {
        if(currentScene == newScene && !fromSave) { return; }
        
        m_PreviousScene = currentScene;
        m_NextScene = newScene;

        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {

        AsyncOperation intoLoad = SceneManager.LoadSceneAsync(LoadingScreenScene);

        while (!intoLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(5);

        AsyncOperation intoNextScene = SceneManager.LoadSceneAsync(m_NextScene);

        while (!intoNextScene.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5);

        AsyncOperation intoNextScene = SceneManager.LoadSceneAsync(m_NextScene);

        while (!intoNextScene.isDone)
        {
            yield return null;
        }
    }

    public int GetMaxScenes() { return SceneManager.sceneCountInBuildSettings; }
    public int GetLevelReached() { return m_LevelReached; }
    public void IncreaseLevelReached()
    {
        m_LevelReached++;
    }
    public int GetCurrentScene() { return SceneManager.GetActiveScene().buildIndex; }

    public Player GetPlayer()
    {
        return m_CurrentPlayer;
    }

    public void RegisterPlayer(Player player)
    {
        m_CurrentPlayer = player;
    }

    public void UnregisterPlayer()
    {
        if (m_CurrentPlayer)
        {
            Destroy(m_CurrentPlayer.gameObject);
        }
        m_CurrentPlayer = null;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 3000, 60), "Color Changes Everything Alpha");
    }

    Player m_CurrentPlayer;

    int m_PreviousScene;
    int m_NextScene;

    int m_LevelReached;
}

