using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIText : UIView
{
    public Text title;
    public string text
    {
        get
        {
            return title.text;
        }

        set
        {
            title.text = value;
            LayOut();
        }

    }

    public int fontSize
    {
        get
        {
            return title.fontSize;
        }

        set
        {
            title.fontSize = value;
            LayOut();
        }

    }
    public Color color
    {
        get
        {
            return title.color;
        }

        set
        {
            title.color = value;
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
        title.fontSize = sz;
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
