using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWordListCellItem : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public override void UpdateItem(List<object> list)
    {
        WordItemInfo info = list[index] as WordItemInfo;
        textTitle.text = info.dbInfo.word;
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        {
            // RectTransform rctran = imageBg.GetComponent<RectTransform>();
            // float w = imageBg.texture.width;//rectTransform.rect.width;
            // float h = imageBg.texture.height;//rectTransform.rect.height;
            // RectTransform rctranCellItem = objContent.GetComponent<RectTransform>();

            // float scalex = width / w;
            // float scaley = height / h;
            // float scale = Mathf.Min(scalex, scaley);
            // scale = scale * 0.8f;
            // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

}
