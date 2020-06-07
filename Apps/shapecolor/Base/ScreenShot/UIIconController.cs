using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HighlightingSystem;

public class UIIconController : UIShotBase
{
    public UIImage imageBg;
    public UIImage imageHD;
    public UIImage imageBoard;
    public UIImage imageHuman;
    public UIImage imageShape0;
    public UIImage imageShape1;
    public UIImage imageShape2;
    public UIImage imageHeighLight;
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
        imageBg.UpdateImage(pic);
        if (imageHuman != null)
        {
            imageHuman.UpdateImage(Common.GAME_DATA_DIR + "/screenshot/icon/human.png");
        }

        uiGameShapeColor = GameViewController.main.gameBase as UIGameShapeColor;
        listShape = LevelManager.main.GetGuankaListOfAllPlace();
        InitHighLight();
        LayOut();
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
        List<object> listColor = GameLevelParse.main.listColor;
        Shader shaderColor = Shader.Find("Custom/ShapeColor");
        {
            ShapeColorItemInfo info = listShape[0] as ShapeColorItemInfo;
            imageShape0.UpdateImage(info.pic,imageShape0.keyImage);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[0] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape0.image.material = mat;
        }
        {
            ShapeColorItemInfo info = listShape[1] as ShapeColorItemInfo;
            imageShape1.UpdateImage(info.pic);

            ShapeColorItemInfo infocolor = listColor[1] as ShapeColorItemInfo;
            //ShapeHighlighterController hlc = AddHighLight(imageShape1.gameObject);
            // hlc.UpdateColor(infocolor.color);
            imageHeighLight.UpdateImage(info.pic);
            Material mat = new Material(shaderColor);
            mat.SetColor("_ColorShape", infocolor.color);
            imageHeighLight.image.material = mat;
        }




        {
            ShapeColorItemInfo info = listShape[2] as ShapeColorItemInfo;
            imageShape2.UpdateImage(info.pic);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[2] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape2.image.material = mat;
        }

        LayOut();
        OnUIDidFinish();
    }
    ShapeHighlighterController AddHighLight(GameObject obj)
    {

        return obj.AddComponent<ShapeHighlighterController>();

    }

    void InitHighLight()
    {
        if (mainCam.GetComponent<HighlightingRenderer>() == null)
        {
            mainCam.gameObject.AddComponent<HighlightingRenderer>();
        }
    }
    public override void LayOut()
    {
        base.LayOut();
        if (objContent != null)
        {
            RectTransform rctranContent = objContent.GetComponent<RectTransform>();
            // GridLayoutGroup gridLayout = objContent.GetComponent<GridLayoutGroup>();
            //  gridLayout.cellSize = new Vector2(rctran.rect.width / 2, rctran.rect.height / 2);

            float ratio = 1f;
            float x, y, w, h, w_rect, h_rect, scale;
            w_rect = rctranContent.rect.width / 2;
            h_rect = rctranContent.rect.height / 2;
            {

                // RectTransform rctran = imageHuman.GetComponent<RectTransform>();
                // x = -w_rect / 2;
                // y = -h_rect / 2;
                // rctran.anchoredPosition = new Vector2(x, y);
                // w = imageHuman.sprite.texture.width;
                // h = imageHuman.sprite.texture.height;
                // w_rect = this.frame.width / 2;
                // h_rect = this.frame.height / 2;
                // ratio = 0.9f;
                // scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                // imageHuman.transform.localScale = new Vector3(scale, scale, 1f);
            }

            ratio = 0.7f;
            {
                // RectTransform rctran = imageShape0.GetComponent<RectTransform>();
                // w = imageShape0.sprite.texture.width;
                // h = imageShape0.sprite.texture.height;
                // scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                // imageShape0.transform.localScale = new Vector3(scale, scale, 1f);
                // x = -w_rect / 2;
                // y = h_rect / 2;
                // rctran.anchoredPosition = new Vector2(x, y);
            }
            {
                // RectTransform rctran = imageShape1.GetComponent<RectTransform>();
                // w = imageShape1.sprite.texture.width;
                // h = imageShape1.sprite.texture.height;
                // scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                // imageShape1.transform.localScale = new Vector3(scale, scale, 1f);
                // x = w_rect / 2;
                // y = h_rect / 2;
                // rctran.anchoredPosition = new Vector2(x, y);
            }
            {
                // RectTransform rctran = imageShape2.GetComponent<RectTransform>();
                // w = imageShape2.sprite.texture.width;
                // h = imageShape2.sprite.texture.height;
                // scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
                // imageShape2.transform.localScale = new Vector3(scale, scale, 1f);
                // x = w_rect / 2;
                // y = -h_rect / 2;
                // rctran.anchoredPosition = new Vector2(x, y);
            }



        }

    }
}
