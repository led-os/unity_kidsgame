using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlay : MonoBehaviour
{

    public static AudioPlay main;

    private AudioSource audioSource;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        AudioPlay.main = this;
        /* 
                if(instance==null){
                    instance = this;
                    DontDestroyOnLoad(this);
                }else if(this!=instance){
                    //防止重复创建
                    Destroy(this.gameObject);
                }
        */

    }
    // Use this for initialization
    void Start()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        Debug.Log("AudioPlay Start");
        if (ret)
        {
            audioSource.clip = AudioCache.main.Load("App/Audio/Bg");
            Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Stop()
    {
        audioSource.Stop();
    }
    public void Play()
    {
        Debug.Log("AudioPlay play()");
        audioSource.Play();
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }


    }

    public void PlayAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        audioSource.PlayOneShot(clip);
    }

    public void PlayFile(string audiofile)
    {
        AudioClip clip = AudioCache.main.Load(audiofile);
        if (clip == null)
        {
            return;
        }
        audioSource.PlayOneShot(clip);
    }


    public void PlayUrl(string url)
    {
        StartCoroutine(PlayUrlEnumerator(url));
    }

    //https://blog.csdn.net/qq_15386973/article/details/78696116 
    IEnumerator PlayUrlEnumerator(string url)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.LogError(uwr.error);
                yield break;
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
            // use audio clip
            PlayAudioClip(clip);
        }
    }
}
