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
        // if ((Common.appKeyName == GameRes.GAME_IdiomConnect) || (Common.appKeyName == GameRes.GAME_IDIOM) || (Common.appKeyName == GameRes.GAME_RIDDLE) || (Common.appKeyName == GameRes.GAME_POEM) || (Common.appKeyName == GameRes.GAME_XIEHOUYU))
        // {

        // }
        string pic = info.pic;
        if (!FileUtil.FileIsExistAsset(pic))
        {
            pic = Common.GAME_RES_DIR + "/place/image/PlaceItemBg.png";
            LanguageManager.main.UpdateLanguagePlace();
            textTitle.gameObject.SetActive(true);
            textTitle.text = LanguageManager.main.languagePlace.GetString("STR_PLACE_" + info.id);
        } 
        TextureUtil.UpdateRawImageTexture(imageBg, pic, true);
        imageIcon.gameObject.SetActive(info.isAd);
        textTitle.color = ColorConfig.main.GetColor(GameRes.KEY_COLOR_PlaceItemTitle);
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
