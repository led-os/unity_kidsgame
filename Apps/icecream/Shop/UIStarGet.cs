using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStarGet : UIView
{
    public Image imageBg;
    public Image imageBoard;
    public Image imageTitle;
    public Image imageStar0;
    public Image imageStar1;
    public Image imageStar2;
    public Button btnClose;
    public GameObject objContent;
    public GameObject objLayoutStar;
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


}
