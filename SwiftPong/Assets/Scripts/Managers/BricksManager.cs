using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    private List<GameObject> bricks = new List<GameObject>();
    private List<Brick> bricksScripts = new List<Brick>();

    #region SingletonLogic
    private static BricksManager instance;
    public static BricksManager Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("The BricksManager instance is NULL");
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

    public void AddBricks()
    {
        bricks.Clear();
        bricks.AddRange(GameObject.FindGameObjectsWithTag("Brick"));
        for (int i = 0; i < bricks.Count; i++)
        {
            bricksScripts.Add(bricks[i].GetComponent<Brick>());
        }
    }

    public void RespawnBricks()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            bricks[i].SetActive(true);
            bricksScripts[i].ReEnable();
        }
    }
}
