using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnActionCompleteDelegate(GameObject obj);
public class ActionBase : MonoBehaviour
{
    public GameObject target;
    public float percentage;
    public float duration;//总时间
    float runningTime;


    bool reverse;
    bool isPaused;
    public OnActionCompleteDelegate callbackComplete { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        runningTime = 0;
        percentage = 0;
        reverse = false;
        isPaused = true;
        InitAction();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            UpdatePercentage();
            UpdateAction();
            OnFinish();
        }

    }


    public void UpdatePercentage()
    {
        runningTime += Time.deltaTime;

        if (duration != 0)
        {
            if (reverse)
            {
                percentage = 1 - runningTime / duration;
            }
            else
            {
                percentage = runningTime / duration;
            }
            //Debug.Log("UpdatePercentage:percentage=" + percentage);

        }


    }


    public void OnFinish()
    {
        if (percentage > 1f)
        {
            Pause();
            OnActionComplete();
            if (callbackComplete != null)
            {
                callbackComplete(this.gameObject);
            }
            Destroy(this);
        }
    }

    public void Pause()
    {
        isPaused = true;
    }
    public void Run()
    {
        isPaused = false;
    }


    public virtual void InitAction()
    {

    }
    public virtual void UpdateAction()
    {

    }

    public virtual void OnActionComplete()
    {

    }
}
