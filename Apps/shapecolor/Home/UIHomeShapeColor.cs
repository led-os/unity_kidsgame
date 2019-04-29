using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
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
    List<object> listBtns;
    int indexAction;
    float timeAction;
    bool isActionFinish;

    // [SerializeField] protected GameObject objBtn;  
    void Awake()
    {
        listBtnPos = new List<Vector2>();
        listBtns = new List<object>();
        listBtns.Add(btnShape);
        listBtns.Add(btnColor);
        listBtns.Add(btnShapeColor);
        timeAction = 0.3f;
        indexAction = 0;
        isActionFinish = false;
        imageBgName.gameObject.SetActive(false);
        //bg

        if (Common.isWeb)
        {
            string filePath = Application.streamingAssetsPath + "/" + AppRes.IMAGE_HOME_BG;
            StartCoroutine(WWWReadData(filePath));
        }
        else
        {
            TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
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
            TTS.main.Speak(appname);
        }

        InitBtnPos();
    }
    // Use this for initialization
    void Start()
    {
        RunActionBtn();
        LayOut();
        OnUIDidFinish();

        // AudioPlay.main.PlayUrl("https://cdn.feilaib.top/img/sounds/bg.mp3");

        // AudioPlay.main.PlayUrl("https://tsn.baidu.com/text2audio?&lan=zh&cuid=moon&ctp=1&tok=24.6bcdf44420e6778fdfdfe2f417f0b76a.2592000.1557112977.282335-15699370&tex=%e7%ba%a2%e8%89%b2%e7%9a%84%e4%b8%89%e8%a7%92%e5%bd%a2");

        //   StartCoroutine(LoadMusic("F:/1.mp3"));
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBase();

    }
    public override void LayOut()
    {
        float x = 0, y = 0;
        Vector2 sizeCanvas = this.frame.size;
        float w, h;

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
        // {
        //     RectTransform rctran = btnShape.GetComponent<RectTransform>();
        //     rctran.anchoredPosition = GetPosOfBtn(0);
        // }
        // {
        //     RectTransform rctran = btnColor.GetComponent<RectTransform>();
        //     rctran.anchoredPosition = GetPosOfBtn(1);
        // }
        // {
        //     RectTransform rctran = btnShapeColor.GetComponent<RectTransform>();
        //     rctran.anchoredPosition = GetPosOfBtn(2);
        // }



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
            TextureUtil.UpdateRawImageTexture(imageBg, tex, true, Vector4.zero);
            LayOut();

        }

    }


    private IEnumerator LoadMusic(string filepath)
    {
        filepath = "file://" + filepath;
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                // use audio clip
                //  audioSource.clip = clip;
                AudioPlay.main.PlayAudioClip(clip);
            }
        }
    }

    void UpdateGold()
    {


    }
    Vector2 GetPosOfBtn(Button btn, int idx)
    {
        return listBtnPos[idx];
    }

    void InitBtnPos()
    {
        float x, y, w, h;
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

        Vector2 size = AppSceneBase.main.sizeCanvas;
        for (int i = 0; i < this.listBtns.Count; i++)
        {
            Button btn = this.listBtns[i] as Button;
            RectTransform rctran = btn.GetComponent<RectTransform>();
            h = rctran.rect.height;
            Vector2 pt = this.GetPosOfBtn(btn, i);
            x = pt.x;
            y = -size.y / 2 - h;
            //y = 0; 
            rctran.anchoredPosition = new Vector2(x, y);
        }

    }
    void RunActionBtn()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction;
        // var size = this.node.getContentSize();
        // var x_start, y_start, x_end, y_end, w, h;
        Button btn = listBtns[indexAction] as Button;

        Vector2 pt = this.GetPosOfBtn(btn, this.indexAction);
        // x_end = pt.x;
        // y_end = pt.y;


        // cc.Debug.Log("RunActionBtn:pt=" + pt + " idx=" + this.indexAction);

        // var action = cc.moveTo(duration, x_end, y_end).easing(cc.easeOut(3.0));


        RectTransform rctran = btn.GetComponent<RectTransform>();
        float z = btn.transform.localPosition.z;
        Vector3 toPos = new Vector3(pt.x, pt.y, z);
        rctran.DOLocalMove(toPos, duration).OnComplete(() =>
                  {
                      if (this.indexAction < this.listBtns.Count)
                      {
                          this.RunActionBtn();
                          this.indexAction++;
                      }
                      else
                      {
                          this.isActionFinish = true;
                          this.RunActionUpDown();
                      }
                  });
    }

    //上下晃动动画
    void RunActionUpDown()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = this.timeAction * 4;
        Vector2 size = AppSceneBase.main.sizeCanvas;
        float w, h;
        // // x_end = pt.x;
        // // y_end = pt.y;

        for (int i = 0; i < this.listBtns.Count; i++)
        {
            Button btn = this.listBtns[i] as Button;
            RectTransform rctran = btn.GetComponent<RectTransform>();
            h = rctran.rect.height;
            float y_step = h / 10;
            Vector2 pt = this.GetPosOfBtn(btn, i);
            // var actionUp = cc.moveBy(duration, 0, y_step);
            // var actionDown = cc.moveBy(duration, 0, -y_step);
            //  var time = cc.delayTime(0.5 * i);
            //   var seq = cc.sequence([time, actionUp, actionUp.reverse(), actionDown, actionDown.reverse()]);
            //  btn.node.runAction(seq.repeatForever());
            float z = btn.transform.localPosition.z;

            Vector3 posNormal = new Vector3(pt.x, pt.y, z);
            Vector3 toPos = new Vector3(pt.x, pt.y + y_step, z);
            Sequence seq = DOTween.Sequence();
            //actionUp
            Tweener acUp = rctran.DOLocalMove(toPos, duration);

            //normal
            Tweener acNormal = rctran.DOLocalMove(posNormal, duration);
            Tweener acNormal2 = rctran.DOLocalMove(posNormal, duration);
            //actionDown
            toPos = new Vector3(pt.x, pt.y - y_step, z);
            Tweener acDown = rctran.DOLocalMove(toPos, duration);
            float time = 0.5f * i;
            seq.AppendInterval(time).Append(acUp).Append(acNormal).Append(acDown).Append(acNormal2).SetLoops(-1);

        }

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
        if (!isActionFinish)
        {
            return;
        }
        AudioPlay.main.PlayFile(AppCommon.AUDIO_BTN_CLICK);
        GameManager.gameMode = mode;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
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
}
