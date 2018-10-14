using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        main = this;
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
}
