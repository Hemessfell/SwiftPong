using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

//[RequireComponent(typeof(BoxCollider2D))]
public class Buttons : MonoBehaviour
{
    public UnityEvent onClick;

    public enum Effects { scale};
    public Effects[] myActions;

    private bool isScaled;
    private bool wasOver;

    private void Update()
    {
        if (UIManager.Instance.IsOverThisButton(this))
        {
            ApplyEffects();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PerformAction();
            }
        }
        else if(wasOver)
        {
            ResetBools();
            ResetTransform();
        }

        wasOver = UIManager.Instance.IsOverThisButton(this);
    }

    public void ApplyEffects()
    {
        for (int i = 0; i < myActions.Length; i++)
        {
            if(myActions[i] == Effects.scale && !isScaled)
            {
                DoScale();
            }
        }
    }

    private void DoScale()
    {
        transform.localScale = new Vector3(1.15f, 1.15f);
        isScaled = true;
    }

    private void ResetBools()
    {
        isScaled = false;
    }

    private void ResetTransform()
    {
        transform.localScale = new Vector3(1.0f, 1.0f);
    }

    private void PerformAction()
    {
        onClick.Invoke();
    }
}
