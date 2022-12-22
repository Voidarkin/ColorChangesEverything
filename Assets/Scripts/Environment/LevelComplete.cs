using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextScene = currentScene + 1;
            if(nextScene >= LevelManager.Instance.GetMaxScenes())
            {
                nextScene = 0; //To main menu
            }
            else
            {
                LevelManager.Instance.IncreaseLevelReached();
            }
            LevelManager.Instance.LoadLevel(currentScene, nextScene);
        }
    }
}
