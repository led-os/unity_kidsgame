using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopSelectBarCellItem : UICellItemBase
{
    public Image imageBg;
    public Image imageLock;
    public bool enableLock = true;
    FoodItemInfo infoFood;
    public override void UpdateItem(List<object> list)
    {
        float x, y, w, h;
        FoodItemInfo info = list[index] as FoodItemInfo;
        infoFood = info;
      //  Debug.Log("UpdateItem:info.pic=" + info.pic);
        TextureUtil.UpdateImageTexture(imageBg, info.pic, true);
        {
            RectTransform rctranCup = imageBg.GetComponent<RectTransform>();
            string pic = GameIronIceCream.IMAGE_LOCK;
            TextureUtil.UpdateImageTexture(imageLock, pic, true);
            w = imageLock.sprite.texture.width;
            h = imageLock.sprite.texture.height;
            float w_cup = rctranCup.rect.width * imageBg.transform.localScale.x;
            float h_cup = rctranCup.rect.height * imageBg.transform.localScale.y;
            float w_rect = w_cup / 2;
            float h_rect = h_cup / 2;

            float scale = Common.GetBestFitScale(w, h, w_rect, h_rect);
            imageLock.transform.localScale = new Vector3(scale, scale, 1f);

            RectTransform rctran = imageLock.GetComponent<RectTransform>();
            x = w_cup / 2 - w_rect / 2;
            y = -h_cup / 2 + h_rect / 2;
            rctran.anchoredPosition = new Vector2(x, y);
            imageLock.gameObject.SetActive(false);
            if (info.isLock && enableLock)
            {
                imageLock.gameObject.SetActive(true);
            }
        }
        LayOut();
    }
    public override bool IsLock()
    {
        return infoFood.isLock;
    }

    public override void LayOut()
    {
        RectTransform rctran = imageBg.GetComponent<RectTransform>();
        float ratio = 1f;

        float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
        imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

    }
}
