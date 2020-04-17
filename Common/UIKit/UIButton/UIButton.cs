using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIView
{

    public enum Type
    {
        IMAGE = 0,//一张背景  
        IMAGE_TEXT,
        IMAGE_ICON,//一张背景 一张Icon 叠加

        IMAGE_SWITCH,//一张背景
        IMAGE_ICON_SWITCH,//一张背景 一张Icon 叠加

    }

    public UIImage imageBg;
    public UIImage imageIcon;
    public UIText textTitle;

    bool isSwicthSelect;
    public Type _type;

    public Type type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            imageBg.gameObject.SetActive(true);
            switch (_type)
            {
                case Type.IMAGE:
                case Type.IMAGE_SWITCH:
                    {
                        imageIcon.gameObject.SetActive(false);
                        textTitle.gameObject.SetActive(false);
                    }
                    break;
                case Type.IMAGE_TEXT:
                    {
                        imageIcon.gameObject.SetActive(false);
                        textTitle.gameObject.SetActive(true);
                    }
                    break;
                case Type.IMAGE_ICON:
                case Type.IMAGE_ICON_SWITCH:
                    {
                        imageIcon.gameObject.SetActive(true);
                        textTitle.gameObject.SetActive(false);
                    }
                    break;

            }
        }

    }

    public void Awake()
    {
        base.Awake();
        type = _type;
        LayOut();

    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();

    }

    public override void LayOut()
    {
        base.LayOut();

        if (type == Type.IMAGE_TEXT)
        {
            if (textTitle.isFitFontWidth)
            {
                //自动适配大小
                RectTransform rctranOrigin = this.GetComponent<RectTransform>();
                Vector2 offsetMin = rctranOrigin.offsetMin;
                Vector2 offsetMax = rctranOrigin.offsetMax;
                RectTransform rctran = this.transform as RectTransform;
                Vector2 sizeDelta = rctran.sizeDelta;
                sizeDelta.x = textTitle.width;
                rctran.sizeDelta = sizeDelta;
                if ((rctran.anchorMin == new Vector2(0.5f, 0.5f)) && (rctran.anchorMax == new Vector2(0.5f, 0.5f)))
                {
                }
                else
                {
                    //sizeDelta 会自动修改offsetMin和offsetMax 所以需要还原
                    rctran.offsetMin = offsetMin;
                    rctran.offsetMax = offsetMax;
                }

            }
        }


    }
    public void UpdateSwitch(bool isSel)
    {
        isSwicthSelect = isSel;
        if (isSwicthSelect)
        {
            imageBg.UpdateImageByKey(imageBg.keyImage);
            imageIcon.UpdateImageByKey(imageIcon.keyImage);
        }
        else
        {
            imageBg.UpdateImageByKey(imageBg.keyImage2);
            imageIcon.UpdateImageByKey(imageIcon.keyImage2);
        }
    }

}
