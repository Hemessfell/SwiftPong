using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var ii = i;
            Buttons button = transform.GetChild(ii).gameObject.GetComponent<Buttons>();
            button.onClick.AddListener(lfkasjf);
        }

    }

    private void lfkasjf()
    {

    }
}
