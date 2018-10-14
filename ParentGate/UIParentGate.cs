using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tacticsoft;

public delegate void OnUIParentGateDidCloseDelegate(UIParentGate ui, bool isLongPress);
public class UIParentGate : UIView
{
    //中文
    static public string[] ParentWord_CN = { "国", "围", "团", "圆" };
    static public string[] ParentDuyin_CN = { "guo", "wei", "tuan", "yuan" };

    //英文
    static public string[] ParentWord_EN = { "A", "B", "C", "D" };
    static public string[] ParentDuyin_EN = { "eɪ", "bi:", "si:", "di:" };
    int wordNum = 4;

    public GameObject objContent;
    public Text textTitle;
    public Text textWord0;
    public Text textWord1;
    public Text textWord2;
    public Text textWord3;



    int indexSelectWord;

    public OnUIParentGateDidCloseDelegate callbackClose { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {


        indexSelectWord = Random.Range(0, wordNum);
    }
    // Use this for initialization
    void Start()
    {

        {
            //如果您是家长请按[yuan]2秒以上
            string[] arrayDuyin = GetParentDuyin();
            string str = Language.main.GetString("STR_PARENT_GATE_TITLE_HEAD") + "<color=#FF0000FF>" + "[" + arrayDuyin[indexSelectWord] + "]" + "</color>" + Language.main.GetString("STR_PARENT_GATE_TITLE_END");
            textTitle.text = str;
        }

        ParentGateLongPress longPress = null;
        string[] arrayWord = GetParentWord();

        textWord0.text = arrayWord[0];
        longPress = textWord0.GetComponent<ParentGateLongPress>();
        longPress.callbackPress = OnParentGateLongPressDidPress;
        longPress.index = 0;

        textWord1.text = arrayWord[1];
        longPress = textWord1.GetComponent<ParentGateLongPress>();
        longPress.callbackPress = OnParentGateLongPressDidPress;
        longPress.index = 1;

        textWord2.text = arrayWord[2];
        longPress = textWord2.GetComponent<ParentGateLongPress>();
        longPress.callbackPress = OnParentGateLongPressDidPress;
        longPress.index = 2;

        textWord3.text = arrayWord[3];
        longPress = textWord3.GetComponent<ParentGateLongPress>();
        longPress.callbackPress = OnParentGateLongPressDidPress;
        longPress.index = 3;

        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }

    public override void LayOut()
    {
        float w = 0, h = 0;
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            w = Mathf.Min(this.frame.width, this.frame.height) * 0.8f;
            h = w;
            rctran.sizeDelta = new Vector2(w, h);

        }
    }
    string[] GetParentWord()
    {
        string[] ret = ParentWord_EN;
        if (Language.main.GetLanguage() == SystemLanguage.Chinese)
        {
            ret = ParentWord_CN;
        }
        if (Language.main.GetLanguage() == SystemLanguage.English)
        {
            ret = ParentWord_EN;
        }

        return ret;
    }

    string[] GetParentDuyin()
    {
        string[] ret = ParentDuyin_EN;
        if (Language.main.GetLanguage() == SystemLanguage.Chinese)
        {
            ret = ParentDuyin_CN;
        }
        if (Language.main.GetLanguage() == SystemLanguage.English)
        {
            ret = ParentDuyin_EN;
        }

        return ret;
    }


    public void OnParentGateLongPressDidPress(ParentGateLongPress press)
    {
        if (indexSelectWord == press.index)
        {
            //right click by parent
            Debug.Log("OnParentGateLongPressDidPress");
            if (this.callbackClose != null)
            {
                Debug.Log("OnParentGateLongPressDidPress press=true");
                this.callbackClose(this, true);
            }
            OnClickBtnBack();
        }
    }

    public void OnClickBtnBack()
    {

        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }




}
