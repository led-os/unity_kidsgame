using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentItem : MonoBehaviour
{
    public Image imageSel;
    public Text textTitle;
    public int index;
    public Color colorSel = Color.red;
    public Color colorUnSel = Color.white;

 private ISegmentItemDelegate _delegate;

    public ISegmentItemDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	public void UpdateInfo(ItemInfo info)
    {
		textTitle.color = colorUnSel;
		textTitle.text = info.title;
    }
	public void SetSelect(bool isSel)
	{
		if(isSel){
			textTitle.color = colorSel;
		}else{
			textTitle.color = colorUnSel;
		}
	}
	public void OnClick()
	{
		 if (_delegate != null)
        {
            _delegate.SegmentItemDidClick(this);
        }
	}
}
