using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHomeSticker : UIHomeBase
{

    public GameObject objGoldBar;
    public Image imageGoldBg;
    public Text textGold;
    public Button btnPlay;
    // [SerializeField] protected GameObject objBtn;  

    float goldBaroffsetYNormal;
    void Awake()
    {
        if (objGoldBar != null)
        {
            //
            RectTransform rctran = objGoldBar.GetComponent<RectTransform>();
            goldBaroffsetYNormal = rctran.offsetMax.y;
            if (Config.main.isHaveShop)
            {
                objGoldBar.SetActive(true);
            }
            else
            {
                objGoldBar.SetActive(false);
            }
        }

        //bg
        // Texture2D tex = AppResImage.texMainBg;
        // if (tex)
        // {
        //     Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        //     imageBg.sprite = sp;
        //     RectTransform rctan = imageBg.GetComponent<RectTransform>();
        //     rctan.sizeDelta = new Vector2(tex.width, tex.height);
        // }
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
    }
    // Use this for initialization
    void Start()
    {


        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            TTS.main.Speak(appname);
        }



        UpdateGold();

        LayOut();

        OnUIDidFinish();
    }
    // Update is called once per frame
    void Update()
    {
       UpdateBase();
    }
    public override void LayOut()
    {
        {
            RectTransform rectTransform = imageBgName.GetComponent<RectTransform>();
            float w, h;
            int fontSize = TextName.fontSize;
            int r = fontSize / 2;
            w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 2;
            rectTransform.sizeDelta = new Vector2(w, h);
        }



        LayoutChildBase();
    }

    void UpdateGold()
    {
        if (imageGoldBg == null)
        {
            return;
        }
        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        textGold.text = str;
        int fontsize = textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = imageGoldBg.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;
        float oft = 0;
        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;
    }


    public void OnClickBtnPlay()
    {

        GameManager.gameMode = 0;

        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            int total = GameManager.placeTotal;
            if (total > 1)
            {
                navi.Push(PlaceViewController.main);
            }
            else
            {
                navi.Push(GuankaViewController.main);
            }
        }

    }
    public void OnClickBtnGameMode2()
    {

        GameManager.gameMode = UIGameSticker.GAME_MODE_RANDOM;

        GameManager.placeLevel = 0;
        //必须在placeLevel设置之后再设置gameLevel
        GameManager.gameLevel = 0;

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            navi.Push(GameViewController.main);
        }
    }

    public void OnClickGold()
    {

    }

}
