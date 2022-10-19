using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectsBase : MonoBehaviour
{
    protected Bar player;
    protected SpriteRenderer spr;

    public Items myItem;

    protected float lifeTime;

    protected void InitializeVariables()
    {
        spr = GetComponent<SpriteRenderer>();

        spr.sprite = myItem.spr;
        lifeTime = myItem.lifeTime;
    }

    protected void FindReferencesInScene()
    {
        player = FindObjectOfType<Bar>();
    }

    public virtual void ApplyEffect()
    {
        UIManager.Instance.AddToInventory(spr.sprite, myItem, lifeTime, RemoveEffect);
        gameObject.SetActive(false);
    }

    public abstract void RemoveEffect();
}
