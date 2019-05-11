using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaCellItemPintu : UICellItemBase
{

    public Text textTitle;
    public RawImage imageBg;
    public Image imageSel;
    public Image imageIconLock;

   
    public override void UpdateItem(List<object> list)
    {
        textTitle.text = (index + 1).ToString();
        textTitle.gameObject.SetActive(false);
        ItemInfo info = list[index] as ItemInfo;
        TextureUtil.UpdateRawImageTexture(imageBg, info.icon, true);
        imageSel.gameObject.SetActive(false);

        float scale = Common.GetBestFitScale(imageBg.texture.width, imageBg.texture.height, width, height);
        imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }


}
