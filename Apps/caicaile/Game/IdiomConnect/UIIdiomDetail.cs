using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIIdiomDetail : UIViewPop, ISegmentDelegate
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";


    public UISegment uiSegment;
    public UITextView textView;
    public Text textTitle;
    public Text textPinyin;
    public Image imageBg;
    public RawImage imageHead;
    public Button btnClose;

    public Button btnAdd;
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
        textPinyin.color = GameRes.main.colorGameWinTitle;


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

        //翻译
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_TRANSLATION;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        //出处
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_ALBUM;
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

        }
        float w_btns_landscape = 420;
        float space = 32f;
        //textView
        {
            RectTransform rctran = textView.GetComponent<RectTransform>();
            float oftTop = 0;
            float oftBottom = 0;
            float oftLeft = 0;
            float oftRight = 0;
            if (Device.isLandscape)
            {
                oftLeft = space;
                oftRight = w_btns_landscape + space;
                oftTop = 400;
                oftBottom = space;
            }
            else
            {
                oftLeft = space;
                oftRight = space;
                oftTop = 400;
                oftBottom = 200;
            }
            w = rctranRoot.rect.width - oftLeft - oftRight;
            h = rctranRoot.rect.height - oftTop - oftBottom;
            x = ((-rctranRoot.rect.width / 2 + oftLeft) + (rctranRoot.rect.width / 2 - oftRight)) / 2;
            y = ((-rctranRoot.rect.height / 2 + oftBottom) + (rctranRoot.rect.height / 2 - oftTop)) / 2;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
            textView.LayOut();
        }

        //objLayoutBtn
        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                w = w_btns_landscape;
                h = rctranRoot.rect.height;
                y = 0;
                x = rctranRoot.rect.width / 2 - w / 2 - space;
            }
            else
            {
                w = rctranRoot.rect.width;
                h = 160;
                x = 0;
                y = -rctranRoot.rect.height / 2 + h / 2 + space;
            }
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);


            LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;
            int btn_count = lg.GetChildCount(false);
            if (Device.isLandscape)
            {
                lg.row = btn_count;
                lg.col = 1;
            }
            else
            {
                lg.row = 1;
                lg.col = btn_count;
            }
            lg.LayOut();
        }

        // imageHead.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void UpdateItem(CaiCaiLeItemInfo info)
    {
        infoItem = info;
        GameGuankaParse.main.ParseIdiomItem(info);
        string str = info.title;
        if (Common.BlankString(str))
        {
            str = LanguageManager.main.languageGame.GetString(info.id);
        }
        textTitle.text = str;
        textPinyin.text = info.pinyin;
        uiSegment.Select(indexSegment, true);
        UpdateLoveStatus();

    }

    public void UpdateLoveStatus()
    {
        string strBtn = "";
        if (LoveDB.main.IsItemExist(infoItem))
        {
            strBtn = Language.main.GetString("STR_IdiomDetail_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_IdiomDetail_ADD_LOVE");
        }

        Common.SetButtonText(btnAdd, strBtn, 0, false);
    }


    public void UpdateText(ItemInfo info)
    {
        string str = "";
        if (infoItem == null)
        {
            return;
        }
        if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
        {
            str = infoItem.translation;
        }

        if (info.id == KEY_GAMEWIN_INFO_ALBUM)
        {
            str = infoItem.album;
        }

        if (Common.BlankString(str))
        {
            // str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
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
        if (LoveDB.main.IsItemExist(infoItem))
        {
            LoveDB.main.DeleteItem(infoItem);
        }
        else
        {
            LoveDB.main.AddItem(infoItem);
        }
        UpdateLoveStatus();
    }
}
