using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoreAppCellItem : UICellItemBase
{
    public Image imageItem;
    public Text textTitle;
    public Text textDetail;
    public UIViewLoading viewLoading;
    // Use this for initialization
    private string strAppUrl;
    bool isHideChild;

    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        textTitle.text = info.title;
        textDetail.text = info.description;
        strAppUrl = info.url;

        textTitle.gameObject.SetActive(false);
        textDetail.gameObject.SetActive(false);

        StartParsePic(info.pic);

    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }


    void StartParsePic(string pic)
    {
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
     //  return;
           if (isSuccess)
        {
            if (!MoreViewController.main.isActive)
            {
                return;
            }
            Texture2D tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            tex.LoadImage(data);
            Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            imageItem.sprite = sp;
            if (!req.isReadFromCatch)
            {
                //imageItem.GetComponent<Animation>().Play();
            }

            RectTransform rctran = imageItem.GetComponent<RectTransform>();
            float w = imageItem.sprite.texture.width;//rectTransform.rect.width;
            float h = imageItem.sprite.texture.height;//rectTransform.rect.height;
                                                      // print("imageItem size:w=" + w + " h=" + h);
            rctran.sizeDelta = new Vector2(w, h);

            RectTransform rctranCellItem = this.gameObject.GetComponent<RectTransform>();
            float scalex = rctranCellItem.rect.width / w;
            float scaley = rctranCellItem.rect.height / h;
            float scale = Mathf.Min(scalex, scaley);
            imageItem.transform.localScale = new Vector3(scale, scale, 1.0f);

            viewLoading.Show(false);
        }
        else
        {

        }
    }
}
