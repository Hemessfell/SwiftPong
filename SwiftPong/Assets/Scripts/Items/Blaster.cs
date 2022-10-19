using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Blaster : EffectsBase
{
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
        if (!player.isShooting)
        {
            player.isShooting = true;
            player.Shoot();
        }

        base.ApplyEffect();
    }

    public override void RemoveEffect()
    {
        player.isShooting = false;
        player.StopShooting();
        Destroy(gameObject);
    }
}
