using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//成语接龙
/*
成语小秀才
https://www.taptap.com/app/167939
 */

public interface IUIWordFillBoxDelegate
{
    //回退word
    void UIWordFillBoxDidBackWord(UIWordFillBox ui, string word);

}
public class UIWordFillBox : UIView
{
    public Image imageBg;
    public Text textTitle;
    public UILetterItem uiLetterItemPrefab;
    public int row = 7;
    public int col = 7;
    public List<UILetterItem> listItem;
    LayOutGrid lygrid;
    int indexFillWord;
    int indexAnswer;
    private IUIWordFillBoxDelegate _delegate;

    public IUIWordFillBoxDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    void Awake()
    {
        lygrid = this.GetComponent<LayOutGrid>();
        listItem = new List<UILetterItem>();
        row = 7;
        col = 7;
        lygrid.row = row;
        lygrid.col = col;
        lygrid.enableLayout = false;
        lygrid.dispLayVertical = LayOutBase.DispLayVertical.TOP_TO_BOTTOM;
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
        float x, y, w, h;
        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        if (lygrid != null)
        {
            lygrid.LayOut();
            foreach (UILetterItem item in listItem)
            {
                Vector2 pos = lygrid.GetItemPostion(item.indexRow, item.indexCol);
                RectTransform rctran = item.GetComponent<RectTransform>();
                w = (rctranRoot.rect.width - (lygrid.space.x) * (col - 1)) / col;
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = pos;
                item.LayOut();
            }
        }


    }
    public void UpdateGuankaLevel(int level)
    {
        UpdateItem();
    }
    public void UpdateItem()
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        for (int i = 0; i < info.listWord.Count; i++)
        {
            string strword = info.listWord[i];
            UILetterItem ui = (UILetterItem)GameObject.Instantiate(uiLetterItemPrefab);
            ui.transform.SetParent(this.transform);
            ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiLetterItemPrefab.gameObject, ui.gameObject);
            ui.indexRow = info.listPosY[i];
            ui.indexCol = info.listPosX[i];
            ui.index = i;
            ui.SetStatus(UILetterItem.Status.UNLOCK);
            ui.wordAnswer = strword;
            ui.UpdateItem(strword);
            listItem.Add(ui);
        }
        indexAnswer = 0;
        indexFillWord = info.listWordAnswer[indexAnswer];
        for (int i = 0; i < info.listWordAnswer.Count; i++)
        {
            int idx = info.listWordAnswer[i];
            UILetterItem ui = listItem[idx];
            if (idx == indexFillWord)
            {
                ui.SetStatus(UILetterItem.Status.LOCK_SEL);
            }
            else
            {
                ui.SetStatus(UILetterItem.Status.LOCK_UNSEL);
            }

        }

        LayOut();

    }


    public UILetterItem GetItem(int idxRow, int idxCol)
    {
        foreach (UILetterItem item in listItem)
        {
            if ((idxRow == item.indexRow) && (idxCol == item.indexCol))
            {
                return item;
            }
        }
        return null;
    }


    //判断答案是否正确
    public bool CheckAllAnswer()
    {
        bool isAllAnswer = true;
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        for (int i = 0; i < info.listWordAnswer.Count; i++)
        {
            int idx = info.listWordAnswer[i];
            UILetterItem ui = listItem[idx];
            if (ui.GetStatus() != UILetterItem.Status.RIGHT_ANSWER)
            {
                isAllAnswer = false;
                break;
            }
        }

        if (isAllAnswer)
        {
            //全部猜对 game win
            // OnGameWin();

        }
        else
        {
            //游戏失败
            //  OnGameFail();
        }
        return isAllAnswer;
    }


    public void OnAddWord(string word)
    {
        UILetterItem ui = listItem[indexFillWord];
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
        if (UILetterItem.Status.ERROR_ANSWER == ui.GetStatus())
        {
            //先字符退回
            if (iDelegate != null)
            {
                iDelegate.UIWordFillBoxDidBackWord(this, ui.wordDisplay);
            }
        }
        ui.UpdateItem(word);
        if (ui.wordAnswer == word)
        {
            ui.SetStatus(UILetterItem.Status.RIGHT_ANSWER);
            //显示下一个
            indexAnswer++;
            if (indexAnswer < info.listWordAnswer.Count)
            {
                indexFillWord = info.listWordAnswer[indexAnswer];
                UILetterItem uiNext = listItem[indexFillWord];
                uiNext.SetStatus(UILetterItem.Status.LOCK_SEL);
            }

        }
        else
        {
            ui.SetStatus(UILetterItem.Status.ERROR_ANSWER);
        }
        // foreach (UIWordItem item in listItem)
        // {
        //     if (Common.BlankString(item.wordDisplay))
        //     {
        //         item.UpdateTitle(word);
        //         wordNumCur++;
        //         break;
        //     }
        // }

        // if (IsWordFull())
        // {
        //     CheckAnswer();
        // }
    }

    public void OnClickItem()
    {
    }
}
