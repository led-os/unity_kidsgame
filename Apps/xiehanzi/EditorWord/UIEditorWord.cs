using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEditorWord : UIView
{
    public GameObject objLayoutBtn;
    WordXmlPoint2Json wordXmlPoint2Json;
    void Awake()
    {
        wordXmlPoint2Json = this.gameObject.AddComponent<WordXmlPoint2Json>();
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
        //btnXml2Json.gameObject.SetActive(false);
        wordXmlPoint2Json.OnClickBtnGuankaJson();
    }
}
