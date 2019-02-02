
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UINetImageCellItem : UICellItemBase
{
    public Image imageBg;
    public Image imagePic;
    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;

            Vector4 border = AppRes.borderCellSettingBg;
            TextureUtil.UpdateImageTexture(imagePic, "", true);
        }
    }
    public override void LayOut()
    {
        {
            // RectTransform rctran = imageBg.GetComponent<RectTransform>();
            // float w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            // float h = imageBg.sprite.texture.height;//rectTransform.rect.height;
            // RectTransform rctranCellItem = objContent.GetComponent<RectTransform>();

            // float scalex = width / w;
            // float scaley = height / h;
            // float scale = Mathf.Min(scalex, scaley);
            // scale = scale * 0.8f;
            // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }
}

