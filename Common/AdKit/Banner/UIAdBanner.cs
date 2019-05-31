using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIAdBanner : UIView
{

    public const string PREFAB_UIAdBanner = "Common/Prefab/AdKit/Banner/UIAdBanner";
    public const string URL_AD_LIST = "https://6d6f-moonma-dbb297-1258816908.tcb.qcloud.la/adbanner/ad_list.json?sign=747d21f7431c81908c4ef07d19ae096b&t=1559294845";
    public Button btnClose;
    public Text textTitle;
    public Text textDetail;
    public RawImage imageBg;
    public RawImage imageIcon;
    public RawImage imageAd;
    float timeUpdate = 0.5f;//second
    public List<ItemInfo> listAd;

    HttpRequest httpReqBg;
    HttpRequest httpReqIcon;
    int indexAd;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listAd = new List<ItemInfo>();
        indexAd = 0;
        StartParseAd();
    }

    public void OnUpdateTime()
    {
        Invoke("OnUpdateTime", timeUpdate);
    }


    public void UpdateItem()
    {
        ItemInfo info = listAd[indexAd];
        textTitle.text = info.title;
        textDetail.text = info.detail;


        httpReqBg = new HttpRequest(OnHttpRequestFinishedImage);
        httpReqBg.Get(info.pic);

        httpReqIcon = new HttpRequest(OnHttpRequestFinishedImage);
        httpReqIcon.Get(info.icon);

        indexAd++;
        if (indexAd >= listAd.Count)
        {
            indexAd = 0;
        }
    }

    public override void LayOut()
    {
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        RectTransform rctranBg = imageBg.GetComponent<RectTransform>();
        float scale = 1f;
        float ratio = 1f;
        {
            scale = Common.GetBestFitScale(rctranBg.rect.width, rctranBg.rect.height, rctran.rect.width, rctran.rect.height) * ratio;
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        {
            float w = imageIcon.texture.width;//rectTransform.rect.width;
            float h = imageIcon.texture.height;//rectTransform.rect.height;
            scale = Common.GetBestFitScale(w, h, rctran.rect.height, rctran.rect.height);
            imageIcon.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

    }

    public void OnClickAd()
    {
    }

    public void OnClickAdInternal()
    {
        ItemInfo info = listAd[indexAd];
        string url = info.url;
        if (!Common.BlankString(url))
        {
            Application.OpenURL(url);
        }
    }

    public void OnClickBtnClose()
    {

    }
    public void StartParseAd()
    {
        listApp = new List<ItemInfo>();
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(URL_AD_LIST);
    }

    static public void parserJson(byte[] data, List<ItemInfo> list)
    {
        string str = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(str);
        JsonData appList = root["app"];
        string key = "";
        for (int i = 0; i < appList.Count; i++)

        {
            ItemInfo info = new ItemInfo();
            JsonData current = appList[i];

            info.pic = (string)current["pic"];
            info.icon = (string)current["icon"];
            key = "detail";
            if (Common.JsonDataContainsKey(current, key))
            {
                info.description = (string)current[key];
            }

            info.title = (string)current["title"];

            JsonData jsonPackage = current["PACKAGE"];
            key = Source.IOS;
            if (Common.isAndroid)
            {
                key = Source.ANDROID;
            }
            info.id = (string)jsonPackage[key];


            JsonData jsonAppId = current["APPID"];
            key = Source.APPSTORE;
            if (Common.isAndroid)
            {
                key = Source.TAPTAP;
            }

            if (Common.JsonDataContainsKey(jsonAppId, key))
            {
                info.appid = (string)jsonAppId[key];
            }


            JsonData jsonUrl = current["URL"];
            key = Source.IOS;
            if (Common.isAndroid)
            {
                key = Source.ANDROID;
            }
            info.url = (string)jsonUrl[key];

            string appname = Common.GetAppName();
            //(GetAppIdCur() != info.appid)
            if (!Common.BlankString(info.url) && (!appname.Contains(info.title)) && (!(Common.GetAppPackage() + ".pad").Contains(info.id)))
            {
                list.Add(info);

            }

        }
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // Debug.Log("MoreAppParser OnHttpRequestFinished"); 
        if (isSuccess)
        {
            parserJson(data, listAd);
            Invoke("OnUpdateTime", timeUpdate);
        }
    }



    void OnHttpRequestFinishedImage(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        //  return;
        if (isSuccess)
        {
            if (!GameViewController.main.isActive)
            {
                return;
            }
            Texture2D tex = LoadTexture.LoadFromData(data);
            RawImage image = null;
            if (httpReqIcon == req)
            {
                image = imageIcon;
            }
            if (httpReqBg == req)
            {
                image = imageBg;
            }

            TextureUtil.UpdateRawImageTexture(image, tex, true);

            LayOut();
        }
    }

}


