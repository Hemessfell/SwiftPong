using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    public bool isShooting;

    [SerializeField] private float minThreshold, maxThreshold;

    [HideInInspector] public SpriteRenderer spr;
    private Camera myCamera;

    private Coroutine shootCoroutine;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        myCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(Mathf.Clamp(mousePos.x, minThreshold, maxThreshold), transform.position.y);
    }

    public void Shoot()
    {
        shootCoroutine = StartCoroutine(TimeToShoot());
    }

    public void StopShooting()
    {
        StopCoroutine(shootCoroutine);
    }

    private IEnumerator TimeToShoot()
    {
        while (true)
        {
            Instantiate(bullet, transform.GetChild(0).GetChild(0).position, Quaternion.identity);
            Instantiate(bullet, transform.GetChild(0).GetChild(1).position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}