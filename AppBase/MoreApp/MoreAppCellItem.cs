using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreAppCellItem : MonoBehaviour
{
    public Image imageItem;
    public Text textTitle;
    public Text textDetail;
    public UIViewLoading viewLoading;
    public int index;
    // Use this for initialization
    private string strAppUrl;
    bool isHideChild;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
         textTitle.gameObject.SetActive(false);
        textDetail.gameObject.SetActive(false);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickItem()
    {
        if (isHideChild)
        {
            return;
        }
        if (Common.BlankString(strAppUrl))
        {
            return;
        }
        Application.OpenURL(strAppUrl);
    }


    public void SetItemIndex(int idx)
    {
        index = idx;

    }

    public void UpdateInfo(ItemInfo info)
    {
        textTitle.text = info.title;
        textDetail.text = info.description;
        strAppUrl = info.url;

        viewLoading.Show(true);
        StartParsePic(info.pic);
    }

    public void Hide(bool isHide)
    {
        // imageItem.gameObject.SetActive(!isHide);
        // textTitle.gameObject.SetActive(!isHide);
        // textDetail.gameObject.SetActive(!isHide);
        HideChild(isHide);
    }


    void HideChild(bool isHide)
    {
        isHideChild = isHide;
        imageItem.gameObject.SetActive(!isHide);

        // foreach (Transform tr in transform)
        // {
        //     tr.gameObject.SetActive(!isHide);
        // }
    }

    void StartParsePic(string pic)
    {
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(pic);
    }


    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        if (isSuccess)
        {
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
