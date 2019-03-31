using UnityEngine;
using System.Collections;

public class TTSApiBase
{
    public string textSpeak;
    public virtual string GetTextUrl(string text)
    {
        return "";
    }
    public virtual void SpeakWeb(string text)
    {

    }

}

