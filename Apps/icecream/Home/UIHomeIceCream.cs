using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHomeIceCream : UIHomeBase
{

    public Button btnPlay;
    public Button btnFree; 
    public Button btnLanguae;
    public Image imageLogo;
    public Text textGold;
    void Awake()
    {
        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        TextureUtil.UpdateImageTexture(imageLogo, AppRes.IMAGE_HOME_LOGO, true);

        UpdateLanguage();
        UpdateBtnMusic();

    }
    // Use this for initialization
    void Start()
    {
        InitUI();

        //TrophyViewController.main.Show(null, null);

    }
    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }
    public void UpdateGold()
    {
        string str = Common.gold.ToString();
        textGold.text = str;
    }

    void UpdateLanguage()
    {
        TextureUtil.UpdateButtonTexture(btnFree, Language.main.IsChinese() ? AppRes.IMAGE_BTN_FREE_cn : AppRes.IMAGE_BTN_FREE_en, true);
        TextureUtil.UpdateButtonTexture(btnLanguae, Language.main.IsChinese() ? AppRes.IMAGE_BTN_LANGUAGE_cn : AppRes.IMAGE_BTN_LANGUAGE_en, true);
    }

    public void UpdateBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        TextureUtil.UpdateButtonTexture(btnMusic, ret ? AppRes.IMAGE_BtnMusicOn : AppRes.IMAGE_BtnMusicOff, true);
    }

    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;

        Vector2 sizeCanvas = this.frame.size;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
        //logo
        float ratio = 0.7f;
        w = this.frame.width;
        h = (this.frame.height / 2 - topBarHeight);
        RectTransform rctranLogo = imageLogo.GetComponent<RectTransform>();

        if (imageLogo.sprite != null)
        {
            float scale = Common.GetBestFitScale(imageLogo.sprite.texture.width, imageLogo.sprite.texture.height, w * ratio, h * ratio);
            imageLogo.gameObject.transform.localScale = new Vector3(scale, scale, 1f);
            x = 0;
            y = h / 2 - 80;
            rctranLogo.anchoredPosition = new Vector2(x, y);
        }
        // Debug.Log("Home this.frame="+this.frame.size+"canvas="+AppSceneBase.main.sizeCanvas);
        //btnPlay
        {
            float y_top = rctranLogo.anchoredPosition.y - rctranLogo.rect.height * imageLogo.transform.localScale.y / 2;
            float y_bottom = -this.frame.height / 2;
            RectTransform rctran = btnPlay.GetComponent<RectTransform>();
            x = 0;
            y = y_bottom + (y_top - y_bottom) / 2 + 40;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        LayoutChildBase();

    }

    void InitUI()
    {

        // string appname = Common.GetAppNameDisplay();
        // TextName.text = appname;
        // bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        // if (ret)
        // {
        //     TTS.Speek(appname);
        // }
        UpdateGold();

        LayOut();

        OnUIDidFinish();
    }

    public void GotoGame(string name)
    {

        GameViewController.gameType = name;
        GameViewController.dirRootPrefab = "AppCommon/Prefab/Game/" + GameViewController.gameType;

        UIViewController controller = GameViewController.main;
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            navi.Push(controller);

        }


    }

    public void OnClickBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        bool value = !ret;
        Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
        if (value)
        {
            AudioPlay.main.Play();
        }
        else
        {
            AudioPlay.main.Stop();
        }
        UpdateBtnMusic();
    }
    public void OnClickBtnShop()
    {
        StarViewController p = StarViewController.main;
        p.SetType(StarViewController.TYPE_STAR_BUY);
        p.Show(null, null);
    }

    public void OnClickBtnRestore()
    {
        StarViewController p = StarViewController.main;
        p.SetType(StarViewController.TYPE_STAR_RESTORE);
        p.Show(null, null);
    }

    public void OnClickBtnFree()
    {
    }
    public void OnClickBtnLanguage()
    {
        SystemLanguage lan = SystemLanguage.English;
        if (Language.main.IsChinese())
        {
            lan = SystemLanguage.English;

        }
        else
        {
            lan = SystemLanguage.Chinese;
        }

        Language.main.SetLanguage(lan);
        PlayerPrefs.SetInt(AppString.STR_KEY_LANGUAGE, (int)lan);
        UpdateLanguage();
    }
    public void OnClickBtnPlay()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            //navi.source = AppRes.SOURCE_NAVI_GUANKA; 
            int total = LevelManager.main.placeTotal;
            List<object> listItem = LevelManager.main.ParsePlaceList();
            if (total > 1)
            {
                navi.Push(PlaceViewController.main);
            }
            else
            {
                ItemInfo info = listItem[0] as ItemInfo;
                GotoGame(info.id);
            }
        }
    }



}
