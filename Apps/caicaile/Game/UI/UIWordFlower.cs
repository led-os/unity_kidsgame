using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//成语接龙
/*
成语小秀才
https://www.taptap.com/app/167939
 */

public class UIWordFlower : UIWordContentBase, IUILetterItemDelegate
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
    void Awake()
    {
        base.Awake();
        lygrid = this.GetComponent<LayOutGrid>();
        listItem = new List<UILetterItem>();
        row = 4;
        col = 4;
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
                Vector2 pos = lygrid.GetItemPostion(item.gameObject, item.indexRow, item.indexCol);
                RectTransform rctran = item.GetComponent<RectTransform>();
                w = (rctranRoot.rect.width - (lygrid.space.x) * (col - 1)) / col;
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = pos;
                item.LayOut();
            }
        }


    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();
    }
    public void UpdateItem()
    {
        List<object> listPos = LevelParseIdiomFlower.main.listPosition;
        int idx_pos = Random.Range(0, listPos.Count);
        //idx_pos = 0;
        PositionInfo infoPos = listPos[idx_pos] as PositionInfo;

        lygrid.row = row;
        lygrid.col = col;
        int level = LevelManager.main.gameLevel;
        int idx = 0;
        for (int i = level * 4; i < (level + 1) * 4; i++)
        {
            CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[i] as CaiCaiLeItemInfo;
            for (int j = 0; j < 4; j++)
            {
                UILetterItem ui = (UILetterItem)GameObject.Instantiate(uiLetterItemPrefab);
                ui.transform.SetParent(this.transform);
                ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UIViewController.ClonePrefabRectTransform(uiLetterItemPrefab.gameObject, ui.gameObject);
                ui.iDelegate = this;
                RowColInfo infoRowCol = infoPos.listRowCol[idx];
                ui.indexRow = infoRowCol.row;
                ui.indexCol = infoRowCol.col;
                ui.index = idx;
                ui.isAnswerItem = false;
                ui.SetStatus(UILetterItem.Status.NORMAL);
                ui.UpdateItem(info.title.Substring(j, 1));
                listItem.Add(ui);
                idx++;
            }

        }

        LayOut();

    }

    public UILetterItem GetSelItem()
    {
        UILetterItem ui = listItem[indexFillWord];
        return ui;
    }

    public UILetterItem GetFistUnSelItem()
    {
        foreach (UILetterItem item in listItem)
        {
            if (item.GetStatus() == UILetterItem.Status.LOCK_UNSEL)
            {
                return item;
            }
        }
        return null;
    }
    public UILetterItem GetItem(int idx)
    {
        foreach (UILetterItem item in listItem)
        {
            if (idx == item.index)
            {
                return item;
            }
        }
        return null;
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
    public override bool CheckAllAnswerFinish()
    {
        bool isAllAnswer = true;
        // CaiCaiLeItemInfo info = infoItem as CaiCaiLeItemInfo;
        // for (int i = 0; i < info.listWordAnswer.Count; i++)
        // {
        //     int idx = info.listWordAnswer[i];
        //     UILetterItem ui = listItem[idx];
        //     // Debug.Log("CheckAllAnswer Status=" + ui.GetStatus() + " i=" + i);
        //     if (!IsItemRightAnswer(ui))
        //     {
        //         Debug.Log("CheckAllAnswer Status=" + ui.GetStatus() + " i=" + i);
        //         isAllAnswer = false;
        //         break;
        //     }
        // }

        // if (isAllAnswer)
        // {
        //     //全部猜对 game win
        //     // OnGameWin();

        // }
        // else
        // {
        //     //游戏失败
        //     //  OnGameFail();
        // }
        return isAllAnswer;
    }

    public override void OnAddWord(string word)
    {
        // UILetterItem ui = listItem[indexFillWord];
        // CaiCaiLeItemInfo info = infoItem as CaiCaiLeItemInfo;
        // if (UILetterItem.Status.ERROR_ANSWER == ui.GetStatus())
        // {
        //     //先字符退回
        //     if (iDelegate != null)
        //     {
        //         iDelegate.UIWordContentBaseDidBackWord(this, ui.wordDisplay);
        //     }
        // }
        // ui.UpdateItem(word);
        // if (ui.wordAnswer == word)
        // {
        //     ui.SetStatus(UILetterItem.Status.RIGHT_ANSWER);
        //     ScanItem(ui.indexRow, ui.indexCol);
        //     //显示下一个
        //     indexAnswer = GetNextFillWord(info);
        //     if ((indexAnswer < info.listWordAnswer.Count) && (indexAnswer >= 0))
        //     {
        //         indexFillWord = info.listWordAnswer[indexAnswer];
        //         UILetterItem uiNext = listItem[indexFillWord];
        //         uiNext.SetStatus(UILetterItem.Status.LOCK_SEL);
        //     }

        // }
        // else
        // {
        //     ui.SetStatus(UILetterItem.Status.ERROR_ANSWER);
        // }

    }

    public override void OnTips()
    {
        // CaiCaiLeItemInfo info = infoItem as CaiCaiLeItemInfo;
        // int idx = GetFirstUnFinishAnswer();
        // if (idx >= 0)
        // {
        //     indexAnswer = idx;
        //     if (indexAnswer < info.listWordAnswer.Count)
        //     {
        //         indexFillWord = info.listWordAnswer[indexAnswer];
        //         string strword = info.listWord[indexFillWord];
        //         OnAddWord(strword);
        //         if (iDelegate != null)
        //         {
        //             iDelegate.UIWordContentBaseDidTipsWord(this, strword);
        //         }
        //     }

        // }

    }

    public override void OnReset()
    {
        // int idx = 0;
        // indexAnswer = 0;
        // CaiCaiLeItemInfo info = infoItem as CaiCaiLeItemInfo;
        // indexFillWord = info.listWordAnswer[indexAnswer];
        // foreach (UILetterItem item in listItem)
        // {
        //     if (item.isAnswerItem)
        //     {
        //         if (IsItemRightAnswer(item) || (item.GetStatus() == UILetterItem.Status.ERROR_ANSWER))
        //         {
        //             if (idx == 0)
        //             {
        //                 item.SetStatus(UILetterItem.Status.LOCK_SEL);
        //             }
        //             else
        //             {

        //                 item.SetStatus(UILetterItem.Status.LOCK_UNSEL);
        //             }
        //             idx++;
        //         }

        //     }
        //     else
        //     {
        //         item.SetStatus(UILetterItem.Status.NORMAL);
        //     }
        // }
    }

    void UpdateSelItem(UILetterItem ui, CaiCaiLeItemInfo info)
    {
        //更新选中项目
        // foreach (UILetterItem item in listItem)
        // {
        //     if (item.GetStatus() == UILetterItem.Status.LOCK_SEL)
        //     {
        //         Debug.Log("OnUILetterItemDidClick unsel word=" + item.wordAnswer);
        //         item.SetStatus(UILetterItem.Status.LOCK_UNSEL);
        //     }
        // }
        // ui.SetStatus(UILetterItem.Status.LOCK_SEL);
        // int idx_answer = GetIndexAnswer(ui);
        // Debug.Log("OnUILetterItemDidClick idx_answer=" + idx_answer);
        // if (idx_answer >= 0)
        // {
        //     indexAnswer = idx_answer;
        //     indexFillWord = info.listWordAnswer[indexAnswer];
        // }
    }

    public void OnUILetterItemDidClick(UILetterItem ui)
    {
        // Debug.Log("OnUILetterItemDidClick status=" + ui.GetStatus());
        // CaiCaiLeItemInfo info = infoItem as CaiCaiLeItemInfo;
        // if (ui.GetStatus() == UILetterItem.Status.LOCK_UNSEL)
        // {
        //     //更新选中项目
        //     UpdateSelItem(ui, info);
        // }

        // if (ui.GetStatus() == UILetterItem.Status.ERROR_ANSWER)
        // {
        //     //回退并且选中 
        //     UpdateSelItem(ui, info);
        //     if (iDelegate != null)
        //     {
        //         iDelegate.UIWordContentBaseDidBackWord(this, ui.wordDisplay);
        //     }
        // }

    }
    public void OnClickItem()
    {
    }
}
