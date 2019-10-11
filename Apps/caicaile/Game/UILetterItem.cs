
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UILetterItem : UIView
{
    public enum Type
    {
        Connect = 0,
    }

    public enum Status
    {
        LOCK = 0,
        UNLOCK = 1,
        DUPLICATE = 2,//重复连线
        HIDE = 3,
        LOCK_SEL = 4,
        LOCK_UNSEL = 5,
        RIGHT_ANSWER = 6,
        ERROR_ANSWER = 7,
    }
    public Image imageBg;
    public Image imageIcon;
    public Text textTitle;
    public int indexRow;
    public int indexCol;
    public int index;
    public string wordDisplay;
    public string wordAnswer;
    Status status;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetStatus(Status.LOCK);
        LayOut();
    }


    public override void LayOut()
    {

    }
    public void SetStatus(Status st)
    {
        status = st;
        if (st == Status.LOCK)
        {
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(true);

        }

        if (st == Status.LOCK_SEL)
        {
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);

        }
        if (st == Status.LOCK_UNSEL)
        {
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);

        }
        if (st == Status.DUPLICATE)
        {
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);

            // 10 秒内， 物体 X,Y,Z 旋转角度在 自身-5 到 自身加 5 之间震动 
            this.transform.DOShakeRotation(1f, new Vector3(0, 0, 30)).OnComplete(() =>
               {
                   SetStatus(Status.UNLOCK);
               });
        }

        if (st == Status.UNLOCK)
        {
            textTitle.color = Color.black;
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }
        if (st == Status.HIDE)
        {
            imageBg.gameObject.SetActive(false);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);
        }

        if (st == Status.RIGHT_ANSWER)
        {
            textTitle.color = Color.white;
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }
        if (st == Status.ERROR_ANSWER)
        {
            textTitle.color = Color.red;
            imageBg.gameObject.SetActive(true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }

    }

    public Status GetStatus()
    {
        return status;
    }

    public void UpdateItem(string letter)
    {
        wordDisplay = letter;
        textTitle.text = letter;
    }
}
