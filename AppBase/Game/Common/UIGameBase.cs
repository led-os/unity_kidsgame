using System.Collections;
using System.Collections.Generic;
using LitJson;
using Moonma.Share;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameBase : UIView
{

    public const int GAME_AD_INSERT_SHOW_STEP = 2;
    public const string STR_KEYNAME_VIEWALERT_USER_GUIDE = "keyname_viewalert_user_guide";
    public const string STR_KEYNAME_VIEWALERT_GAME_FINISH = "keyname_viewalert_game_finish";
    public const string STR_KEYNAME_VIEWALERT_GOLD = "keyname_viewalert_gold";
    public const string STR_KEYNAME_VIEWALERT_ADVIDEO_FAIL = "keyname_viewalert_advideo_fail";
    //public AudioClip audioClipBtn;
    public Button btnShare;
    public Button btnMusic;
    static public List<object> listGuanka;
    public List<object> listGuankaItemId;//image id
    static public Language languageGame;
    //static public int heightAdBanner;
    //static public float heightAdBannerWorld;
    public HttpRequest httpReqLanguage;
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
        GameManager.main.isShowGameAdInsert = false;
        // if ((GameManager.gameLevel != 0) && ((GameManager.gameLevel % _step) == 0))
        if ((GameManager.gameLevel % _step) == 0)
        {
            AdKitCommon.main.InitAdInsert();
            AdKitCommon.main.ShowAdInsert(100);
            GameManager.main.isShowGameAdInsert = true;
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
        Debug.Log("ParseGuanka UIGameBase");
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
        UpdateLanguage();
        AdKitCommon.main.callbackFinish = OnAdKitFinish;
    }
    public virtual void UpdatePlaceLevel(int level)
    {
    }


    public void ParseGuankaItemId(int count_one_group)
    {
        listGuankaItemId = new List<object>();

        listGuanka = new List<object>();
        ItemInfo infoPlace = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        string fileName = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName); //((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string type = (string)root["type"];
        string picRoot = Common.GAME_RES_DIR + "/image/" + type + "/";

        //search_items
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            info.pic = picRoot + info.id + ".png";
            listGuankaItemId.Add(info);
        }


        //让总数是count_one_group的整数倍
        int tmp = (listGuankaItemId.Count % count_one_group);
        if (tmp > 0)
        {
            for (int i = 0; i < (count_one_group - tmp); i++)
            {
                ItemInfo infoId = listGuankaItemId[i] as ItemInfo;
                ItemInfo info = new ItemInfo();
                info.id = infoId.id;
                info.pic = infoId.pic;
                listGuankaItemId.Add(info);
            }
        }

    }

    public virtual void PreLoadDataForWeb()
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

    public void UpdateLanguage()
    {
        ItemInfo info = GameManager.main.GetPlaceItemInfo(GameManager.placeLevel);
        string strlan = Common.GAME_RES_DIR + "/language/" + info.language + ".csv";
        languageGame = new Language();
        languageGame.Init(strlan);
        languageGame.SetLanguage(Language.main.GetLanguage());

    }
    public void UpdateBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (btnMusic != null)
        {
            TextureUtil.UpdateButtonTexture(btnMusic, ret ? AppRes.IMAGE_BtnMusicOn : AppRes.IMAGE_BtnMusicOff, false);
        }
    }

    public void OnGameWinBase()
    {
        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
    }

    public void OnGameFailBase()
    {
    }
    public void OnClickBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        bool value = !ret;
        Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
        if (value)
        {
            AudioPlay.main.PlayMusicBg();
        }
        else
        {
            AudioPlay.main.Stop();
        }
        UpdateBtnMusic();
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
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
    }


    public void OnAdKitFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        OnGameAdKitFinish(type, status, str);
    }

    public virtual void OnGameAdKitFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.BANNER)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                int w = 0;
                int h = 0;
                int idx = str.IndexOf(":");
                string strW = str.Substring(0, idx);
                int.TryParse(strW, out w);
                string strH = str.Substring(idx + 1);
                int.TryParse(strH, out h);
                Debug.Log("OnGameAdKitFinish AdBannerDidReceiveAd::w=" + w + " h=" + h);

                Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
                GameManager.main.heightAdScreen = h + Device.heightSystemHomeBar;
                GameManager.main.heightAdWorld = Common.ScreenToWorldHeight(mainCam, h);
                GameManager.main.heightAdCanvas = Common.ScreenToCanvasHeigt(sizeCanvas, h);
            }

            if (status == AdKitCommon.AdStatus.FAIL)
            {

            }

            LayOut();
        }
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


}
