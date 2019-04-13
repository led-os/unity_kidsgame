
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.Media
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS_AD = "com.moonma.common.AdBannerCommon";
        public   void InitAd(string source)
		{ 
		Debug.Log("AndroidWrapper:InitAd");

				using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					Debug.Log("AndroidWrapper:InitAd CallStatic");
					javaClass.CallStatic("adBanner_setAd",source);
				}
		}

	   public override void Open(string url)
        {

        }

        public override void Close()
        {

        }

        public override void Play()
        {

        }
        public override void Pause()
        {

        }

    }
}
#endif
