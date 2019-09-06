using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWin : UIViewPop, ISegmentDelegate
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";




    public UISegment uiSegment;
    public UITextView textView;
    public Text textTitle;
    public Image imageBg;
    public Image imageHead;
    public Button btnClose;

    public Button btnFriend;
    public Button btnNext;
    public Button btnAddLove;
    int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();

        //Common.SetButtonText(btnFriend, Language.main.GetString("STR_GameWin_BtnFriend"));
        Common.SetButtonText(btnNext, Language.main.GetString("STR_GameWin_BtnNext"), 0, false);
        //Common.SetButtonText(btnAddLove, Language.main.GetString("STR_GameWin_BtnAddLove"));

        string str = info.title;
        if (Common.BlankString(str))
        {
            str = LanguageManager.main.languageGame.GetString(info.id);
        }
        textTitle.text = str;

        textView.SetFontSize(80);
        textView.SetTextColor(new Color32(192, 90, 59, 255));

        InitSegment();
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


    public void InitSegment()
    {

        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;

        //简介
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_INTRO;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //原文
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_YUANWEN;
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
        //作者简介
        // {
        //     ItemInfo infoSeg = new ItemInfo();
        //     infoSeg.id = KEY_GAMEWIN_INFO_AUTHOR_INTRO;
        //     infoSeg.title = Language.main.GetString(infoSeg.id);
        //     uiSegment.AddItem(infoSeg);
        // }

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
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rctran = this.GetComponent<RectTransform>();
            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctran.sizeDelta = new Vector2(w, h);

        }

        textView.LayOut();
    }

    public void UpdateText(ItemInfo info)
    {
        CaiCaiLeItemInfo infoGuanka = GameGuankaParse.main.GetItemInfo();
        string str = "";
        //         public string author;
        // public string year;
        // public string style;
        // public string album;
        // public string intro;
        // public string translation;
        // public string appreciation;
        // public List<PoemContentInfo> listPoemContent;
        if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_POEM)
        {
            if (info.id == KEY_GAMEWIN_INFO_INTRO)
            {

                string strtmp = infoGuanka.author;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }

                string strAuthor = Language.main.GetString("STR_AUTHOR") + ":" + strtmp + "    ";



                strtmp = infoGuanka.album;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }
                string strAlbum = Language.main.GetString("STR_ALBUM") + ":" + strtmp + "\n";


                strtmp = infoGuanka.year;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }
                string strYear = Language.main.GetString("STR_YEAR") + ":" + strtmp + "    ";


                strtmp = infoGuanka.style;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }
                string strStyle = Language.main.GetString("STR_STYLE") + ":" + strtmp + "\n";



                str = strAuthor + strAlbum + strYear + strStyle + "\n" + infoGuanka.intro;
            }
            if (info.id == KEY_GAMEWIN_INFO_YUANWEN)
            {
                for (int i = 0; i < infoGuanka.listPoemContent.Count; i++)
                {
                    PoemContentInfo infoPoem = infoGuanka.listPoemContent[i];
                    str += infoPoem.content + "\n";
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
            {
                str = infoGuanka.translation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_TRANSLATION");
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_JIANSHUANG)
            {
                str = infoGuanka.appreciation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_JIANSHUANG");
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
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnFriend()
    {
    }
    public void OnClickBtnNext()
    {
        Close();
        LevelManager.main.GotoNextLevel();
    }
    public void OnClickBtnAddLove()
    {
    }
}
