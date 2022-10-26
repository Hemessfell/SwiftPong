using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    Touch touch;
    Vector3 dragStartPos;

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
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if(touch.phase == TouchPhase.Moved)
            {
                Dragging();
            }
            /*if(touch.phase == TouchPhase.Ended)
            {
                DragRelease();
            }*/
        }
       
    }

    private void DragStart()
    {
        dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos.z = 0f;
        transform.position = new Vector3(Mathf.Clamp(dragStartPos.x, minThreshold, maxThreshold), transform.position.y);
    }

   private void Dragging()
    {
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        draggingPos.z = 0f;
        transform.position = new Vector3(Mathf.Clamp(draggingPos.x, minThreshold, maxThreshold), transform.position.y);
    }
    /*
    private void DragRelease()
    {

    }*/

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