using UnityEngine;
using System.Collections;
 
  public class TTS  
	{
        	public static void Speak(string text)
		{
            TTSBasePlatformWrapper platformWrapper = TTSPlatformWrapper.platform;
					 
            platformWrapper.Speak(text);
		}

 
    }

