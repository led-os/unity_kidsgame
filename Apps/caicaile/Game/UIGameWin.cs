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
    public AnimateButton btnClose;

    void Awake()
    {
        CaiCaiLeItemInfo info = GameGuankaParse.main.GetItemInfo();

        textTitle.text = info.title;
        string str = GetText();

        textView.text = str;
    }
    // Use this for initialization
    void Start()
    {
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
            w = Mathf.Min(sizeCanvas.x, sizeCanvas.y) * 0.8f;
            h = w;
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
        return str;
    }
    public void OnClickBtnClose()
    {
        Close();
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
