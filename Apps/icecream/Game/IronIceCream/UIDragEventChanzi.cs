using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void UIDragEventChanziOnAlphaChange(float alpha);
public class UIDragEventChanzi : UIDragEvent//, IPointerUpHandler, IPointerDownHandler//, IDragHandler
{

    float moveStepYMin = 0.1f;
    float moveY;

    public UIDragEventChanziOnAlphaChange callBackALpha { get; set; }
    int count;
    public void OnReset()
    {
        count = 0;
    }
    //相当于touchDown
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        moveY = posworld.y;
    }
    //相当于touchUp
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
    //相当于touchMove
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if (!enableDrag)
        {
            return;
        }
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        float stepy = posworld.y - moveY;
        if (stepy > moveStepYMin)
        {
            moveY = posworld.y;
            OnAlphaChange();
        }
    }

    void OnAlphaChange()
    {
        float alpha_setp = 0.1f;

        //float duration = 8f;
        float alpha = 0f;
        alpha = 0f + alpha_setp * count;
        if (alpha > 1f)
        {
            alpha = 1f;
        }

        Debug.Log("OnAlphaChange alpha=" + alpha);
        if (callBackALpha != null)
        {
            callBackALpha(alpha);
        }
        count++;
    }
}
