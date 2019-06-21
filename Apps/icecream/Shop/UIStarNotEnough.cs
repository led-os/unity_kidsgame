using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStarNotEnough : UIView
{
    public Image imageBg;
    public Image imageBoard;
    public Button btnClose;
    public GameObject objContent;

    void Awake()
    {



    }
    void Start()
    {
        LayOut();

    }


    public override void LayOut()
    {
        float x, y, w, h, ratio, scale;

    }

    public void OnClickBtnClose()
    {
        PopViewController p = this.controller as PopViewController;
        if (p != null)
        {
            p.Close();
        }
    }

    public void OnClickBtnBuy()
    {

    }
    public void OnClickBtnFree()
    {

    }
    public void OnClickBtnPlay()
    {

    }
}
