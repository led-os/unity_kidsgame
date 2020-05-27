using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIWordItem : UIView
{
    public UIText textTitle;
    public UIText textPinyin;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    void Awake()
    {
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    void Start()
    {
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void LayOut()
    {
        base.LayOut();
        RectTransform rctran = this.GetComponent<RectTransform>();
        int fontsize = (int)(rctran.rect.width*0.5f);
        textTitle.fontSize = fontsize;

    }

    public void UpdateItem(string word,string pinyin)
    {
        textTitle.text = word;
        // textPinyin.text = pinyin;
        this.LayOut();

    }

}
