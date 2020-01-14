using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//汉字笔顺查询网站：
/*
http://bishun.shufaji.com/0x6211.html
https://bihua.51240.com/e68891__bihuachaxun/
 */
public class UIEditorWord : UIView
{
    public GameObject objLayoutBtn;
    public Text textTitle;
    public Button btnXml2Json;
    public Button btnKeyPoint;
    public InputField inputFieldWord;

    EditorWordScene editorWordScene;
    WordXmlPoint2Json wordXmlPoint2Json;
    void Awake()
    {
        editorWordScene = AppSceneBase.main as EditorWordScene;
        editorWordScene.textWord.text = "我";
        // wordXmlPoint2Json = this.gameObject.AddComponent<WordXmlPoint2Json>();
    }

    // Use this for initialization
    void Start()
    {
        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;

    }


    public void OnClickBtnXml2Json()
    {
        btnXml2Json.gameObject.SetActive(false);
        if (wordXmlPoint2Json != null)
        {
            wordXmlPoint2Json.OnClickBtnGuankaJson();
        }

    }


    public void OnInputValueChangedWord()
    {

    }

    public void OnInputEndWord()
    {
        // textWord.text = inputFieldWord.text;
        // UpdateWordInfo(textWord.text);

        // OnClickBtnClearAll();
    }
    public void OnClickBtnClearBihua()
    {
        // ClearAjustLine();
        // enableAjustline = false;
        // ClearLinePoint();
    }
    public void OnClickBtnClearAll()
    {
        // listDemoPointAll.Clear();
        // listGuidePointAll.Clear();

        // ClearAjustLine();
        // enableAjustline = false;

        // indexBihua = 0;
        // DestroyLetterBihuaGuide();
        // foreach (GameObject obj in listObjBihua)
        // {
        //     GameObject.DestroyImmediate(obj);
        // }
        // listObjBihua.Clear();
        // CreateLine();
    }

    public void OnClickBtnNextBihua()
    {
        // ClearAjustLine();
        // enableAjustline = false;

        // //先更新json
        // AddBiHua2Json();
        // indexBihua++;
        // CreateLine();
        // UpdateTitle();
    }

    public void OnClickBtnGuankaJson()
    {
        // btnXml2Json.gameObject.SetActive(false);
        // wordXmlPoint2Json.OnClickBtnGuankaJson();
    }

    public void OnClickBtnKeyPoint()
    {
        // isKeyPoint = !isKeyPoint;
        // string str = "NotKey";
        // if (isKeyPoint)
        // {
        //     str = "Key";
        // }
        // Text btnText = Common.GetButtonText(btnKeyPoint);
        // btnText.text = str;
    }

    public void OnClickBtnSaveBihua()
    {
        // ClearAjustLine();
        // enableAjustline = false;

        // if (textWord.gameObject.activeSelf)
        // {
        //     //先更新json
        //     AddBiHua2Json();

        //     //save  json 
        //     SaveJson(infoWord);

        //     //
        //     SaveTextWord();
        // }
        // else
        // {
        //     OnClickBtnSaveBihuaShow();
        // }

    }

    public void OnClickBtnSaveBihuaShow()
    {
        // Vector2 size = new Vector2(Screen.width, Screen.height);
        // Rect rc = Common.WorldToScreenRect(mainCam, rectWordWrite);

        // Debug.Log("rectWordWrite=:" + rectWordWrite + " rc=" + rc + " size=" + size);
        // WordItemInfo info = infoWord;
        // string filepath = rootDirImage + "/" + info.id + "/" + info.id + "_stroke.png";
        // Common.CaptureCamera(cameraBihuaShow, rc, filepath, size);//
    }


    // 书写动画演示
    public void OnClickBtnDemo()
    {
        // ClearAjustLine();
        // DestroyObjectWordWrite();
        // float t = 0.2f;
        // indexBihua = 0;
        // indexBihuaPoint = 0;
        // enableAjustline = false;
        // InvokeRepeating("OnTimerDemo", t, t);
    }

    //笔划示意图
    public void OnClickBtndBihuaShow()
    {

        // DestroyObjectWordWrite();
        // enableAjustline = true;
        // if (!wordBihuaShow.gameObject.activeSelf)
        // {
        //     wordBihuaShow.mainCamera = mainCam;
        //     wordBihuaShow.rectWordWrite = rectWordWrite;
        //     wordBihuaShow.UpdateItem(listDemoPointAll);
        //     enableAjustline = false;
        // }
        // wordBihuaShow.gameObject.SetActive(!wordBihuaShow.gameObject.activeSelf);
        // textWord.gameObject.SetActive(!textWord.gameObject.activeSelf);
        // objWordPoint.SetActive(!objWordPoint.activeSelf);
        // wordWriteAjustLine.gameObject.SetActive(!wordWriteAjustLine.gameObject.activeSelf);

    }

}
