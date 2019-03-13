
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
    public UIViewLoading viewLoading;
    public override void UpdateItem(List<object> list)
    {
        // return;
        if (index < list.Count)
        {
            ImageItemInfo info = list[index] as ImageItemInfo;
            StartParsePic(info.pic);

            // Vector4 border = AppRes.borderCellSettingBg;
            // TextureUtil.UpdateImageTexture(imagePic, "", true);
        }
    }
    public override void LayOut()
    {
        RectTransform rctran = imagePic.GetComponent<RectTransform>();
        float ratio = 0.9f;
        float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
        imagePic.transform.localScale = new Vector3(scale, scale, 1.0f);
    }

    void StartParsePic(string pic)
    {
        if (Common.BlankString(pic))
        {
            return;
        }
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(pic);
        viewLoading.Show(true);
        if (http.isReadFromCatch)
        {
            viewLoading.Show(false);
        }
    }
    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        if (isSuccess)
        {

            Texture2D tex = LoadTexture.LoadFromData(data);
            if (!req.isReadFromCatch)
            {
                //imageItem.GetComponent<Animation>().Play();
            }
            TextureUtil.UpdateImageTexture(imagePic, tex, true);
            LayOut();
            viewLoading.Show(false);
        }
        else
        {

        }
    }
}

