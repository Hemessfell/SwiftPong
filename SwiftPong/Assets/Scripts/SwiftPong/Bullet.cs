using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.up * speed);
        Destroy(gameObject, 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            Brick brick = other.gameObject.GetComponent<Brick>();
            if (brick != null)
            {
                brick.Break();
                Destroy(gameObject);
            }
        }
    }
}
