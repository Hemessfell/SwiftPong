using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballPlayer : MonoBehaviour
{
    Touch touch;
    Vector3 dragStartPos, realPosition;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if (touch.phase == TouchPhase.Moved)
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
        realPosition = transform.position;
        realPosition.z = 0f;
        //transform.position = new Vector3(Mathf.Clamp(dragStartPos.x, minThreshold, maxThreshold), transform.position.y);
    }

    private void Dragging()
    {
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        float draggingResultX = dragStartPos.x - draggingPos.x;
        float draggingResultY = dragStartPos.y - draggingPos.y ;
        transform.position = new Vector3((realPosition.x - draggingResultX), (realPosition.y - draggingResultY));
    }
}
