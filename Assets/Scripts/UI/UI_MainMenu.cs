using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{

    public int NewGameSceneIndex;
    public GameObject MainMenuUI;
    public GameObject LevelSelectUI;
    public Transform LevelSelectView;
    public GameObject LevelSelectButtonPrefab;

    void Awake()
    {
        if(!MainMenuUI) { return; }
        if (!LevelSelectUI) { return; }

        MainMenuUI.SetActive(true);
        LevelSelectUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_Levels = new GameObject[0];
    }

    public void NewGame()
    {
        LevelManager.Instance.LoadLevel(0, NewGameSceneIndex);
    }

    public void OpenLevelSelect()
    {
        MainMenuUI.SetActive(false);
        LevelSelectUI.SetActive(true);

        if (!LevelSelectView) { return; }

        int max = LevelManager.Instance.MaxLevel;

        if(m_Levels.Length != 0)
            System.Array.Clear(m_Levels, 0, max);

        m_Levels = new GameObject[max];
        for (int i = 0; i < max; i++)
        {
            m_Levels[i] = Instantiate(LevelSelectButtonPrefab, LevelSelectView);

            UI_LevelSelectButton item = m_Levels[i].GetComponent<UI_LevelSelectButton>();
            bool available = ((i + 1  <= LevelManager.Instance.GetLevelReached())) ? true : false;
            item.Init(i + 1, available);
        }

    }

    public void OpenLevelNum(int i)
    {
        int test = LevelManager.Instance.GetMaxScenes();
        if (i < 2 || i > LevelManager.Instance.GetMaxScenes()) { return; }

        LevelManager.Instance.LoadLevel(0, i);
    }

    public void Default()
    {
        MainMenuUI.SetActive(true);
        LevelSelectUI.SetActive(false);

        if(LevelSelectView.childCount == 0) { return; }

        for (int i = 0; i < LevelSelectView.childCount; i++)
        {
            Destroy(LevelSelectView.GetChild(i).gameObject);  
        }
    }

    public void OpenOptions()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    GameObject[] m_Levels;

}
