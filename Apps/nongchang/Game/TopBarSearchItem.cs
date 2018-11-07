using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarSearchItem : UIView
{
    public Image imageBg;
    public Image imageItem;
    void Awake()
    {
        imageItem.material = new Material(Shader.Find("Custom/Grey"));
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void ShowGrey(bool isShow)
    {
        if (isShow)
        {
            imageItem.material.SetInt("_Enable", 1);
        }
        else
        {
            imageItem.material.SetInt("_Enable", 0);
        }

    }
    public void DidFind()
    {
        ShowGrey(true);
    }
    public void UpdateItem(NongChangItemInfo info)
    {
        Texture2D tex = LoadTexture.LoadFromAsset(info.pic);
        imageItem.sprite = LoadTexture.CreateSprieFromTex(tex);
        RectTransform rctranitem = imageItem.transform as RectTransform;
        rctranitem.sizeDelta = new Vector2(tex.width, tex.height);

        RectTransform rctran = this.gameObject.transform as RectTransform;
        RectTransform rctranParent = this.gameObject.transform.parent as RectTransform;
        float ratio = 0.5f;

        //大小由Layout Group决定
        float w = rctranParent.rect.width / 5;
        float h = rctranParent.rect.height;

        float width = w * ratio;
        float height = h * ratio;
        float scalex = width / rctranitem.rect.width;
        float scaley = height / rctranitem.rect.height;
        float scale = Mathf.Min(scalex, scaley);
        //scale = 0.5f;
        imageItem.transform.localScale = new Vector3(scale, scale, 1f);
        ShowGrey(false);
    }
    public void OnClickBtn()
    {
    }

}
