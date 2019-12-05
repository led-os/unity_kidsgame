using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IUIWordContentBaseDelegate
{
    //回退word
    void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word);

    //提示
    void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word);

}

public class UIWordContentBase : UIView
{
    public ItemInfo infoItem;

    private IUIWordContentBaseDelegate _delegate;

    public IUIWordContentBaseDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    public virtual void UpdateGuankaLevel(int level)
    {
    }
    public virtual void OnTips()
    {
    }

    public virtual void OnAddWord(string word)
    {
    }
    public virtual void OnReset()
    {
    }


    public virtual bool CheckAllFill()
    {
        return false;
    }
    public virtual bool CheckAllAnswerFinish()
    {
        return false;
    }
    public virtual void UpdateWord()
    {
    }
}
