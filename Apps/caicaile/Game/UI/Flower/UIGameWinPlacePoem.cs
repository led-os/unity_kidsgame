using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinPlacePoem : UIGameWinBase, ISegmentDelegate
{
    public const string KEY_GAMEWIN_ContentPinYin = "KEY_GAMEWIN_ContentPinYin";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";
    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";

    public UISegment uiSegment;
    public UITextView textView;
    public UIText textTitle;
    public UIText textDetail;
    public UIImage imageBg;
    public UIImage imageHead;
    public UIButton btnClose;

    public UIButton btnAdd;
    public GameObject objLayoutBtn;

    CaiCaiLeItemInfo infoItem;
    int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        textView.SetFontSize(80);
        textView.SetTextColor(GameRes.main.colorGameWinTextView);

        textTitle.color = GameRes.main.colorGameWinTitle;


        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;
        UpdateSegment();

    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateSegment()
    {


        //原文
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_YUANWEN;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        //原文拼音
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_ContentPinYin;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //翻译
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_TRANSLATION;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //赏析
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_JIANSHUANG;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        // 作者简介
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_AUTHOR_INTRO;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        if (uiSegment != null)
        {
            uiSegment.UpdateList();
        }
        uiSegment.Select(indexSegment, true);
    }
    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.8f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);
            base.LayOut();
        }
        float w_btns_landscape = 420;
        float space = 32f;
        // //textView
        // {
        //     RectTransform rctran = textView.GetComponent<RectTransform>();
        //     float oftTop = 0;
        //     float oftBottom = 0;
        //     float oftLeft = 0;
        //     float oftRight = 0;
        //     if (Device.isLandscape)
        //     {
        //         oftLeft = space;
        //         oftRight = w_btns_landscape + space;
        //         oftTop = 400;
        //         oftBottom = space;
        //     }
        //     else
        //     {
        //         oftLeft = space;
        //         oftRight = space;
        //         oftTop = 400;
        //         oftBottom = 200;
        //     }
        //     w = rctranRoot.rect.width - oftLeft - oftRight;
        //     h = rctranRoot.rect.height - oftTop - oftBottom;
        //     x = ((-rctranRoot.rect.width / 2 + oftLeft) + (rctranRoot.rect.width / 2 - oftRight)) / 2;
        //     y = ((-rctranRoot.rect.height / 2 + oftBottom) + (rctranRoot.rect.height / 2 - oftTop)) / 2;
        //     rctran.sizeDelta = new Vector2(w, h);
        //     rctran.anchoredPosition = new Vector2(x, y);
        //     textView.LayOut();
        // }

        //objLayoutBtn
        // {
        //     RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
        //     if (Device.isLandscape)
        //     {
        //         w = w_btns_landscape;
        //         h = rctranRoot.rect.height;
        //         y = 0;
        //         x = rctranRoot.rect.width / 2 - w / 2 - space;
        //     }
        //     else
        //     {
        //         w = rctranRoot.rect.width;
        //         h = 160;
        //         x = 0;
        //         y = -rctranRoot.rect.height / 2 + h / 2 + space;
        //     }
        //     rctran.sizeDelta = new Vector2(w, h);
        //     rctran.anchoredPosition = new Vector2(x, y);


        //     LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
        //     lg.enableHide = false;
        //     int btn_count = lg.GetChildCount(false);
        //     if (Device.isLandscape)
        //     {
        //         lg.row = btn_count;
        //         lg.col = 1;
        //     }
        //     else
        //     {
        //         lg.row = 1;
        //         lg.col = btn_count;
        //     }
        //     lg.LayOut();
        // }

        // imageHead.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void UpdateItem(CaiCaiLeItemInfo info)
    {


    }

    public override void UpdateItem(ItemInfo info)
    {
        infoItem = info as CaiCaiLeItemInfo;
        GameLevelParse.main.ParseItem(infoItem);
        Debug.Log("UpdateItem ParseItem infoItem.id=" + infoItem.id + " info.pinyin=" + infoItem.pinyin + " info.title=" + infoItem.title);

        textTitle.text = infoItem.infoIdiom.title;
        textDetail.text = infoItem.infoIdiom.author + "|" + infoItem.infoIdiom.year;

        uiSegment.Select(indexSegment, true);
        UpdateLoveStatus();
    }

    public void UpdateLoveStatus()
    {
        string strkey = "";
        if (DBLove.main.IsItemExist(infoItem.infoIdiom))
        {
            strkey = "STR_IdiomDetail_DELETE_LOVE";
        }
        else
        {
            strkey = "STR_IdiomDetail_ADD_LOVE";
        }
        btnAdd.textTitle.UpdateTextByKey(strkey);

    }
    public void UpdateText(ItemInfo info)
    {
        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();

        GameLevelParse.main.ParseItem(infoGuanka);
        string str = "";
        {
            if (info.id == KEY_GAMEWIN_ContentPinYin)
            {
                str = infoGuanka.infoIdiom.content_pinyin;
            }
            if (info.id == KEY_GAMEWIN_INFO_YUANWEN)
            {
                str = infoGuanka.infoIdiom.content;
            }
            if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
            {
                str = infoGuanka.infoIdiom.translation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_TRANSLATION");
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_JIANSHUANG)
            {
                str = infoGuanka.infoIdiom.appreciation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_JIANSHUANG");
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_AUTHOR_INTRO)
            {
                str = infoGuanka.infoIdiom.authorDetail;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN");
                }
            }

        }

        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }

    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        // UpdateSortList(item.index);
        UpdateText(item.infoItem);

    }
    public void OnClickBtnClose()
    {
        Close();
    }

    public void OnClickBtnAdd()
    {
        // Close();
        if (DBLove.main.IsItemExist(infoItem.infoIdiom))
        {
            DBLove.main.DeleteItem(infoItem.infoIdiom);
        }
        else
        {
            DBLove.main.AddItem(infoItem.infoIdiom);
        }
        UpdateLoveStatus();
    }
}
