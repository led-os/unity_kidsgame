using UnityEngine;
using System.Collections;
 
  public class TTS  
	{
        	public static void Speek(string text)
		{
            TTSBasePlatformWrapper platformWrapper = TTSPlatformWrapper.platform;
					 
            platformWrapper.Speek(text);
		}

 
    }

