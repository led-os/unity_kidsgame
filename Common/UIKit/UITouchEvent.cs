using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public delegate void OnUITouchEventDelegate(UITouchEvent ev,PointerEventData eventData, int status);
public class UITouchEvent: MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public const int STATUS_TOUCH_DOWN = 0;
    public const int STATUS_TOUCH_MOVE = 1;
    public const int STATUS_TOUCH_UP = 2;
    bool isTouchDown = false;
    public OnUITouchEventDelegate callBackTouch { get; set; }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(this,eventData, STATUS_TOUCH_DOWN);
        }

    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(this,eventData, STATUS_TOUCH_UP);
        }
    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(this,eventData, STATUS_TOUCH_MOVE);
        }
    }

}
