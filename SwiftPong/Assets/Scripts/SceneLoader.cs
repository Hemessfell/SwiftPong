using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public string[] scenesName;

    #region Singleton Logic
    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("The SceneLoader instance == NULL");
            }

            return instance;
        }
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start()
    {
        EndLoad();
    }

    public void EndApplication()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneToLoad)
    {
        TimeToLoad(sceneToLoad);
    }

    public void LoadScene(string sceneToLoad)
    {
        TimeToLoad(sceneToLoad);
    }

    public void EndLoad()
    {
        UIManager.Instance.FadeGroup().alpha = 1.0f;
        UIManager.Instance.FadeGroup().DOFade(0.0f, 1.0f); ;
    }

    private void TimeToLoad(int scene)
    {
        UIManager.Instance.FadeGroup().DOFade(1.0f, 1.0f).SetUpdate(true).OnComplete(() => OnLoadComplete(scene));
    }

    private void TimeToLoad(string scene)
    {
        UIManager.Instance.FadeGroup().DOFade(1.0f, 1.0f).SetUpdate(true).OnComplete(() => OnLoadComplete(scene));
    }

    public void MoveObjectToAnotherScene(GameObject gameObject)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    private void OnLoadComplete(int scene)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(scene);
    }

    private void OnLoadComplete(string scene)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(scene);
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public int GetSceneIndexThroughName(string name)
    {
        return SceneUtility.GetBuildIndexByScenePath(name);
    }

    public string[] GetTransitionalScenes()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenesName = new string[sceneCount];
        this.scenesName = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenesName[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if(scenesName[i].StartsWith("Fase"))
                this.scenesName[i] = scenesName[i];
        }

        return this.scenesName;
    }
}