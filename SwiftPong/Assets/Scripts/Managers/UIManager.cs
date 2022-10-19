using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI secondsTxt;
    [SerializeField] private GameObject hpHolder, flash, speedometer, inventoryHolder, pickupGraphic, counter, levelHolder, mainMenu;
    [SerializeField] private Transform pointer;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private Canvas[] canvases;

    private Tweener counterSilderTween;

    public List<GameObject> pickupGraphics;

    private float seconds = 11;

    [SerializeField] private float maxVelocity;
    [HideInInspector] public float velocityModifier = 1;

    #region Singleton Logic
    private static UIManager instance;
    public static UIManager Instance 
    {
        get
        {
            if (instance == null)
                Debug.LogError("The UIManager instance is NULL");
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].renderMode == RenderMode.ScreenSpaceOverlay)
                continue;

            canvases[i].worldCamera = Camera.main;
        }
    }
    #endregion

    public bool IsOverThisButton(Buttons me)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<Buttons>() == me)
            {
                return true;
            }
        }

        return false;
    }

    private void StartPlayingSlider()
    {
        counterSilderTween = slider.DOValue(0.0f, 10.0f).SetLoops(-1, LoopType.Restart).SetSpeedBased(true).OnStepComplete(() => UpdateSecondsText());
    }

    private void UpdateSecondsText()
    {
        seconds--;
        secondsTxt.transform.DOScale(secondsTxt.transform.localScale * 1.25f, 0.1f).SetLoops(2, LoopType.Yoyo);

        if(seconds == 0)
        {
            seconds = 10;
            BricksManager.Instance.RespawnBricks();
        }

        if(seconds == 3)
        {
            slider.fillRect.GetComponent<Image>().DOColor(Color.red, 0.0f);
        }

        if(seconds == 6)
        {
            slider.fillRect.GetComponent<Image>().DOColor(Color.yellow, 0.0f);
        }

        if(seconds == 10)
        {
            slider.fillRect.GetComponent<Image>().DOColor(Color.green, 0.0f);
        }

        secondsTxt.text = seconds.ToString();
    }

    public void UpdateHPImages(bool willDisable)
    {
        if (willDisable)
        {
            for (int i = 0; i < hpHolder.transform.childCount; i++)
            {
                if (!hpHolder.transform.GetChild(i).gameObject.activeInHierarchy)
                    continue;

                flash.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(1.0f, 0.05f).SetLoops(2, LoopType.Yoyo);

                hpHolder.transform.GetChild(i).gameObject.SetActive(false);

                GameObject temp = GameObjectExtension.InstantiateEmptyGameObject(hpHolder.transform.GetChild(i).position, 
                    hpHolder.transform.GetChild(i).rotation, hpHolder.transform, typeof(Image));

                RectTransform rect = temp.GetComponent<RectTransform>();
                Image img = temp.GetComponent<Image>();

                temp.transform.localScale = hpHolder.transform.GetChild(i).gameObject.transform.localScale;
                temp.GetComponent<Image>().sprite = hpHolder.transform.GetChild(i).gameObject.GetComponent<Image>().sprite;
                rect.sizeDelta = hpHolder.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta;

                temp.transform.DOScale(2.0f, 0.75f);
                temp.transform.DOMoveY(temp.transform.position.y + 1, 0.75f);
                img.DOColor(Color.red, 0.0f);
                img.DOFade(0.0f, 0.75f).OnComplete(() => Destroy(temp));
                break;
            }
        }
        else
        {
            for (int i = 0; i < hpHolder.transform.childCount; i++)
            {
                hpHolder.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void UpdateSpeedometer(bool willReset)
    {
        if (!willReset)
        {
            float incrementation = 0.025f;


            if(velocityModifier >= maxVelocity)
            {
                return;
            }

            velocityModifier += incrementation;
            pointer.Rotate(0.0f, 0.0f, -11);
        }
        else
        {
            velocityModifier = 1.0f;

            pointer.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 20.0f));
        }
    }

    public void AddToInventory(Sprite img, Items item, float lifeTime, Action removeAction)
    {
        for (int i = 0; i < pickupGraphics.Count; i++)
        {
            if(pickupGraphics[i].GetComponent<PickupGraphics>().myItem == item)
            {
                pickupGraphics[i].GetComponent<PickupGraphics>().Reset(lifeTime);
                return;
            }
        }

        if(pickupGraphics.Count <= 3)
        {
            GameObject temp = Instantiate(pickupGraphic, inventoryHolder.transform);
            temp.GetComponent<PickupGraphics>().Add(item, img, lifeTime, removeAction);
            pickupGraphics.Add(temp);
        }
    }

    public void RemoveFromInventory(GameObject graphics)
    {
        pickupGraphics.Remove(graphics);
    }

    public void RemoveAllItemsFormInventory()
    {
        for (int i = 0; i < pickupGraphics.Count; i++)
        {
            pickupGraphics[i].GetComponent<PickupGraphics>().Remove();
        }
    }

    public void GoToMainMenu()
    {
        counter.SetActive(false);
        hpHolder.SetActive(false);
        speedometer.transform.parent.gameObject.SetActive(false);
        inventoryHolder.transform.parent.gameObject.SetActive(false);
        levelHolder.SetActive(false);
        mainMenu.SetActive(true);
        if (counterSilderTween != null)
            counterSilderTween.Kill();
    }

    public void GoToLevelSelector()
    {
        counter.SetActive(false);
        hpHolder.SetActive(false);
        speedometer.transform.parent.gameObject.SetActive(false);
        inventoryHolder.transform.parent.gameObject.SetActive(false);
        levelHolder.SetActive(true);
        mainMenu.SetActive(false);
        if (counterSilderTween != null)
            counterSilderTween.Kill();
    }

    public void LoadNormalScene()
    {
        counter.SetActive(true);
        hpHolder.SetActive(true);
        speedometer.transform.parent.gameObject.SetActive(true);
        inventoryHolder.transform.parent.gameObject.SetActive(true);
        levelHolder.SetActive(false);
        mainMenu.SetActive(false);
        UpdateSecondsText();
        StartPlayingSlider();
    }

    public CanvasGroup FadeGroup()
    {
        return fadeGroup;
    }

    public void StartGame()
    {
        SceneLoader.Instance.LoadScene(1);
    }
}
