using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//成语接龙
/* 
 */

public class UIWordPlacePoem : UIWordContentBase, IUIItemFlowerDelegate
{
    public UIImage imageBg;
    public UIText textTitle;
    public GameObject objTopBar;
    public GameObject objWord;
    public GameObject objBtnBar;
    public Button btnShare;
    public UIItemFlower uiItemFlowerPrefab;
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;

    List<UIItemFlower> listItemSel;
    LayOutGrid lygrid;
    int indexFillWord;
    int indexAnswer;
    bool isTouchSel;
    UIItemFlower itemTouchSel0;
    UIItemFlower itemTouchSel1;
    UIItemFlower itemTouchSelPre;

    string strAnswer;
    void Awake()
    {
        base.Awake();
        lygrid = objWord.GetComponent<LayOutGrid>();
        listItem = new List<UIItemFlower>();
        listItemSel = new List<UIItemFlower>();
        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
        row = 4;
        col = 4;
        lygrid.row = row;
        lygrid.col = col;
        lygrid.enableLayout = false;
        lygrid.dispLayVertical = LayOutBase.DispLayVertical.TOP_TO_BOTTOM;
        isTouchSel = false;
        indexAnswer = 0;
        UITouchEventWithMove ev = objWord.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnItemTouchEvent;
        objWord.transform.SetAsLastSibling();

        itemTouchSel0 = null;
        itemTouchSel1 = null;
        itemTouchSelPre = null;
    }

    // Use this for initialization
    void Start()
    {
        LayOut();
        // if (iDelegate != null)
        // {
        //     iDelegate.UIWordContentBaseDidGameFinish(this, false);
        // }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
        RectTransform rctranRoot = objWord.GetComponent<RectTransform>();
        Debug.Log("rctranRoot w=" + rctranRoot.rect.width + " h=" + rctranRoot.rect.height);

        if (lygrid != null)
        {
            //lygrid.LayOut();
            foreach (UIItemFlower item in listItem)
            {

                RectTransform rctran = item.GetComponent<RectTransform>();
                w = (rctranRoot.rect.width - (lygrid.space.x) * (col + 1)) / col;
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                item.SetFontSize((int)(w * 0.7f));
                Vector2 pos = lygrid.GetItemPostion(item.gameObject, item.indexRow, item.indexCol);
                rctran.anchoredPosition = pos;
                item.LayOut();
            }
        }


    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();

        // if (iDelegate != null)
        // {
        //     iDelegate.UIWordContentBaseDidGameFinish(this, false);
        // }
    }

    public UIItemFlower GetUnLockPlaceItem(int idx)
    {
        UIItemFlower ret = null;
        List<UIItemFlower> listOther = new List<UIItemFlower>();
        foreach (UIItemFlower item in listItem)
        {
            if ((!item.isHavePlaced) && (idx != item.index) && (item.status != UIItemFlower.Status.LOCK))
            {
                listOther.Add(item);
            }
        }
        if (listOther.Count > 0)
        {
            int rdm = Random.Range(0, listOther.Count);
            ret = listOther[rdm];
        }
        return ret;
    }

    public List<UIItemFlower> GetAllUnLock()
    {
        List<UIItemFlower> listOther = new List<UIItemFlower>();
        foreach (UIItemFlower item in listItem)
        {
            if (item.status != UIItemFlower.Status.LOCK)
            {
                listOther.Add(item);
            }
        }
        return listOther;
    }

    //交换位置
    public void SwapItem(UIItemFlower item0, UIItemFlower item1)
    {
        int row0 = item0.indexRow;
        int row1 = item1.indexRow;
        int col0 = item0.indexCol;
        int col1 = item1.indexCol;
        item0.indexRow = row1;
        item0.indexCol = col1;

        item1.indexRow = row0;
        item1.indexCol = col0;

    }

    // 随机排列
    public void RandomPlace()
    {
        List<UIItemFlower> listUnlock = GetAllUnLock();
        int[] indexList = Common.RandomIndex(listUnlock.Count, listUnlock.Count);
        int idx = 0;
        foreach (UIItemFlower item in listItem)
        {
            if ((!item.isHavePlaced) && (item.status != UIItemFlower.Status.LOCK))
            {
                UIItemFlower itemother = GetUnLockPlaceItem(item.index);
                if (itemother != null)
                {
                    SwapItem(item, itemother);
                    item.isHavePlaced = true;
                }

                idx++;
            }
        }

    }

