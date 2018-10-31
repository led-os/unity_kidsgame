using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;

public class UILearnProgressCellItem : UICellItemBase
{

    public const int ITEM_TYPE_SHAPE = 0;
    public const int ITEM_TYPE_COLOR = 1;
    public Image imageBg;
    public Image imageIcon;
    public Text textTitle;
    public Text textDetail;
    public float itemWidth;
    public float itemHeight;
    public int itemType;
    //public TableView tableView; 

    public Color colorSel;
    public Color colorUnSel;

    Shader shaderColor;

    int indexShape;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string strshader = "Custom/ShapeColor";
        shaderColor = Shader.Find(strshader);
        UIGameShapeColor game = GameViewController.main.gameBase as UIGameShapeColor;
        game.ParseGuanka();
        indexShape = Random.Range(0, UIGameShapeColor.listShape.Count);
    }

    public override void UpdateItem(List<object> list)
    {
        ShapeColorItemInfo info = list[index] as ShapeColorItemInfo;
        UpdateInfo(info);

    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public void SetItemType(int type)
    {
        itemType = type;

    }

    void SetSelect(bool isSel)
    {
        if (isSel)
        {
            textTitle.color = colorSel;
        }
        else
        {
            textTitle.color = colorUnSel;
        }
    }

    public void UpdateInfo(ShapeColorItemInfo info)
    {
        //Sprite sp = Resources.Load(info.pic, typeof(Sprite)) as Sprite;
        //imageBg.sprite = sp;

        //color

        imageIcon.material = new Material(shaderColor);
        Material mat = imageIcon.material;
        UIGameShapeColor game = GameViewController.main.gameBase as UIGameShapeColor;
        switch (itemType)
        {
            case ITEM_TYPE_SHAPE:
                {
                    imageIcon.sprite = LoadTexture.CreateSprieFromAsset(info.pic);

                    string str = game.ShapeTitleOfItem(info);
                    textTitle.text = str;
                    textDetail.text = game.GameStatusOfShape(info);
                    mat.SetColor("_ColorShape", colorSel);
                }

                break;
            case ITEM_TYPE_COLOR:
                {

                    indexShape = UIGameShapeColor.listShape.Count / 2;
                    ShapeColorItemInfo infoshape = UIGameShapeColor.listShape[indexShape] as ShapeColorItemInfo;
                    imageIcon.sprite = LoadTexture.CreateSprieFromAsset(infoshape.pic);
                    string str = game.ColorTitleOfItem(info);
                    textTitle.text = str;
                    textDetail.text = game.GameStatusOfColor(info);
                    mat.SetColor("_ColorShape", info.color);

                }
                break;
        }


        int width = imageIcon.sprite.texture.width;
        int height = imageIcon.sprite.texture.height;
        RectTransform rctran = imageIcon.transform as RectTransform;
        float w_icon = rctran.rect.width;
        float h_icon = rctran.rect.height;
        float scalex = w_icon / width;
        float scaley = h_icon / height;
        float scale = Mathf.Min(scalex, scaley);
        imageIcon.transform.localScale = new Vector3(scale, scale, 1f);
    }

}
