using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadEvent : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneLoader.Instance.EndLoad();

        if(scene.name == "MainMenu")
        {
            UIManager.Instance.GoToMainMenu();
        }else if(scene.name == "LevelSelector")
        {
            UIManager.Instance.GoToLevelSelector();
        }else if (scene.name.StartsWith("Level"))
        {
            BricksManager.Instance.AddBricks();
            UIManager.Instance.LoadNormalScene();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
