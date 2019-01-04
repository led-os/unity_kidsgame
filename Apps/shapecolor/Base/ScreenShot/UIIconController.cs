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
    public Image imageHuman;
    public Image imageShape0;
    public Image imageShape1;
    public Image imageShape2;
    public Image imageCenter;
    public GameObject objContent;
    List<object> listShape;
    UIGameShapeColor uiGameShapeColor;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        imageHD.gameObject.SetActive(false);
        imageBoard.gameObject.SetActive(true);
        string pic_name = Common.GAME_DATA_DIR + "/screenshot/icon/bg";
        string pic = pic_name + ".jpg";
        if (!FileUtil.FileIsExistAsset(pic))
        {
            pic = pic_name + ".png";
        }
        TextureUtil.UpdateImageTexture(imageBg, pic, true);
        TextureUtil.UpdateImageTexture(imageHuman, Common.GAME_DATA_DIR + "/screenshot/icon/human.png", true);

        uiGameShapeColor = GameViewController.main.gameBase as UIGameShapeColor;
        listShape = GameManager.main.GetGuankaListOfAllPlace();
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
        List<object> listColor = UIGameShapeColor.listColor;
        Shader shaderColor = Shader.Find("Custom/ShapeColor");
        {
            ShapeColorItemInfo info = listShape[0] as ShapeColorItemInfo;
            TextureUtil.UpdateImageTexture(imageShape0, info.pic, true);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[0] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape0.material = mat;
        }
        {
            ShapeColorItemInfo info = listShape[1] as ShapeColorItemInfo;
            TextureUtil.UpdateImageTexture(imageShape1, info.pic, true);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[1] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape1.material = mat;
        }
        {
            ShapeColorItemInfo info = listShape[2] as ShapeColorItemInfo;
            TextureUtil.UpdateImageTexture(imageShape2, info.pic, true);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[2] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape2.material = mat;
        }
        {
            ShapeColorItemInfo info = listShape[3] as ShapeColorItemInfo;
            TextureUtil.UpdateImageTexture(imageCenter, info.pic, true);
        }

        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        base.LayOut();
        {
            RectTransform rctranContent = objContent.GetComponent<RectTransform>();
            // GridLayoutGroup gridLayout = objContent.GetComponent<GridLayoutGroup>();
            //  gridLayout.cellSize = new Vector2(rctran.rect.width / 2, rctran.rect.height / 2);

            float ratio = 1f;
            float x, y, w, h, w_rect, h_rect, scale;
            w_rect = rctranContent.rect.width / 2;
            h_rect = rctranContent.rect.height / 2;
            {

                RectTransform rctran = imageHuman.GetComponent<RectTransform>();
                x = -w_rect / 2;
                y = -h_rect / 2;
                rctran.anchoredPosition = new Vector2(x, y);
                w = imageHuman.sprite.texture.width;
                h = imageHuman.sprite.texture.height;
                w_rect = this.frame.width / 2;
                h_rect = this.frame.height / 2;
                ratio = 0.9f;
                scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                imageHuman.transform.localScale = new Vector3(scale, scale, 1f);
            }

            ratio = 0.7f;
            {
                RectTransform rctran = imageShape0.GetComponent<RectTransform>();
                w = imageShape0.sprite.texture.width;
                h = imageShape0.sprite.texture.height;
                scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                imageShape0.transform.localScale = new Vector3(scale, scale, 1f);
                x = -w_rect / 2;
                y = h_rect / 2;
                rctran.anchoredPosition = new Vector2(x, y);
            }
            {
                RectTransform rctran = imageShape1.GetComponent<RectTransform>();
                w = imageShape1.sprite.texture.width;
                h = imageShape1.sprite.texture.height;
                scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                imageShape1.transform.localScale = new Vector3(scale, scale, 1f);
                x = w_rect / 2;
                y = h_rect / 2;
                rctran.anchoredPosition = new Vector2(x, y);
            }
            {
                RectTransform rctran = imageShape2.GetComponent<RectTransform>();
                w = imageShape2.sprite.texture.width;
                h = imageShape2.sprite.texture.height;
                scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                imageShape2.transform.localScale = new Vector3(scale, scale, 1f);
                x = w_rect / 2;
                y = -h_rect / 2;
                rctran.anchoredPosition = new Vector2(x, y);
            }

            {
                ratio = 0.8f;
                RectTransform rctran = imageCenter.GetComponent<RectTransform>();
                w = imageCenter.sprite.texture.width;
                h = imageCenter.sprite.texture.height;
                scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                imageCenter.transform.localScale = new Vector3(scale, scale, 1f);
                x = 0;
                y = 0;
                rctran.anchoredPosition = new Vector2(x, y);
            }

        }

    }
}
