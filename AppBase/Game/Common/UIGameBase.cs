using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameBase : UIView
{

    public const int GAME_AD_INSERT_SHOW_STEP = 3;
    public const string STR_KEYNAME_VIEWALERT_USER_GUIDE = "keyname_viewalert_user_guide";
    public const string STR_KEYNAME_VIEWALERT_GAME_FINISH = "keyname_viewalert_game_finish";
    public const string STR_KEYNAME_VIEWALERT_GOLD = "keyname_viewalert_gold";
    public const string STR_KEYNAME_VIEWALERT_ADVIDEO_FAIL = "keyname_viewalert_advideo_fail";
    //public AudioClip audioClipBtn;
    public Button btnShare;
    static public List<object> listGuanka;

    //public GameObject objSpriteBg;
    // public Sprite spriteBg;
    public List<Texture2D> listTexTure;
    static public Language languageGame;
    static public int heightAdBanner;
    static public float heightAdBannerWorld;

    private int _gameMode;


    public OnInitUIFinishDelegate callbackGameInitUIFinish { get; set; }

    //public Camera mainCamera;


    static public int gameMode;//已经通关 
    public void Init()
    {
        Debug.Log("UIGameBase Init");

        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
    }




    void ShowShare()
    {
        Debug.Log("gamebase showshare");
        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }
    public void ShowAdVideoFailAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_ADVIDEO_FAIL");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_ADVIDEO_FAIL");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_ADVIDEO_FAIL");
        string no = "no";
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_ADVIDEO_FAIL, null);

    }


    public void ShowParentGate(OnUIParentGateDidCloseDelegate callbackClose)
    {
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = callbackClose;
    }

    public void ShowAdInsert(int step)
    {
        int _step = step;
        if (_step <= 0)
        {
            _step = 1;
        }
        if ((GameManager.gameLevel != 0) && ((GameManager.gameLevel % _step) == 0))
        {
            AdKitCommon.main.ShowAdInsert(100);
        }
    }

    public virtual void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIGAME_SHARE_TITLE");
        string detail = Language.main.GetString("UIGAME_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }

    // public virtual int GetPlaceTotal()
    // {
    //     return 0;
    // } 
    public virtual int GetGuankaTotal()
    {
        return 0;
    }

    public virtual void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }
    public virtual int ParseGuanka()
    {
        return 0;
    }

    public virtual ItemInfo GetGuankaItemInfo(int idx)
    {
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ItemInfo info = listGuanka[idx] as ItemInfo;
        return info;
    }

    public virtual void UpdateGuankaLevel(int level)
    {
    }
    public virtual void UpdatePlaceLevel(int level)
    {
    }

    public void LayoutChildBase()
    {
        // if (objSpriteBg != null)
        // {
        //     SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
        //     Vector2 worldsize = Common.GetWorldSize(AppSceneBase.main.mainCamera);
        //     float w = render.size.x;//rectTransform.rect.width;
        //     float h = render.size.y;//rectTransform.rect.height;
        //     float scalex = worldsize.x / w;
        //     float scaley = worldsize.y / h;
        //     float scale = Mathf.Max(scalex, scaley);
        //     objSpriteBg.transform.localScale = new Vector3(scale, scale, 1.0f);

        // }
        AppSceneBase.main.LayoutChild();
    }
    public void OnClickBtnShare()
    {
        ShowShare();
    }


    public virtual void OnClickBtnBack()
    {
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }
        Debug.Log("GameBase:OnClickBtnBack");
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public virtual void AdBannerDidReceiveAd(int w, int h)
    {

    }

    public virtual void AdVideoDidFail(string str)
    {

    }

    public virtual void AdVideoDidStart(string str)
    {

    }
    public virtual void AdVideoDidFinish(string str)
    {

    }

    #region TTS
    public virtual void TTSSpeechDidStart(string str)
    {

    }
    public virtual void TTSSpeechDidFinish(string str)
    {
        Debug.Log("GameBase TTSSpeechDidFinish ");
    }

    public void PlaySoundFromResource(string file)
    {
        GameObject audioPlayer = GameObject.Find("AudioPlayer");
        if (audioPlayer != null)
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            AudioClip audioClip = AudioCache.main.Load(file);
            audioSource.PlayOneShot(audioClip);
        }
    }

    #endregion
}
