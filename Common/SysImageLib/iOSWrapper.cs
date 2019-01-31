#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.SysImageLib
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		[DllImport ("__Internal")] 
		public static extern void SysImageLib_SetObjectInfo(string objName,string objMethod);
		[DllImport ("__Internal")]
		public static extern void SysImageLib_OpenImage(); 
		[DllImport ("__Internal")]
	 	public static extern void SysImageLib_OpenCamera(); 
	  	 
   		 public override void SetObjectInfo(string objName, string objMethod)
        { 
            SysImageLib_SetObjectInfo( objName,objMethod);
        }

       public override void OpenImage()
        {
   			SysImageLib_OpenImage();
        }   
           public override void OpenCamera()
        {
   			SysImageLib_OpenCamera();	 
        }  
	}
}

#endif