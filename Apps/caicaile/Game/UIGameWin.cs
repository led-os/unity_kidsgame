using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWin : UIViewPop
{
    public UITextView textView;
    public Text textTitle;
    public Image imageBg;
    public Image imageHead;
    public Button btnClose;

    public Button btnFriend;
    public Button btnNext;
    public Button btnAddLove;
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
        str = GetText();
        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }

        textView.SetFontSize(80);
        textView.SetTextColor(new Color32(192, 90, 59, 255));
        textView.text = str;
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
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rctran = this.GetComponent<RectTransform>();
            w = sizeCanvas.x * 0.8f;
            h = rctran.rect.size.y * w / rctran.rect.size.x;
            rctran.sizeDelta = new Vector2(w, h);

        }

        textView.LayOut();
    }

    public string GetText()
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();
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
            string strPoem = "原文:";
            for (int i = 0; i < info.listPoemContent.Count; i++)
            {
                PoemContentInfo infoPoem = info.listPoemContent[i];
                strPoem += infoPoem.content + "\n";
            }
            string strAuthor = "诗人:" + info.author + "\n";
            string strAlbum = "出处:" + info.album + "\n";
            string strIntro = "简介:" + info.intro + "\n";
            string strTranslation = "译文:" + info.translation + "\n";
            string strAppreciation = "赏析:" + info.appreciation + "\n";

            str = strPoem + strAuthor + strAlbum + strIntro + strTranslation + strAppreciation;
        }

        return str;
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
