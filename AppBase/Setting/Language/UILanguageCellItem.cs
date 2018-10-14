using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class UILanguageCellItem : UICellItemBase
{
    public Text textTitle;
    public Image imageBg;
    string[] strImageBg = { AppRes.IMAGE_CELL_BG_BLUE, AppRes.IMAGE_CELL_BG_ORINGE, AppRes.IMAGE_CELL_BG_YELLOW };

    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;
            textTitle.text = info.title;
            tagValue = info.tag;

            Vector4 border = AppRes.borderCellSettingBg;
            TextureUtil.UpdateImageTexture(imageBg, strImageBg[index % 3], false, border);
        }
    }

}



