using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUINaviBarClickBackDelegate(UINaviBar bar);

public class UINaviBar : UIView
{
    public Image imageBg;
    public Text textTitle;
    public Button btnBack;
    public OnUINaviBarClickBackDelegate callbackBackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
    public void UpdateTitle(string title)
    {
        textTitle.text = title;
    }

    public void HideBtnBack(bool isHide)
    {
        btnBack.gameObject.SetActive(!isHide);
    }

    public void OnClickBtnBack()
    {
        if (callbackBackClick != null)
        {
            callbackBackClick(this);
        }
    }
}

