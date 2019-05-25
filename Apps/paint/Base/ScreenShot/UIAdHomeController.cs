using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAdHomeController : UIShotBase
{
    public Image imageBg;
    public Image imageGame;
    public Image imageHuman;
    public Image imagePen;
    UIGamePaint game;
    public Image imageTitle;
    public Text textTitle;


    List<object> listItem;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;
        game = GameViewController.main.gameBase as UIGamePaint;
        GameManager.main.ParseGuanka();
        listItem = GameManager.main.GetGuankaListOfAllPlace();
        ColorItemInfo info = listItem[1] as ColorItemInfo;
        TextureUtil.UpdateImageTexture(imageGame, info.picmask, true);
        TextureUtil.UpdateImageTexture(imageHuman, Common.GAME_DATA_DIR + "/screenshot/icon/human.png", true);
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);
        TextureUtil.UpdateImageTexture(imagePen, Common.GAME_DATA_DIR + "/screenshot/icon/pen.png", true);
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
        float ratio = 1f;

        float titleBarHeight = 160f;

        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float scale = Common.GetMaxFitScale(rctran.rect.width, rctran.rect.height, this.frame.size.x, this.frame.size.y);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        {
            ratio = 0.8f;
            RectTransform rctran = imageGame.GetComponent<RectTransform>();
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, this.frame.size.x / 2, this.frame.size.y - titleBarHeight * 2) * ratio;
            imageGame.transform.localScale = new Vector3(scale, scale, 1.0f);
            x = -this.frame.size.x / 4;
            y = 0;
            rctran.anchoredPosition = new Vector2(x, y);
        }


        {
            ratio = 0.6f;
            RectTransform rctran = imagePen.GetComponent<RectTransform>();
            RectTransform rctranGame = imageGame.GetComponent<RectTransform>();
            w = rctranGame.rect.width * imageGame.transform.localScale.x;
            h = rctranGame.rect.height * imageGame.transform.localScale.y;
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, w, h) * ratio;
            imagePen.transform.localScale = new Vector3(scale, scale, 1.0f);
            rctran.anchoredPosition = rctranGame.anchoredPosition;
        }


        {
            ratio = 0.9f;
            RectTransform rctran = imageHuman.GetComponent<RectTransform>();
            w = this.frame.size.x / 2;
            h = this.frame.size.y - titleBarHeight * 2;
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, w, h) * ratio;
            imageHuman.transform.localScale = new Vector3(scale, scale, 1.0f);
            x = this.frame.size.x / 4;
            y = 0;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        if (imageTitle != null)
        {
            x = 0;
            RectTransform rctran = imageTitle.GetComponent<RectTransform>();
            y = -this.frame.size.y / 2 + rctran.rect.height + 8;
            rctran.anchoredPosition = new Vector2(x, y);

            string str = textTitle.text;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + fontsize + oft * 2;
            rctran.sizeDelta = sizeDelta;
        }


    }
}
