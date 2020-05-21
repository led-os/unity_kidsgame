using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAdHomeController : UIShotBase
{
    public UIImage imageBg;
    public UIText textTitle;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
        OnUIDidFinish(0.5f);
    }
    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;



    }
}
