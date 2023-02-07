using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Brick : MonoBehaviour
{
    [SerializeField] private GameObject breakParticle;
    [SerializeField] private GameObject[] pickups;

    private Image img;
    private Camera myCamera;
    private Tween shakeCameraTween;

    [SerializeField] private float life = 1;

    private Sequence changeSequence;

    private float startLife;

    private Vector3 myCameraStartPos;

    private void Awake()
    {
        img = GetComponent<Image>();

        SetLife();
    }

    private void Start()
    {
        myCamera = GameObject.FindGameObjectWithTag("ShakeCamera").GetComponent<Camera>();
        myCameraStartPos = myCamera.transform.position;
    }

    private void SetLife()
    {
        life = Random.Range(1, 4);

        if (life == 2)
        {
            int random = Random.Range(1, 4);

            if (random == 1)
            {
                life = 2;

                img.color = new Color(0.1215686f, 0.567f, 0.213f);
            }
            else
            {
                life = 1;
            }
        }
        
        if(life == 3)
        {
            int random = Random.Range(1, 7);

            if(random == 3)
            {
                life = 3;

                img.color = new Color(0.524f, 0.1216429f, 0.3485558f);
            }
            else
            {
                life = 1;
            }
        }
        
        if (life == 1)
        {
            img.color = new Color(0.589f, 0.589f, 0.589f);
        }

        startLife = life;
    }

    public void Break()
    {
        life--;

        if (changeSequence != null)
            changeSequence.Kill();
        if (shakeCameraTween != null)
            shakeCameraTween.Kill();

        myCamera.transform.position = myCameraStartPos;
        shakeCameraTween = myCamera.DOShakePosition(Random.Range(0.15f, 0.25f), 0.25f, 20, 90.0f, false);
        UIManager.Instance.UpdateSpeedometer(false);

        if (life <= 0)
        {
            transform.localScale = new Vector3(0.0f, 0.0f);
            Instantiate(breakParticle, transform.position, Quaternion.identity);
            Drop();
            gameObject.SetActive(false);
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            changeSequence = sequence.Append(transform.DOScale(1.25f, 0.05f).SetLoops(2, LoopType.Yoyo));

            if (life == 1)
            {
                img.color = new Color(0.589f, 0.589f, 0.589f);
            }
            else if (life == 2)
            {
                img.color = new Color(0.1215686f, 0.567f, 0.213f);
            }
        }
    }

    public void ReEnable()
    {
        SetLife();

        if (changeSequence != null)
            changeSequence.Kill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.25f, 0.15f)).Append(transform.DOScale(1.0f, 0.05f));
    }

    private void Drop()
    {
        int rand = Random.Range(0, 11); 

        if(rand == 0)
        {
            int random = Random.Range(0, pickups.Length);
            Instantiate(pickups[random], transform.position, Quaternion.identity);
        }
    }
}
