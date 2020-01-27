using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moonma.AdKit.AdBanner;
using Moonma.AdKit.AdInsert;
using UnityEngine.EventSystems;
using LitJson;
using System;
// using UnityEngine.SceneManagement;
//sprite 描边：http://m.blog.csdn.net/article/details?id=57082556
public class UIGameSticker : UIGameBase, IGameDelegate
{
    static public bool isPauseGame = false;

    public const float GAME_ITEM_SCREEN_WIDTH = 256;//  
    public Text textTitle;
    //public Image imageBgLeft;
    public Image imageBgTopBar;
    //public UIViewAlert viewAlert;
    public GameObject objTopBar;
    GameSticker gameStickerPrefab;
    GameSticker gameSticker;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
        UpdateBtnMusic();
        LoadPrefab();
    }
    // Use this for initialization
    void Start()
    {

        languageGame = new Language();
        languageGame.Init(Common.GAME_RES_DIR + "/language/language.csv");
        languageGame.SetLanguage(Language.main.GetLanguage());

        //listTexTure = new List<Texture2D>();

        ShowTitle(false);

        //  ParseGuanka();
        PauseGame(false);


        UpdateGuankaLevel(LevelManager.main.gameLevel);

    }

    // Update is called once per frame
    void Update()
    {

        if (Device.isDeviceDidRotation)
        {
            // LayOutChild();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }


    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {

    }

    void LoadPrefab()
    {
        GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/GameSticker");
        if (obj != null)
        {
            gameStickerPrefab = obj.GetComponent<GameSticker>();

        }

    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AppSceneBase.main.ClearMainWorld();
        {
            gameSticker = (GameSticker)GameObject.Instantiate(gameStickerPrefab);
            AppSceneBase.main.AddObjToMainWorld(gameSticker.gameObject);
            UIViewController.ClonePrefabRectTransform(gameStickerPrefab.gameObject, gameSticker.gameObject);
            gameSticker.transform.localPosition = new Vector3(0f, 0f, -1f);
            gameSticker.infoGuankaItem = GameLevelParse.main.GetItemInfo();
            gameSticker.iDelegate = this;
            gameSticker.LoadGame();
        }

        OnUIDidFinish();
        // Invoke("OnUIDidFinish",1f);

    }


    public override void LayOut()
    {
        if (gameSticker != null)
        {
            gameSticker.LayOut();
        }
    }

    void ShowTitle(bool isShow)
    {
        textTitle.gameObject.SetActive(isShow);
        imageBgTopBar.gameObject.SetActive(isShow);
    }

    void UpdateTitle(StickerGameItem info)
    {
        if (info == null)
        {
            return;
        }
        string str = languageGame.GetString(info.id);
        textTitle.text = str;
        int fontsize = textTitle.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = imageBgTopBar.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;
        float oft = 0;
        sizeDelta.x = str_w + fontsize + oft * 2;
        rctran.sizeDelta = sizeDelta;

        TTS.main.Speak(str);
    }



    public void OnGameDidWin(GameBase g)
    {
        ShowGameWin();
    }
    public void OnGameDidFail(GameBase g)
    {

    }
    public void OnGameUpdateTitle(GameBase g, ItemInfo info, bool isshow)
    {
        ShowTitle(isshow);
        StickerGameItem infotmp = info as StickerGameItem;
        UpdateTitle(infotmp);
    }
    void ShowGameWin()
    {
        Debug.Log("ShowGameWin");
        OnGameWinBase2(true);
        if (GameManager.main.gameMode == GameSticker.GAME_MODE_RANDOM)
        {
            LevelManager.main.GotoNextLevel();
            gameSticker.GotoGameModeRandom();
            return;
        }
        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, "STR_KEYNAME_VIEWALERT", OnUIViewAlertFinished);
    }

    void PauseGame(bool isPause)
    {
        isPauseGame = isPause;
        if (isPauseGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }


    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            gameSticker.GotoGameGroup(gameSticker.gameGroupIndx);
        }
        else
        {
            OnClickBtnBack();
        }



    }

    public override void OnClickBtnBack()
    {
        base.OnClickBtnBack();
    }
    public void OnClickGameEndBtnPlay()
    {
        PauseGame(false);

    }



}
