using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numberOfBalls = 1;

    #region Singleton Logic
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("The GameManager instance is NULL");
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

    public void AddNumberOfBalls()
    {
        numberOfBalls++;
    }

    public void SubtractNumberOfBalls()
    {
        numberOfBalls--;
    }
}
