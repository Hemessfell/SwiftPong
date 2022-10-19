using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PickupGraphics : MonoBehaviour
{
    [HideInInspector] public Items myItem;

    Tweener sliderTween;
    Slider slider;

    public Action removeAction;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Add(Items item, Sprite spr, float lifeTime, Action removeAction)
    {
        myItem = item;
        slider.maxValue = lifeTime;
        slider.value = lifeTime;
        transform.GetChild(1).gameObject.GetComponent<Image>().sprite = spr;
        this.removeAction = removeAction;

        sliderTween = slider.DOValue(0.0f, lifeTime).OnComplete(() => Remove());
    }

    public void Remove()
    {
        Debug.Log("ffalksdflsdf");
        sliderTween.Kill();
        transform.DOScale(0.0f, 0.5f).OnComplete(() => Destroy(gameObject));
        UIManager.Instance.RemoveFromInventory(gameObject);
        removeAction.Invoke();
    }

    public void Reset(float lifeTime)
    {
        if(sliderTween != null)
        {
            sliderTween.Kill();
        }

        slider.maxValue = lifeTime;
        slider.value = lifeTime;
        sliderTween = slider.DOValue(0.0f, lifeTime).OnComplete(() => Remove());
    }
}