
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using Moonma.AdKit.AdConfig;
//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UIAdOfferWallCellItem : UICellItemBase
{
    public Text textTitle;
    public Image imageBg;
    public Button btnShow;
    ItemInfo infoItem;
    string[] strImageBg = { AppRes.IMAGE_CELL_BG_BLUE, AppRes.IMAGE_CELL_BG_ORINGE, AppRes.IMAGE_CELL_BG_YELLOW };
    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;
            infoItem = info;
            textTitle.text = info.title + "(appid:" + AdConfig.main.GetAppId(info.source) + ")";

            Vector4 border = AppRes.borderCellSettingBg;
            TextureUtil.UpdateImageTexture(imageBg, strImageBg[index % 3], false, border);
        }
    }

    public void OnClickBtnShow()
    {
    }

}

