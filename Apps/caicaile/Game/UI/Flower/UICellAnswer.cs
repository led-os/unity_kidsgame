
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICellAnswer : UIView
{
    public int index;
    public UIItemAnswer uiItem0;
    public UIItemAnswer uiItem1;
    public UIItemAnswer uiItem2;
    public UIItemAnswer uiItem3;
    string strAnswer;
    public List<UIItemAnswer> listItem;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UIItemAnswer>();
        listItem.Add(uiItem0);
        listItem.Add(uiItem1);
        listItem.Add(uiItem2);
        listItem.Add(uiItem3);

        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();
    }

    public void UpdateItem(string word)
    {
        strAnswer = word;
        for (int i = 0; i < listItem.Count; i++)
        {
            UIItemAnswer item = listItem[i];
            item.UpdateItem(word.Substring(i, 1));
        }
    }
    public bool IsRightAnswer(string word)
    {
        bool ret = false;
        if (strAnswer == word)
        {
            ret = true;
        }
        return ret;
    }

    public void Show(int idx)
    {
        UIItemAnswer item = listItem[idx];
        item.Show();
    }
    public Vector2 GetPosAnswerLetter(int idxLetter)
    {
        UIItemAnswer item = listItem[idxLetter];
        return item.transform.position;

    }
    public UIItemAnswer GetFirstItemNotAnswer()
    {
        foreach (UIItemAnswer ui in listItem)
        {
            if (!ui.textTitle.gameObject.activeSelf)
            {
                return ui;
            }
        }
        return null;
    }
}
