using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BricksSpawner : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private float amount;

    private void Awake()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(brick, transform.GetChild(0));
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).gameObject.GetComponent<GridLayoutGroup>().enabled = false;
    }
}
