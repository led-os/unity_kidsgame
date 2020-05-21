using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIIconController : UIShotBase
{
    public UIImage imageBg;
    public UIImage imageHD;
    public UIImage imageBoard;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        imageHD.gameObject.SetActive(false);
        imageBoard.gameObject.SetActive(false);
    }
    void Start()
    {
        IconViewController iconctroller = (IconViewController)this.controller;
        if (iconctroller != null)
        {
            if (iconctroller.deviceInfo.isIconHd)
            {
                imageHD.gameObject.SetActive(true);
            }

        }
        LayOut();
        OnUIDidFinish(0.5f);
    }
    public override void LayOut()
    {
        base.LayOut();
    }
}
