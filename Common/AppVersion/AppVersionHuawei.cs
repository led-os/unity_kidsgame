using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public class AppVersionHuawei : AppVersionBase
{
    //http://appstore.huawei.com/app/C100270155
    //new:https://appgallery1.huawei.com/#/app/C100270155
    public override void StartParseVersion()
    {
        string strappid = Config.main.GetAppIdOfStore(Source.HUAWEI);
        // strappid = "100270155";
        string url = "https://appgallery1.huawei.com/#/app/C" + strappid;
        Debug.Log("version huawei url=" + url);
        strUrlAppstore = url;
        strUrlComment = url;
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
    }

    public override void ParseData(byte[] data)
    {
        string strData = Encoding.UTF8.GetString(data);

        /*
        <li class="ul-li-detail">版本： <span>1.1.1</span>
        </li>

         */

        //<span itemprop="softwareVersion" class="info-item-content">1.0.5</span>
        // string ptmpversion = "softwareVersion"; 
        // <div data-v-684c8842="" class="info_val">2.1.7</div>
        string ptmpversion_start = "版本"; //版本 Version
        string ptmpversion_mid = "info_val\">";
        string ptmpversion_end = "</div>";


        int idx = strData.IndexOf(ptmpversion_start);
        Debug.Log("version huawei strData=" + strData);
        if (idx >= 0)
        {
            string ptmp = strData.Substring(idx);
            idx = ptmp.IndexOf(ptmpversion_mid);
            if (idx >= 0)
            {
                string p = ptmp.Substring(idx + ptmpversion_mid.Length);
                idx = p.IndexOf(ptmpversion_end);
                if (idx >= 0)

                {
                    string version = p.Substring(0, idx);
                    strVersionStore = version;
                }
            }

        }
        Debug.Log("version huawei =" + strVersionStore);


        ParseFinished(this);
    }
}
