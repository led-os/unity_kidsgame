using UnityEngine;
using System.Collections;

public class TTS
{
    TTSApiBase ttsApi;

    static private TTS _main = null;
    public static TTS main
    {
        get
        {
            if (_main == null)
            {
                _main = new TTS();
                _main.Init();
            }
            return _main;
        }
    }

    public void Init()
    {
        ttsApi = new TTSApiBaidu();
    }

    public void Speak(string text)
    {
        if (Common.isAndroid || Common.isiOS)
        {
            TTSBasePlatformWrapper platformWrapper = TTSPlatformWrapper.platform;

            platformWrapper.Speak(text);
        }
        if (Common.isWinUWP)
        {
            SpeakWeb(text);
        }
    }

    public void SpeakWeb(string text)
    {
        if (ttsApi != null)
        {
            ttsApi.SpeakWeb(text);
        }
    }



}

