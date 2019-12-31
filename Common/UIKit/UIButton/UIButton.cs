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
        IMAGE = 0,
        IMAGE_TEXT,
        IMAGE_ICON,//一张背景 一张Icon 叠加

    }

    public UIImage imageBg;
    public UIImage imageIcon;
    public UIText textTitle;

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

      
    }


}
