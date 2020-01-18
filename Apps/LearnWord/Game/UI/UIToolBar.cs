using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIToolBar : UIView
{
    public const string STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION = "keyname_viewalert_first_use_function";
    public const string STR_KEYNAME_VIEWALERT_SAVE = "STR_KEYNAME_VIEWALERT_SAVE";
    public const string STR_KEYNAME_VIEWALERT_DELETE = "STR_KEYNAME_VIEWALERT_DELETE";
    public const string STR_KEYNAME_VIEWALERT_SAVE_TIPS = "STR_KEYNAME_VIEWALERT_SAVE_TIPS";
    public UIGameXieHanzi uiGameXieHanzi;


    public Image imageGoldBg;
    public Text textGold;

    public Button btnBack;
    public Button btnModeAll;
    public Button btnModeOne;
    public Button btnModeNone;
    public Button btnSave;
    public Button btnDel;
    public Button btnColorInput;//任意颜色
    public Button btnLineSetting;
    public Button btnShare;
    public Button btnColorBoard;

    WordWriteMode writeModeCur;
    WordWriteMode writeModePre;//
    bool isShowBihuaImage = false;
    int indexBihua;
    int indexBihuaPoint;

    bool isFirstUseColorInput
    {
        get
        {
            return false;
            // if (Common.noad)
            // {
            //     return false;
            // }
            // if (!AppVersion.appCheckHasFinished)
            // {
            //     return false;
            // }
            // return Common.Int2Bool(PlayerPrefs.GetInt("KEY_STR_FIRST_USE_COLOR_INPUT", Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt("KEY_STR_FIRST_USE_COLOR_INPUT", Common.Bool2Int(value));
        }
    }
    public Color colorWord
    {
        get
        {
            return Common.RGBString2Color(PlayerPrefs.GetString("KEY_STR_COLOR_WORD", "255,0,0"));
        }
        set
        {

            PlayerPrefs.SetString("KEY_STR_COLOR_WORD", Common.Color2RGBString(value));
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ShowFirstUseAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_FIRST_USE_FUNCTION");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_FIRST_USE_FUNCTION");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_FIRST_USE_FUNCTION");
        string no = "no";
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION, OnUIViewAlertFinished);

    }


    public void ShowSaveAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_SAVE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_SAVE, OnUIViewAlertFinished);
    }

    public void ShowDeleteAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_DELETE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_DELETE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_DELETE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_DELETE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_DELETE, OnUIViewAlertFinished);
    }

    //返回保存提示
    public void ShowSaveTipsAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_SAVE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_SAVE_TIPS, OnUIViewAlertFinished);
    }



    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        //  if (STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION == alert.keyName)
        //  {
        //         if (isYes)
        //         {
        //             GameManager.GotoNextLevel();
        //         }
        //         else
        //         {
        //             OnClickBtnBack();
        //         }
        //  }

        if (STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION == alert.keyName)
        {
            if (isYes)
            {
                if (isFirstUseColorInput)
                {

                    //show ad video
                    AdKitCommon.main.ShowAdVideo();

                }
                else
                {
                    DoClickBtnColorInput();
                }
            }

        }

        if (STR_KEYNAME_VIEWALERT_SAVE == alert.keyName)
        {
            if (isYes)
            {
                DoBtnSave();
            }
        }

        if (STR_KEYNAME_VIEWALERT_DELETE == alert.keyName)
        {
            if (isYes)
            {
                DoBtnDelete();
            }
        }

        if (STR_KEYNAME_VIEWALERT_SAVE_TIPS == alert.keyName)
        {
            if (isYes)
            {
                DoBtnSave();
            }

            DoBtnBack();
        }


    }



    void DoBtnBack()
    {
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        NaviViewController navi = uiGameXieHanzi.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
        //uiGameXieHanzi.ShowAdInsert(1);
    }
    public void OnClickBtnBack()
    {
        if (UIGameXieHanzi.gameMode == GameXieHanzi.GAME_MODE_FREE_WRITE)
        {
            if ((!uiGameXieHanzi.gameXieHanzi.isHasSave) && (uiGameXieHanzi.gameXieHanzi.isHasPaint))
            {
                ShowSaveTipsAlert();
                return;
            }

        }

        DoBtnBack();

    }



    public void OnClickBtnBihuaAll()
    {
        indexBihua = 0;
        uiGameXieHanzi.gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteWithOneWord);
    }
    public void OnClickBtnBihuaOne()
    {
        indexBihua = 0;
        uiGameXieHanzi.gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteWithOneBihua);
    }
    public void OnClickBtnBihuaNone()
    {
        indexBihua = 0;
        uiGameXieHanzi.gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteWithNone);

        //ShowGameWin();
    }

    void DoClickBtnColorInput()
    {
        isFirstUseColorInput = false;
        uiGameXieHanzi.uiColorInput.UpdateInitColor(colorWord);
        uiGameXieHanzi.uiColorInput.gameObject.SetActive(!uiGameXieHanzi.uiColorInput.gameObject.activeSelf);
        uiGameXieHanzi.uiColorInput.ColorNow = colorWord;
        uiGameXieHanzi.uiColorInput.UpdateColorNow();
    }
    void DoBtnSave()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        info.id = "id_free_write_" + Common.GetCurrentTimeMs();
        string filePath = uiGameXieHanzi.GetSavePath();

        uiGameXieHanzi.gameXieHanzi.SaveImage(filePath);

        if (info == null)
        {
            return;
        }
        DBWord.mainFreeWrite.AddItem(info);
    }
    public void OnClickBtnSave()
    {
        if (uiGameXieHanzi.gameXieHanzi.isHasPaint)
        {
            ShowSaveAlert();
        }

    }
    public void DoBtnDelete()
    {
        uiGameXieHanzi.gameXieHanzi.ClearAll();
    }
    public void OnClickBtnDelete()
    {

        if (uiGameXieHanzi.gameXieHanzi.isHasPaint)
        {
            ShowDeleteAlert();
        }

    }
    public void OnClickBtnColorInput()
    {
        if (Application.isEditor)
        {
            DoClickBtnColorInput();
            return;
        }
        if (isFirstUseColorInput)
        {
            ShowFirstUseAlert();
        }
        else
        {
            DoClickBtnColorInput();
        }
    }

    public void OnClickBtnColorBoard()
    {
        uiGameXieHanzi.uiColorBoard.gameObject.SetActive(!uiGameXieHanzi.uiColorBoard.gameObject.activeSelf);
    }

    public void OnClickBtnLineSetting()
    {
        uiGameXieHanzi.uiLineSetting.gameObject.SetActive(true);
    }
    public void OnClickBtnShare()
    {

    }
}
