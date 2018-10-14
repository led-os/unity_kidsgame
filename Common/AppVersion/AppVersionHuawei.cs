using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public class AppVersionHuawei : AppVersionBase
{
    //http://appstore.huawei.com/app/C100270155
    public override void StartParseVersion()
    {
        string strappid = Config.main.GetAppIdOfStore(Source.HUAWEI);

        string url = "http://appstore.huawei.com/app/C" + strappid;
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
        string ptmpversion_start = "版本： ";
        string ptmpversion_mid = "<span>";
        string ptmpversion_end = "</span>";

        int idx = strData.IndexOf(ptmpversion_start);
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



        ParseFinished(this);
    }
}
