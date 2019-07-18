using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeCaiCaiLe : UIHomeBase
{
    float timeAction;
    bool isActionFinish;
    public LayOutGrid layoutBtn;
    public ActionHomeBtn actionBtnPlay;
    public ActionHomeBtn actionBtnLearn;
    void Awake()
    {
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        timeAction = 0.3f;
        isActionFinish = false;
        layoutBtn.enableLayout = false;


        actionBtnPlay.ptNormal = layoutBtn.GetItemPostion(0, 0);

        actionBtnLearn.ptNormal = layoutBtn.GetItemPostion(0, 1);
        actionBtnLearn.gameObject.SetActive(Config.main.APP_FOR_KIDS);
        UpdateLayoutBtn();
    }

    // Use this for initialization
    void Start()
    {
        isActionFinish = false;
        RunActionImageName();
        actionBtnPlay.RunAction();
        actionBtnLearn.RunAction();
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }

    public Vector4 GetPosOfImageName()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();

            int fontSize = TextName.fontSize;
            int r = fontSize / 2;
            w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 1.5f;
            if (!Device.isLandscape)
            {
                h = fontSize * 2;
                if ((w + r * 2) > sizeCanvas.x)
                {
                    //显示成两行文字
                    w = w / 2 + r * 2;
                    h = h * 2;
                    // RectTransform rctranText = TextName.GetComponent<RectTransform>();
                    // float w_text = rctranText.sizeDelta.x;
                    // rctranText.sizeDelta = new Vector2(w_text, h);
                }
            }


            x = 0;
            y = (sizeCanvas.y / 2 - topBarHeight) / 2;
        }
        return new Vector4(x, y, w, h);
    }

    void RunActionImageName()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction;
        Vector4 ptNormal = GetPosOfImageName();
        RectTransform rctran = imageBgName.GetComponent<RectTransform>();
        Vector2 sizeCanvas = this.frame.size;
        float x, y;
        x = 0;
        y = sizeCanvas.y / 2 + rctran.rect.height;
        rctran.anchoredPosition = new Vector2(x, y);

        Vector2 toPos = new Vector2(ptNormal.x, ptNormal.y);
        rctran.DOLocalMove(toPos, duration).OnComplete(() =>
                  {
                      Invoke("OnUIDidFinish", 1f);
                      isActionFinish = true;
                      Invoke("LayOut", 0.2f);
                  });
    }


    public void OnClickBtnPlay()
    {
        if (!isActionFinish)
        {
            return;
        }
        //AudioPlay.main.PlayAudioClip(audioClipBtn);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            int total = LevelManager.main.placeTotal;
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

    public void OnClickBtnLearn()
    {
        if (!isActionFinish)
        {
            return;
        }
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(LearnProgressViewController.main);

        }
    }
    public void UpdateLayoutBtn()
    {
        float w_item = 256;
        float h_item = 256;
        float oft = 64;
        layoutBtn.row = 1;
        layoutBtn.col = 2;
        if (!actionBtnLearn.gameObject.activeSelf)
        {
            layoutBtn.col = 1;
        }

        RectTransform rctran = layoutBtn.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2((w_item + oft) * layoutBtn.col, (h_item + oft) * layoutBtn.row);
        layoutBtn.LayOut();

    }

    public override void LayOut()
    {

        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
        RectTransform rctranImageName = imageBgName.GetComponent<RectTransform>();
        Vector4 ptImageName = GetPosOfImageName();
        //image name
        {

            rctranImageName.sizeDelta = new Vector2(ptImageName.z, ptImageName.w);
            rctranImageName.anchoredPosition = new Vector2(ptImageName.x, ptImageName.y);
        }


        //layoutBtn

        UpdateLayoutBtn();
        x = 0;
        float y1 = rctranImageName.anchoredPosition.y - rctranImageName.rect.size.y / 2;
        float ofty = rctranAppIcon.rect.size.y;
        if (Device.isLandscape)
        {
            ofty = 0;
        }
        float y2 = -sizeCanvas.y / 2 + ofty;
        y = (y1 + y2) / 2;
        //Debug.Log("layout y1=" + y1 + " y2=" + y2 + " y=" + y + " rctranAppIcon.rect.size.y=" + rctranAppIcon.rect.size.y);

        RectTransform rctranBtn = layoutBtn.GetComponent<RectTransform>();
        rctranBtn.anchoredPosition = new Vector2(x, y);


        LayoutChildBase();
    }
}
