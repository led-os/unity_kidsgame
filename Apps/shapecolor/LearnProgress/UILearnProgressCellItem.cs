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
    public GameObject objIconContent;
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

    public override void LayOut()
    {
        if (imageIcon.sprite == null)
        {
            return;
        }
        if (imageIcon.sprite.texture == null)
        {
            return;
        }
        int width = imageIcon.sprite.texture.width;
        int height = imageIcon.sprite.texture.height;
        RectTransform rctran = objIconContent.transform as RectTransform;
        if ((width != 0) && (height != 0))
        {
            float w_icon = rctran.rect.width;
            float h_icon = rctran.rect.height;
            float radio = 0.9f;
            float scalex = w_icon / width;
            float scaley = h_icon / height;
            float scale = Mathf.Min(scalex, scaley) * radio;
            imageIcon.transform.localScale = new Vector3(scale, scale, 1f);
        }

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

        // imageIcon.material = new Material(shaderColor);
        // Material mat = imageIcon.material;
        UIGameShapeColor game = GameViewController.main.gameBase as UIGameShapeColor;
        switch (itemType)
        {
            case ITEM_TYPE_SHAPE:
                {
                    if (Common.isWeb)
                    {
                        TextureUtil texutil = new TextureUtil();
                        ShapeColorItemInfo infohttp = new ShapeColorItemInfo();
                        infohttp.color = colorSel;
                        texutil.UpdateImageTextureWeb(imageIcon, HttpRequest.GetWebUrlOfAsset(info.pic), OnTextureHttpRequestFinished, infohttp, false);
                    }
                    else
                    {
                        Texture2D tex = TextureCache.main.Load(info.pic);
                        UpdateIcon(tex, colorSel);
                    }
                    string str = game.ShapeTitleOfItem(info);
                    textTitle.text = str;
                    textDetail.text = game.GameStatusOfShape(info);
                    //  mat.SetColor("_ColorShape", colorSel);
                }

                break;
            case ITEM_TYPE_COLOR:
                {

                    indexShape = UIGameShapeColor.listShape.Count / 2;
                    ShapeColorItemInfo infoshape = UIGameShapeColor.listShape[indexShape] as ShapeColorItemInfo;
                    // imageIcon.sprite = LoadTexture.CreateSprieFromAsset(infoshape.pic);

                    if (Common.isWeb)
                    {
                        TextureUtil texutil = new TextureUtil();
                        ShapeColorItemInfo infohttp = new ShapeColorItemInfo();
                        infohttp.color = info.color;
                        texutil.UpdateImageTextureWeb(imageIcon, HttpRequest.GetWebUrlOfAsset(infoshape.pic), OnTextureHttpRequestFinished, infohttp, false);
                    }
                    else
                    {
                        Texture2D tex = TextureCache.main.Load(infoshape.pic);
                        UpdateIcon(tex, info.color);
                    }
                    string str = game.ColorTitleOfItem(info);
                    textTitle.text = str;
                    textDetail.text = game.GameStatusOfColor(info);
                    //  mat.SetColor("_ColorShape", info.color);

                }
                break;
        }
        LayOut();


    }

    Texture2D GetIconFillColor(Texture2D tex, Color color)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        Material mat = new Material(shaderColor);
        mat.SetColor("_ColorShape", color);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt, tex.format, new Rect(0, 0, rt.width, rt.height));
        return texRet;
    }

    void UpdateIcon(Texture2D tex, Color color)
    {
        Debug.Log("UpdateIcon color=" + color);
        Texture2D texNew = GetIconFillColor(tex, color);
        imageIcon.sprite = LoadTexture.CreateSprieFromTex(texNew);
        RectTransform rctan = imageIcon.GetComponent<RectTransform>();
        rctan.sizeDelta = new Vector2(texNew.width, texNew.height);
    }
    public void OnTextureHttpRequestFinished(bool isSuccess, Texture2D tex, object data)
    {
        //  if (isSuccess && (tex != null))
        {
            Debug.Log("OnTextureHttpRequestFinished data  start");
            ShapeColorItemInfo info = data as ShapeColorItemInfo;
            Debug.Log("OnTextureHttpRequestFinished data  end");
            UpdateIcon(tex, info.color);
        }
        LayOut();
    }

}
