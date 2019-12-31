using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIText : UIView
{
    public Text textTitle;
    public string text
    {
        get
        {
            return textTitle.text;
        }

        set
        {
            textTitle.text = value;
            LayOut();
        }

    }

    public int fontSize
    {
        get
        {
            return textTitle.fontSize;
        }

        set
        {
            textTitle.fontSize = value;
            LayOut();
        }

    }
    public Color color
    {
        get
        {
            return textTitle.color;
        }

        set
        {
            textTitle.color = value;
        }

    }

    //



    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        UpdateLanguage();
        Debug.Log("keyColor=" + keyColor);
        if (!Common.isBlankString(keyColor))
        {
            this.color = GetKeyColor();
        }
        if (!Common.isBlankString(keyText))
        {
            this.text = GetKeyText();
        }
        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
    }

    public void SetFontSize(int sz)
    {
        textTitle.fontSize = sz;
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }
    public override void UpdateLanguage()
    {
        base.UpdateLanguage();
        string str = GetKeyText();
        if (!Common.isBlankString(str))
        {
            this.text = GetKeyText();
        }
        LayOut();
    }
}
