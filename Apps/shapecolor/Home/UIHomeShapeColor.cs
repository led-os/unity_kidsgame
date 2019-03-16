using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHomeShapeColor : UIHomeBase, IPopViewControllerDelegate
{
    public UILearnProgress uiLearnProgressPrefab;
    public Image imageGoldBg;
    public Text textGold;
    public Button btnShape;
    public Button btnColor;
    public Button btnShapeColor;
    public GameObject objLayoutBtn;

    List<Vector2> listBtnPos;
    float layoutBtnOffsetYNormal;
    // [SerializeField] protected GameObject objBtn;  
    void Awake()
    {
        listBtnPos = new List<Vector2>();
        imageBgName.gameObject.SetActive(false);
        //bg

        if (Common.isWeb)
        {
            string filePath = Application.streamingAssetsPath + "/" + AppRes.IMAGE_HOME_BG;
            StartCoroutine(WWWReadData(filePath));
        }
        else
        {
            TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        }
        string appname = Common.GetAppNameDisplay();
        // appname = "appname";
        string dirRoot = Application.temporaryCachePath;//webgl: /tmp
        Debug.Log("cache dirRoot=" + dirRoot + " appname=" + appname);

        // string str = Language.main.GetString(AppString.STR_SETTING);
        // Debug.Log("str STR_SETTING=" + str);
        TextName.text = appname;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            TTS.Speek(appname);
        }

    }
    // Use this for initialization
    void Start()
    {
        LayOut();
        OnUIDidFinish();
    }
    // Update is called once per frame
    void Update()
    {

    }
    Vector2 GetPosOfBtn(int idx)
    {
        float oft = 32;
        Vector2 pos = listBtnPos[idx];
        Vector2 size = this.frame.size;
        float w, h;
        w = btnShape.GetComponent<RectTransform>().rect.width;
        h = btnShape.GetComponent<RectTransform>().rect.height;
        //limit pos
        if ((pos.x - w / 2) < -size.x / 2)
        {
            pos.x = -size.x / 2 + oft + w / 2;
        }
        if ((pos.x + w / 2) > size.x / 2)
        {
            pos.x = size.x / 2 - oft - w / 2;
        }
        if ((pos.y - h / 2) < -size.y / 2)
        {
            pos.y = -size.y / 2 + oft + h / 2;
        }
        if ((pos.y + h / 2) > size.y / 2)
        {
            pos.y = size.y / 2 - oft - h / 2;
        }
        return pos;
    }
    public override void LayOut()
    {
        float x = 0, y = 0;
        Vector2 sizeCanvas = this.frame.size;
        float w, h;
        listBtnPos.Clear();
        w = btnShape.GetComponent<RectTransform>().rect.width;
        if (Device.isLandscape)
        {
            listBtnPos.Add(new Vector2(-w, w / 2));
            listBtnPos.Add(new Vector2(w * 1.2f, 0));
            listBtnPos.Add(new Vector2(0, -w / 2));
        }
        else
        {
            listBtnPos.Add(new Vector2(0, w * 1.2f));
            listBtnPos.Add(new Vector2(-w * 0.8f, -w * 0.2f));
            listBtnPos.Add(new Vector2(w / 2, -w));
        }


        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();

            int fontSize = TextName.fontSize;
            int r = fontSize / 2;
            w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 1.5f;
            if (!Device.isLandscape)
            {
                RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
                if ((w + r * 2 + topBarHeight * 2) > sizeCanvas.x)
                {
                    w = w / 2 + r * 2;
                    h = h * 2;
                }
            }

            rctran.sizeDelta = new Vector2(w, h);
            x = 0;
            y = (sizeCanvas.y - topBarHeight) / 4;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        //btns
        {
            RectTransform rctran = btnShape.GetComponent<RectTransform>();
            rctran.anchoredPosition = GetPosOfBtn(0);
        }
        {
            RectTransform rctran = btnColor.GetComponent<RectTransform>();
            rctran.anchoredPosition = GetPosOfBtn(1);
        }
        {
            RectTransform rctran = btnShapeColor.GetComponent<RectTransform>();
            rctran.anchoredPosition = GetPosOfBtn(2);
        }



        LayoutChildBase();
    }
    IEnumerator WWWReadData(string filePath)
    {
        // if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            byte[] data = www.bytes;
            Texture2D tex = LoadTexture.LoadFromData(data);
            TextureUtil.UpdateImageTexture(imageBg, tex, true, Vector4.zero);
            LayOut();

        }

    }

    void UpdateGold()
    {


    }

    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        //MainScene.isInMainUI = true;
    }
    void ShowLearnProgress()
    {
        // MainScene.isInMainUI = false;
        LearnProgressViewController.main.Show(null, this);
    }


    public void OnClickGold()
    {

    }
    public void OnClickBtnHistory()
    {

    }



    public void OnClickBtnShape()
    {
        GotoGameByMode(GameShapeColor.GAME_MODE_SHAPE);
    }
    public void OnClickBtnShapeColor()
    {
        GotoGameByMode(GameShapeColor.GAME_MODE_SHAPE_COLOR);
    }
    public void OnClickBtnColor()
    {
        GotoGameByMode(GameShapeColor.GAME_MODE_COLOR);
    }

    public void OnClickBtnBoard()
    {
        ShowLearnProgress();
    }

    void GotoGameByMode(int mode)
    {
        AudioPlay.main.PlayFile(AppCommon.AUDIO_BTN_CLICK);
        GameManager.gameMode = mode;
        GameManager.placeLevel = mode;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;


            int total = GameManager.placeTotal;
            if (Common.appKeyName == AppType.SHAPECOLOR)
            {
                total = 0;
            }
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
}
