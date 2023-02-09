using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] float radius, speed;
    DodgeballPlayer player;
    Transform target;
    void Start()
    {
        player = FindObjectOfType<DodgeballPlayer>();
        target = player.transform;
        StartCoroutine(Spawn());
    }

    void Update()
    {
        
    }
    void SpawnBallsRandom()
    {
        Vector3 randomPos = new Vector3 (transform.position.x + (Random.insideUnitCircle.x * radius),
            (transform.position.y + (Random.insideUnitCircle.x * radius)));
        GameObject balls = Instantiate(ball, randomPos, Quaternion.identity);
        //balls.transform.DORotate(new Vector3(0.0f, 0.0f, 10000.0f), 10.0f, RotateMode.FastBeyond360);
        if (target != null)
        {
            Vector2 moveDirection = (target.position - balls.transform.position).normalized * speed;
            balls.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDirection.x, moveDirection.y));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    private IEnumerator Spawn()
    {
        SpawnBallsRandom();
        yield return new WaitForSeconds(1);
        StartCoroutine(Spawn());
    }
}
