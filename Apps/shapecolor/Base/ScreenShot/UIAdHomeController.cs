using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAdHomeController : UIShotBase
{
    public UIImage imageBg;

    public UIImage imageHuman;
    public UIImage imageShape0;
    public UIImage imageShape1;
    public UIImage imageShape2;
    public UIImage imageHeighLight;
    public GameObject objContent;
    public UIText textTitle;

    List<object> listShape;
    UIGameShapeColor uiGameShapeColor;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;
        string pic_name = Common.GAME_DATA_DIR + "/screenshot/icon/ad_bg";
        string pic = pic_name + ".jpg";
        if (!FileUtil.FileIsExistAsset(pic))
        {
            pic = pic_name + ".png";
        }
        imageBg.UpdateImage(pic);
        if (imageHuman != null)
        {

            imageHuman.UpdateImage(Common.GAME_DATA_DIR + "/screenshot/icon/human.png", imageShape0.keyImage);
        }
        uiGameShapeColor = GameViewController.main.gameBase as UIGameShapeColor;
        listShape = LevelManager.main.GetGuankaListOfAllPlace();

        LayOut();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

        List<object> listColor = GameLevelParse.main.listColor;
        Shader shaderColor = Shader.Find("Custom/ShapeColor");
        {
            ShapeColorItemInfo info = listShape[0] as ShapeColorItemInfo;
            imageShape1.UpdateImage(info.pic, imageShape1.keyImage);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[0] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape1.image.material = mat;
        }
        {
            ShapeColorItemInfo info = listShape[1] as ShapeColorItemInfo;
            imageShape0.UpdateImage(info.pic, imageShape0.keyImage);
            ShapeColorItemInfo infocolor = listColor[1] as ShapeColorItemInfo;
            // mat.SetColor("_ColorShape", infocolor.color);
            // imageShape1.material = mat;
            imageHeighLight.UpdateImage(info.pic, imageHeighLight.keyImage);
            Material mat = new Material(shaderColor);
            mat.SetColor("_ColorShape", infocolor.color);
            imageHeighLight.image.material = mat;
        }
        {
            ShapeColorItemInfo info = listShape[2] as ShapeColorItemInfo;
            imageShape2.UpdateImage(info.pic, imageShape2.keyImage);
            Material mat = new Material(shaderColor);
            ShapeColorItemInfo infocolor = listColor[2] as ShapeColorItemInfo;
            mat.SetColor("_ColorShape", infocolor.color);
            imageShape2.image.material = mat;
        }

        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        base.LayOut();
    }
}
