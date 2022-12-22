using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Button))]
public class UI_LevelSelectButton : MonoBehaviour
{
    void Awake()
    {
        m_Text = GetComponentInChildren<TMP_Text>();
        m_Button = GetComponent<Button>();
    }

    public void Init(int sceneNum, bool available = false)
    {
        m_Text.text = sceneNum.ToString();
        m_Scene = sceneNum + 2; //Adjust for buildIndex
        m_Button.interactable = available;
    }

    public void OnPressed()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (m_Scene < LevelManager.Instance.GetMaxScenes())
            LevelManager.Instance.LoadLevel(currentScene, m_Scene);
    }

    Button m_Button;
    TMP_Text m_Text;
    int m_Scene;

}
