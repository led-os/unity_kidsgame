using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopFoodToolBar : UIView
{
    public void OnClickBtnDel()
    {
        IronIceCreamStepBase.uiWanIron.OnDeleteTopFood();
    }
    //逆时针旋转
    public void OnClickBtnRotationAdd()
    {
        IronIceCreamStepBase.uiWanIron.OnRotationAddTopFood();

    }


    //顺时针旋转
    public void OnClickBtnRotationMinus()
    {
        IronIceCreamStepBase.uiWanIron.OnRotationMinusTopFood();

    }
    public void OnClickBtnMinus()
    {
        IronIceCreamStepBase.uiWanIron.OnScaleMinusTopFood();
    }
    public void OnClickBtnAdd()
    {
        IronIceCreamStepBase.uiWanIron.OnScaleAddTopFood();
    }

}
