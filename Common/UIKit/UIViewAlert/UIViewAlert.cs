using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUIViewAlertFinishedDelegate(UIViewAlert alert, bool isYes);
public class UIViewAlert : UIView
{
    public GameObject objContent;
    public Image imageBg;
    public Image imageBoard;
    public Text textTitle;
    public Text textMsg;
    public Button btnYes;
    public Button btnNo;
    public string keyName;
    public OnUIViewAlertFinishedDelegate callback { get; set; }


    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        {
            TextureUtil.UpdateImageTexture(imageBoard, AppRes.IMAGE_UIVIEWALERT_BG_BOARD, true, AppRes.borderUIViewAlertBgBoard);
            RectTransform rctran = imageBoard.GetComponent<RectTransform>();
            rctran.offsetMin = Vector2.zero;
            rctran.offsetMax = Vector2.zero;
        }
        {
            RectTransform rctran = btnYes.GetComponent<RectTransform>();
            Vector2 size = rctran.sizeDelta;
            TextureUtil.UpdateImageTexture(btnYes.GetComponent<Image>(), AppRes.IMAGE_UIVIEWALERT_BG_BTN, true, AppRes.borderUIViewAlertBgBtn);
            //恢复初始大小
            rctran.sizeDelta = size;
        }

        {
            RectTransform rctran = btnNo.GetComponent<RectTransform>();
            Vector2 size = rctran.sizeDelta;
            TextureUtil.UpdateImageTexture(btnNo.GetComponent<Image>(), AppRes.IMAGE_UIVIEWALERT_BG_BTN, true, AppRes.borderUIViewAlertBgBtn);
            //恢复初始大小
            rctran.sizeDelta = size;
        }
    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        float w, h;
        Debug.Log("UIViewAlert:frame=" + this.frame);
        // {
        //     RectTransform rctran = objContent.GetComponent<RectTransform>();
        //     Vector2 sizeDelta = rctran.sizeDelta;
        //     float ratio = 0.8f;
        //     if (sizeDelta.x > this.frame.width * ratio)
        //     {
        //         //防止超出屏幕显示
        //         Vector2 sizeDeltaNew = sizeDelta;
        //         sizeDeltaNew.x = this.frame.width * ratio;
        //         sizeDeltaNew.y = sizeDeltaNew.x * sizeDelta.y / sizeDelta.x;
        //         if (!Device.isLandscape)
        //         {
        //             sizeDeltaNew.y = sizeDeltaNew.x;
        //         }
        //         rctran.sizeDelta = sizeDeltaNew;
        //     }
        // }

        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            w = Mathf.Min(this.frame.width, this.frame.height) * 0.8f;
            h = w / 2;
            rctran.sizeDelta = new Vector2(w, h);

        }
    }


    void Remove()
    {

        DestroyImmediate(this.gameObject);



    }

    public void Hide()
    {
        Remove();
    }
    public void SetText(string title, string msg, string yes, string no)
    {
        textTitle.text = title;
        textMsg.text = msg;

        {
            string strYes = yes;
            string strNo = no;
            Text btnText = Common.GetButtonText(btnYes);
            float strWYes = Common.GetButtonTextWidth(btnYes, strYes);
            float strWNo = Common.GetButtonTextWidth(btnNo, strNo);
            float oft = btnText.fontSize;
            float strW = Mathf.Max(strWYes, strWNo) + oft;
            Common.SetButtonTextWidth(btnYes, strYes, strW);
            Common.SetButtonTextWidth(btnNo, strNo, strW);
        }

    }
    public void ShowBtnNo(bool isShow)
    {
        btnNo.gameObject.SetActive(isShow);
    }
    public void OnClickBtnYes()
    {
        Remove();

        if (this.callback != null)
        {
            Debug.Log("UIViewAlert OnClickBtnYes: Callback");
            this.callback(this, true);
        }
    }


    public void OnClickBtnNo()
    {
        Remove();

        if (this.callback != null)
        {
            this.callback(this, false);
        }
    }
}
