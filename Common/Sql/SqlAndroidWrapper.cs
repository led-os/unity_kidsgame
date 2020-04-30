#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
 
internal class SqlAndroidWrapper : SqlBasePlatformWrapper
	{

	public const string JAVA_CLASS_TTS = "com.moonma.sql";
	public override void OpenDB(string dbfile)
    {
		using(var javaClass = new AndroidJavaClass(JAVA_CLASS_TTS))
				{ 
					javaClass.CallStatic("OpenDB");
				}

    }

    public override void CloseDB() 

    {

    }

    }
#endif

