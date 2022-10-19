using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeBalls : EffectsBase
{
    public Items threeBallsItem;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        FindReferencesInScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect();
        }
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        FindObjectOfType<Ball>().InstantiateBalls(2);
    }

    public override void RemoveEffect()
    {
        Ball[] balls = FindObjectsOfType<Ball>();

        for (int i = 0; i < GameManager.Instance.numberOfBalls - 1; i++)
        {
            balls[i]?.Die();
        }

        for (int i = 0; i < UIManager.Instance.pickupGraphics.Count; i++)
        {
            if(UIManager.Instance.pickupGraphics[i].GetComponent<PickupGraphics>().myItem == threeBallsItem)
            {
                UIManager.Instance.pickupGraphics[i].GetComponent<PickupGraphics>().Remove();
                break;
            }
        }

        Destroy(gameObject);
    }
}
