using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopCellItem : UICellItemBase
{

    public Image imageBg;
    public Text textTitle;
    public Button btnBuy;
    public bool isDisable = false;//显示灰色 
    Color colorDisable = new Color(102 / 255.0f, 102 / 255.0f, 102 / 255.0f, 1f);
    string[] strImageBg = { AppRes.IMAGE_CELL_BG_BLUE, AppRes.IMAGE_CELL_BG_ORINGE, AppRes.IMAGE_CELL_BG_YELLOW };


    public override void UpdateItem(List<object> list)
    {
        ShopItemInfo info = list[index] as ShopItemInfo;
        textTitle.text = info.title;
        Vector4 border = AppRes.borderCellSettingBg;
        TextureUtil.UpdateImageTexture(imageBg, strImageBg[index % 3], false, border);

        Common.SetButtonText(btnBuy, info.artist, 0);
        if (info.isIAP)
        {
            btnBuy.gameObject.SetActive(true);
            // offsetMax.x = -232;
        }
        else
        {
            btnBuy.gameObject.SetActive(false);
            // offsetMax.x = -16;
        }
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public void SetDisable(bool disable)
    {
        isDisable = disable;
        if (disable)
        {
            textTitle.color = colorDisable;
        }
        else
        {
            textTitle.color = Color.white;
        }
    }
    public void OnClickBtnBuy()
    {


    }


}
