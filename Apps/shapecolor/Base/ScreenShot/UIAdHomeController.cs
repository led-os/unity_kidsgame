using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAdHomeController : UIShotBase
{
    public Image imageBg;

    public Image imageHuman;
    public Image imageShape0;
    public Image imageShape1;
    public Image imageShape2;
    public Image imageShape3;
    public GameObject objContent;
    public Image imageTitle;
    public Text textTitle;

    List<object> listShape;
    UIGameShapeColor uiGameShapeColor;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;
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

        List<object> listColor = UIGameShapeColor.listColor;
        Shader shaderColor = Shader.Find("Custom/ShapeColor");
        {
            ShapeColorItemInfo info = listShape[0] as ShapeColorItemInfo;
            TextureUtil.UpdateImageTexture(imageShape0, info.pic, true);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[0] as ShapeColorItemInfo;
            // mat.SetColor("_ColorShape", infocolor.color);
            //imageShape0.material = mat;
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
            TextureUtil.UpdateImageTexture(imageShape3, info.pic, true);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[3] as ShapeColorItemInfo;
            //mat.SetColor("_ColorShape", infocolor.color);
            //imageShape3.material = mat;
        }

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
        float titleBarHeight = 160f;
        float x, y, w, h, scale, w_rect, h_rect, ratio;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            scale = Common.GetMaxFitScale(rctran.rect.width, rctran.rect.height, this.frame.size.x, this.frame.size.y);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

        }

        {

            RectTransform rctran = imageHuman.GetComponent<RectTransform>();
            y = 0;
            x = -this.frame.width / 4;
            rctran.anchoredPosition = new Vector2(x, y);
            w = imageHuman.sprite.texture.width;
            h = imageHuman.sprite.texture.height;
            w_rect = this.frame.width / 2;
            h_rect = this.frame.height;
            ratio = 0.7f;
            scale = Common.GetBestFitScale(w, h, w_rect, h_rect) * ratio;
            imageHuman.transform.localScale = new Vector3(scale, scale, 1f);
        }

        {
            ratio = 0.7f;
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            y = 0;
            x = this.frame.width / 4;
            rctran.anchoredPosition = new Vector2(x, y);
            w = this.frame.width / 2;
            h = this.frame.height - titleBarHeight * 2;
            w = Mathf.Min(w, h) * ratio;
            h = w;
            Debug.Log("objContent w =" + w + " h=" + h);
            rctran.sizeDelta = new Vector2(w, h);

            GridLayoutGroup gridLayout = objContent.GetComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(w / 2, h / 2);
        }

        {
            // x = this.frame.width / 4;
            // RectTransform rctran = imageTitle.GetComponent<RectTransform>();
            // y = -rctran.rect.height;
            // rctran.anchoredPosition = new Vector2(x, y);
            {

                int fontsize = textTitle.fontSize;
                float str_w = Common.GetStringLength(textTitle.text, AppString.STR_FONT_NAME, fontsize);
                RectTransform rctran = imageTitle.transform as RectTransform;
                Vector2 sizeDelta = rctran.sizeDelta;
                float oft = 0;
                sizeDelta.x = str_w + fontsize + oft * 2;
                rctran.sizeDelta = sizeDelta;
            }
        }


    }
}