    public void UpdateItem()
    {
        int level = LevelManager.main.gameLevel;
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
        PoemContentInfo infoPoem = info.listPoemContent[0];
        string strIdiom = infoPoem.content;
        row = info.listPoemContent.Count;
        col = strIdiom.Length;
        lygrid.row = row;
        lygrid.col = col;

        int idx = 0;
        strAnswer = "";
        int max = row * col / 2 - 2;
        int min = 2;
        int lockcount = max - min;// 
        int numlock = (max - level % lockcount) * 2;//Random.Range(min, max);
        int[] indexLock = Common.RandomIndex(row * col, numlock);


        int[] indexRow = Common.RandomIndex(info.listPoemContent.Count, info.listPoemContent.Count);

        for (int i = 0; i < info.listPoemContent.Count; i++)
        {
            infoPoem = info.listPoemContent[i];
            strIdiom = infoPoem.content;
            Debug.Log("conent i=:" + i + ":" + strIdiom);
            strAnswer += strIdiom;
            for (int j = 0; j < strIdiom.Length; j++)
            {
                UIItemFlower ui = (UIItemFlower)GameObject.Instantiate(uiItemFlowerPrefab);
                ui.transform.SetParent(objWord.transform);
                ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UIViewController.ClonePrefabRectTransform(uiItemFlowerPrefab.gameObject, ui.gameObject);
                ui.iDelegate = this;
                ui.indexRow = row - 1 - i;
                ui.indexCol = j;
                ui.index = idx;
                ui.isAnswerItem = false;

                bool islock = false;
                foreach (int idxlock in indexLock)
                {
                    if (idx == idxlock)
                    {
                        islock = true;
                    }
                }
                int rdm = j;
                if (islock)
                {
                    ui.status = UIItemFlower.Status.LOCK;
                    ui.isHavePlaced = true;
                }
                else
                {
                    ui.status = UIItemFlower.Status.NORMAL;
                    ui.isHavePlaced = false;
                }

                ui.UpdateItem(strIdiom.Substring(j, 1));

                listItem.Add(ui);
                idx++;
            }

        }

        RandomPlace();
        LayOut();

    }

    //是否
    bool IsFirstSelect()
    {
        bool ret = false;
        if (itemTouchSel0 == null)
        {
            return false;
        }
        if (itemTouchSel0.status == UIItemFlower.Status.SELECT)
        {
            return true;
        }
        return ret;
    }
    bool IsBothSelect()
    {
        bool ret = false;
        if ((itemTouchSel0 == null) || (itemTouchSel1 == null))
        {
            return false;
        }
        if ((itemTouchSel0.status == UIItemFlower.Status.SELECT) && (itemTouchSel1.status == UIItemFlower.Status.SELECT))
        {
            return true;
        }
        return ret;
    }

    void RunSelectMoveAnimate()
    {
        Debug.Log("RunSelectMoveAnimate");
        float duration = 1f;
        Vector2 pos0 = itemTouchSel0.transform.position;
        Vector2 pos1 = itemTouchSel1.transform.position;
        itemTouchSel0.transform.DOMove(pos1, duration).OnComplete(() =>
                          {

                          });

        itemTouchSel1.transform.DOMove(pos0, duration).OnComplete(() =>
      {
          CheckSelect();
      });
    }
    void CheckSelect()
    {
        // itemTouchSel0.status = UIItemFlower.Status.NORMAL;
        // itemTouchSel1.status = UIItemFlower.Status.NORMAL; 
        SwapItem(itemTouchSel0, itemTouchSel1);


        string answer0 = GetAnswerOfItem(itemTouchSel0);
        string answer1 = GetAnswerOfItem(itemTouchSel1);
        if (answer0 == itemTouchSel0.word)
        {
            itemTouchSel0.status = UIItemFlower.Status.LOCK;
        }
        else
        {
            itemTouchSel0.status = UIItemFlower.Status.NORMAL;
        }
        if (answer1 == itemTouchSel1.word)
        {
            itemTouchSel1.status = UIItemFlower.Status.LOCK;
        }
        else
        {
            itemTouchSel1.status = UIItemFlower.Status.NORMAL;
        }

        CheckAllAnswerFinish();

    }

    string GetAnswerOfItem(UIItemFlower item)
    {
        int idx = (row - 1 - item.indexRow) * col + item.indexCol;
        return strAnswer.Substring(idx, 1);
    }


