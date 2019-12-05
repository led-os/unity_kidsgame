using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIBgHome : UIView
{
    public RawImage imageBg;
    public Image imageBgName;
    public void Awake()
    {
        imageBg.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        LayOut();

    }



    public override void LayOut()
    {

    }

}
