using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIIconController : UIShotBase
{
    public Image imageBg;
    public Image imageHD;
    public Image imageBoard;
    public Image imageGame;
    public Image imageHuman;
    public Image imagePen;
    UIGamePaint game;
    List<object> listItem;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        imageHD.gameObject.SetActive(false);
        imageBoard.gameObject.SetActive(true);
        game = GameViewController.main.gameBase as UIGamePaint;
        GameManager.main.ParseGuanka();
        listItem = GameManager.main.GetGuankaListOfAllPlace();
        ColorItemInfo info = listItem[0] as ColorItemInfo;
        TextureUtil.UpdateImageTexture(imageGame, info.picmask, true);
        TextureUtil.UpdateImageTexture(imageHuman, Common.GAME_DATA_DIR + "/screenshot/icon/human.png", true);
        TextureUtil.UpdateImageTexture(imageBg, Common.GAME_DATA_DIR + "/screenshot/icon/bg.jpg", true);
        TextureUtil.UpdateImageTexture(imagePen, Common.GAME_DATA_DIR + "/screenshot/icon/pen.png", true);
    }
    void Start()
    {
        IconViewController iconctroller = (IconViewController)this.controller;
        if (iconctroller != null)
        {
            if (iconctroller.deviceInfo.isIconHd)
            {
                imageHD.gameObject.SetActive(true);
            }

        }
        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        base.LayOut();

        float x, y, w, h;
        float ratio = 1f;

        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float scale = Common.GetMaxFitScale(rctran.rect.width, rctran.rect.height, this.frame.size.x, this.frame.size.y);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        {
            ratio = 0.8f;
            RectTransform rctran = imageGame.GetComponent<RectTransform>();
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, this.frame.size.x, this.frame.size.y) * ratio;
            imageGame.transform.localScale = new Vector3(scale, scale, 1.0f);
        }


        {
            ratio = 0.6f;
            RectTransform rctran = imagePen.GetComponent<RectTransform>();
            RectTransform rctranGame = imageGame.GetComponent<RectTransform>();
            w = rctranGame.rect.width * imageGame.transform.localScale.x;
            h = rctranGame.rect.height * imageGame.transform.localScale.y;
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, w, h) * ratio;
            imagePen.transform.localScale = new Vector3(scale, scale, 1.0f);
            rctran.anchoredPosition = Vector2.zero;
        }


        {
            ratio = 0.9f;
            RectTransform rctran = imageHuman.GetComponent<RectTransform>();
            w = this.frame.size.x / 2;
            h = this.frame.size.y / 2;
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, w, h) * ratio;
            imageHuman.transform.localScale = new Vector3(scale, scale, 1.0f);
            x = this.frame.size.x / 4;
            y = -this.frame.size.y / 4;
            rctran.anchoredPosition = new Vector2(x, y);
        }
    }
}
