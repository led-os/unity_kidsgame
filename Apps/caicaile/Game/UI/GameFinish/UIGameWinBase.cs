using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinBase : UIViewPop
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

    public Button btnFriend;
    public Button btnNext;
    public Button btnAddLove;
    public GameObject objLayoutBtn;


    public int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

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

    public override void LayOut()
    {
    }


    public void UpdateLoveStatus()
    {
        CaiCaiLeItemInfo infoItem = GameLevelParse.main.GetItemInfo();
        string strBtn = "";
        if (LoveDB.main.IsItemExist(infoItem))
        {
            strBtn = Language.main.GetString("STR_IdiomDetail_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_IdiomDetail_ADD_LOVE");
        }

        Common.SetButtonText(btnAddLove, strBtn, 0, false);
    }

    public void OnClickBtnClose()
    {
        Close();
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnNext()
    {
        Close();
        LevelManager.main.GotoNextLevel();
    }
    public void OnClickBtnAddLove()
    {
        CaiCaiLeItemInfo infoItem = GameLevelParse.main.GetItemInfo();
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
