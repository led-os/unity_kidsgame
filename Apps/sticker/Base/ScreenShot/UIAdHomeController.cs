using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAdHomeController : UIShotBase
{
    public Image imageBg;
    public Image imageTitle;
    public Text textTitle;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;

        Vector2 sizeCanvas = this.frame.size;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w_image = rctran.rect.width;
            float h_image = rctran.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

        }



        {
            RectTransform rctran = imageTitle.GetComponent<RectTransform>();

            int fontSize = textTitle.fontSize;
            int r = fontSize*2;
            w = Common.GetStringLength(textTitle.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 2f;
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

            rctran.sizeDelta = new Vector2(w, h);
            x = 0;
            y = 0;
            rctran.anchoredPosition = new Vector2(x, y);
        }

    }
}
