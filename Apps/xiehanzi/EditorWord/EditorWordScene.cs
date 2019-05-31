using System.Collections;
using System.Collections.Generic;
using Moonma.AdKit.AdVideo;
using UnityEngine;
using UnityEngine.UI;
public class EditorWordScene : AppSceneBase
{
    public Canvas canvasWord;
    public Text textWord;
    public Camera cameraBihuaShow;
    public override void RunApp()
    {
        base.RunApp();
        SetRootViewController(EditorWordViewController.main);
    }

}
