using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class a : UIHomeBase
{
    public LayOutGrid layoutBtn;
    public ActionHomeBtn actionPlay;
    public ActionHomeBtn actionLearn;
    public ActionHomeBtn actionAdVideo;

    List<object> listItem;
    float timeAction;
    bool isActionFinish;

    public void Awake()
    {
        base.Awake();
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        timeAction = 0.3f;

        if (actionAdVideo != null)
        {
            actionAdVideo.gameObject.SetActive(true);
            if ((Common.noad) || (!AppVersion.appCheckHasFinished))
            {
                actionAdVideo.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.GP)
                {
                    //GP市场不显示
                    actionAdVideo.gameObject.SetActive(false);
                }
            }
        }

        isActionFinish = false;


        listItem = new List<object>();
        listItem.Add(actionPlay);
        listItem.Add(actionLearn);
        if (actionAdVideo.gameObject.activeSelf)
        {
            listItem.Add(actionAdVideo);
        }

        layoutBtn.enableLayout = false;
        UpdateLayoutBtn();
    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        isActionFinish = false;
        LayOut();
        RunActionImageName();
        foreach (object obj in listItem)
        {
            ActionHomeBtn action = obj as ActionHomeBtn;
            action.RunAction();
        }
        Invoke("OnUIDidFinish", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }
    void UpdateLayoutBtn()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        int row = 0;
        int col = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
        RectTransform rctranImageName = imageBgName.GetComponent<RectTransform>();
        {
            LayOutGrid lygrid = layoutBtn;
            RectTransform rctran = layoutBtn.gameObject.GetComponent<RectTransform>();
            int h_item = 256;
            int oft = 16;

            lygrid.row = 1;
            lygrid.col = 2;



            int idx = 0;
            foreach (object obj in listItem)
            {
                row = idx / lygrid.col;
                row = lygrid.row - 1 - row;
                col = idx % lygrid.col;
                ActionHomeBtn action = obj as ActionHomeBtn;
                action.ptNormal = layoutBtn.GetItemPostion(actionPlay.gameObject, row, col);
                idx++;
            }

            w = (h_item + oft) * lygrid.col;
            h = (h_item + oft) * lygrid.row;
            x = 0;
            float y_bottom = -sizeCanvas.y / 2 + topBarHeight + 16;
            Debug.Log("y_bottom=" + y_bottom);
            y = y_bottom / 2;


            float h_appcenter = rctranImageName.rect.height;
            if (Device.isLandscape)
            {
                h_appcenter = 0;
            }
            //居中
            float y1 = -sizeCanvas.y / 2 + h_appcenter;
            float y2 = rctranImageName.anchoredPosition.y - rctranAppIcon.rect.height / 2;
            y = (y1 + y2) / 2;


            if ((y - h / 2) < y_bottom)
            {
                y = y_bottom + h / 2;
            }

            rctran.sizeDelta = new Vector2(w, h);
            // rctran.offsetMin = new Vector2(0, rctran.offsetMin.y);
            // rctran.offsetMax = new Vector2(0, rctran.offsetMax.y);
            rctran.anchoredPosition = new Vector2(x, y);

            lygrid.LayOut();
        }

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
                      isActionFinish = true;
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

    public override void LayOut()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;

        Vector4 ptImageName = GetPosOfImageName();
        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(ptImageName.z, ptImageName.w);
            rctran.anchoredPosition = new Vector2(ptImageName.x, ptImageName.y);
        }

        UpdateLayoutBtn();

        LayoutChildBase();
    }
}
