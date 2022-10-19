using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    private Rigidbody2D rb;
    private Camera myCamera;

    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Vector2 moveSpeed;

    [SerializeField] private float rotateSpeed, yPositionToDie;
    private float baseMoveSpeedX, baseMoveSpeedY;

    [SerializeField] private int health = 2;

    private int initialHealth;

    private bool rightChecker, leftChecker, canMove = true, canDie = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        myCamera = GameObject.FindGameObjectWithTag("ShakeCamera").GetComponent<Camera>();

        float x = 1;
        float y = 1;

        moveSpeed *= new Vector2(x * UIManager.Instance.velocityModifier, y * UIManager.Instance.velocityModifier);
        rb.velocity = new Vector2(Mathf.Abs(moveSpeed.x), Mathf.Abs(moveSpeed.y));

        baseMoveSpeedX = Mathf.Abs(moveSpeed.x);
        baseMoveSpeedY = Mathf.Abs(moveSpeed.y);
        initialHealth = health;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        rightChecker = Physics2D.Raycast(transform.position, Vector2.right, 0.75f, whatIsWall);
        leftChecker = Physics2D.Raycast(transform.position, Vector3.left, 0.75f, whatIsWall);

        if(transform.position.y < yPositionToDie && canDie)
        {
            if(GameManager.Instance.numberOfBalls == 1)
            {
                DieAndRespawn();
            }
            else
            {
                Die();
            }

            canDie = false;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (moveSpeed.x > 0)
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, Mathf.Abs(moveSpeed.x), Mathf.Abs(moveSpeed.x)), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -Mathf.Abs(moveSpeed.x), -Mathf.Abs(moveSpeed.x)), rb.velocity.y);
            }

            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, Mathf.Abs(moveSpeed.y * UIManager.Instance.velocityModifier), Mathf.Abs(moveSpeed.y * UIManager.Instance.velocityModifier)));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(moveSpeed.y * UIManager.Instance.velocityModifier), -Mathf.Abs(moveSpeed.y * UIManager.Instance.velocityModifier)));
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int random = Random.Range(-1, 2);

            while(random == 0)
                random = Random.Range(-1, 2);

            if (transform.position.x > other.transform.position.x)
            {
                moveSpeed.x = (baseMoveSpeedX * UIManager.Instance.velocityModifier) + (random * Random.Range(0.5f, 1.0f));
            }
            else
            {
                moveSpeed.x = (-baseMoveSpeedX * UIManager.Instance.velocityModifier) + (random * Random.Range(0.5f, 1.0f));
            }
        }

        if (other.gameObject.CompareTag("Brick"))
        {
            Brick brick = other.gameObject.GetComponent<Brick>();
            if (brick != null)
            {
                brick.Break();
            }
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            if (rightChecker || leftChecker)
            {
                moveSpeed *= -1;
            }
        }
    }

    private void DieAndRespawn()
    {
        health--;
        transform.localScale = Vector3.zero;
        transform.position = Vector3.zero;
        canMove = false;
        UIManager.Instance.velocityModifier = 1;
        UIManager.Instance.UpdateSpeedometer(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0.75f, 0.25f)).Join(myCamera.DOShakePosition(Random.Range(0.45f, 0.55f), 0.25f, 20, 90.0f, false))
            .AppendCallback(() => canMove = true).AppendCallback(() => SetUIHPImages())
            .AppendCallback(() => canDie = true).OnComplete(() => SetSpeed(baseMoveSpeedX, baseMoveSpeedY));

        UIManager.Instance.UpdateHPImages(true);
    }

    public void Die()
    {
        GameManager.Instance.SubtractNumberOfBalls();

        if (GameManager.Instance.numberOfBalls == 1)
        {
            ThreeBalls threeBalls = FindObjectOfType<ThreeBalls>(true);

            if (threeBalls != null)
                threeBalls.RemoveEffect();
        }

        transform.DOScale(0.0f, 0.25f).OnComplete(() => Destroy(gameObject));
    }

    private void SetUIHPImages()
    {
        if(health <= 0)
        {
            UIManager.Instance.UpdateHPImages(false);
            UIManager.Instance.RemoveAllItemsFormInventory();
            health = initialHealth;
        }
    }

    private void SetSpeed(float x, float y)
    {
        moveSpeed = new Vector2(x * UIManager.Instance.velocityModifier, y * UIManager.Instance.velocityModifier);
        rb.velocity = moveSpeed;
    }

    public void InstantiateBalls(int numberOfGenerations)
    {
        for (int i = 0; i < numberOfGenerations; i++)
        {
            GameObject ball = Instantiate(this.ball, Vector3.zero, Quaternion.identity);
            if (i % 2 == 0)
                ball.GetComponent<Ball>().SetSpeed(-baseMoveSpeedX, baseMoveSpeedY);
            else
                ball.GetComponent<Ball>().SetSpeed(baseMoveSpeedX, baseMoveSpeedY);
            GameManager.Instance.AddNumberOfBalls();
        }
    }
}