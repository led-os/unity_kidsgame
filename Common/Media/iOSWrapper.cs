#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.Media
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		 
		//[DllImport ("__Internal")]
		//public static extern void AdBanner_InitAd(string source); 
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