    public void OnItemTouchEvent(UITouchEvent ev, PointerEventData eventData, int st)
    {

        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 posScreen = eventData.position;
        Debug.Log("OnItemTouchEvent status=" + st);
        if (st == UITouchEvent.STATUS_TOUCH_DOWN)
        {

            listItemSel.Clear();
            //清空
            textTitle.text = "";
        }
        if (st == UITouchEvent.STATUS_TOUCH_MOVE)
        {
        }
        if (st == UITouchEvent.STATUS_TOUCH_UP)
        {

            foreach (UIItemFlower item in listItem)
            {
                RectTransform rctran = item.GetComponent<RectTransform>();
                float ratio = 1.0f;
                //屏幕坐标
                w = Common.CanvasToScreenWidth(sizeCanvas, rctran.rect.width * ratio);
                h = Common.CanvasToScreenWidth(sizeCanvas, rctran.rect.height * ratio);
                x = item.transform.position.x;
                y = item.transform.position.y;
                // Vector2 posTouch = mainCam.ScreenToWorldPoint(Common.GetInputPosition());
                Rect rc = new Rect(x - w / 2, y - h / 2, w, h);
                if (rc.Contains(posScreen) && item.gameObject.activeSelf)
                {
                    Debug.Log("OnUIItemFlowerTouchUp item.status=" + item.status);
                    //选中  
                    if (item.status != UIItemFlower.Status.LOCK)
                    {
                        item.status = UIItemFlower.Status.SELECT;
                        if (IsFirstSelect())
                        {
                            Debug.Log("OnUIItemFlowerTouchUp itemTouchSel1");
                            itemTouchSel1 = item;
                            //执行交换
                            RunSelectMoveAnimate();
                        }
                        else if (!IsBothSelect())
                        {
                            Debug.Log("OnUIItemFlowerTouchUp itemTouchSel0");
                            itemTouchSel0 = item;
                        }
                        else
                        {
                            itemTouchSel0.status = UIItemFlower.Status.NORMAL;
                            itemTouchSel1.status = UIItemFlower.Status.NORMAL;
                        }

                        item.transform.SetAsLastSibling();
                        OnUIItemFlowerTouchDown(item);
                        itemTouchSelPre = item;
                        break;
                    }
                }
            }

        }

    }

    public UIItemFlower GetSelItem()
    {
        UIItemFlower ui = listItem[indexFillWord];
        return ui;
    }

    public UIItemFlower GetItem(int idx)
    {
        foreach (UIItemFlower item in listItem)
        {
            if (idx == item.index)
            {
                return item;
            }
        }
        return null;
    }
    public UIItemFlower GetItem(int idxRow, int idxCol)
    {
        foreach (UIItemFlower item in listItem)
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

        foreach (UIItemFlower item in listItem)
        {
            if (item.status != UIItemFlower.Status.LOCK)
            {
                isAllAnswer = false;
            }
        }

        Debug.Log("CheckAllAnswerFinish isAllAnswer=" + isAllAnswer);

        if (isAllAnswer)
        {
            //全部猜对 game win
            if (iDelegate != null)
            {
                iDelegate.UIWordContentBaseDidGameFinish(this, false);
            }

        }
        else
        {
            //游戏失败
            //  OnGameFail();
        }
        return isAllAnswer;
    }

    public override void OnAddWord(string word)
    {


    }

    //提示答案需要先选中一个位置
    public override void OnTips()
    {


    }

    public override void OnReset()
    {

    }

    public void OnUIItemFlowerTouchDown(UIItemFlower ui)
    {
    }
    public void OnUIItemFlowerTouchMove(UIItemFlower ui)
    {
        Debug.Log("OnUIItemFlowerTouchMove ui.word=" + ui.word + " textTitle=" + textTitle.text);


    }
    public void OnUIItemFlowerTouchUp(UIItemFlower ui)
    {


        {
            textTitle.text = "";
            foreach (UIItemFlower item in listItemSel)
            {
                item.status = UIItemFlower.Status.NORMAL;
            }
            if (iDelegate != null)
            {
                iDelegate.UIWordContentBaseDidGameFinish(this, true);
            }

        }

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GOLD == alert.keyName)
        {
            if (isYes)
            {
                ShowShop();
            }
        }



    }

    public void OnNotEnoughGold()
    {

        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_NOT_ENOUGH_GOLD);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NOT_ENOUGH_GOLD);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_NOT_ENOUGH_GOLD);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_NOT_ENOUGH_GOLD);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GOLD, OnUIViewAlertFinished);

    }

    public void OnClickBtnHowPlay()
    {
        HowPlayFlowerViewController.main.Show(null, null);
    }
    public void OnClickBtnInfo()
    {
        GameInfoViewController.main.Show(null, null);
    }

    public void OnClickBtnTips()
    {

        if (Common.gold <= 0)
        {
            OnNotEnoughGold();
            return;
        }
        Common.gold--;
        if (Common.gold < 0)
        {
            Common.gold = 0;
        }
        UpdateGold();
        OnTips();

    }

    public void OnClickBtnTitle()
    {

    }
    public void OnClickBtnTranslation()
    {

    }

    //观看视频看原文
    public void OnClickBtnVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }
    public void OnClickBtnRetry()
    {
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnShare()
    {
    }

}
