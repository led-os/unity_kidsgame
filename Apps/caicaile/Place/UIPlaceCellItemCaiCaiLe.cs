using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceCellItemCaiCaiLe : UICellItemBase
{

    public Text textTitle;
    public RawImage imageBg;
    public RawImage imageIcon;
    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;

        textTitle.gameObject.SetActive(false);
        if ((Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_POEM) || (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_XIEHOUYU))
        {
            LanguageManager.main.UpdateLanguagePlace();
            textTitle.gameObject.SetActive(true);
            textTitle.text = LanguageManager.main.languagePlace.GetString("STR_PLACE_" + info.id);
        }



        TextureUtil.UpdateRawImageTexture(imageBg, info.pic, true);
        imageIcon.gameObject.SetActive(info.isAd);
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.texture.width;//rectTransform.rect.width;
            float h = imageBg.texture.height;//rectTransform.rect.height;
            RectTransform rctranCellItem = objContent.GetComponent<RectTransform>();

            float scalex = width / w;
            float scaley = height / h;
            float scale = Mathf.Min(scalex, scaley);
            scale = scale * 0.8f;
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

}
