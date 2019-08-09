using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWin : UIViewPop
{
    public UITextView textView;

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;

    }

    public void OnClickBtnClose()
    {
        // PopUpManager.main.ClosePopup();
        Close();
    }
}
