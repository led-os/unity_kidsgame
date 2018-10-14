using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public delegate void OnParentGateLongPressDidPressDelegate(ParentGateLongPress press);
public class ParentGateLongPress : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	bool isTouchDown = false;
	public int index;
	public OnParentGateLongPressDidPressDelegate callbackPress { get; set; }

	// Use this for initialization
	void Start () {
		 isTouchDown = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isTouchDown = true;
		Text text = this.gameObject.GetComponent<Text>();
		Debug.Log("OnPointerDown:"+text.text);

		CancelInvoke("OnLongPress");
		Invoke("OnLongPress",2f);
	} 

		public void OnPointerUp(PointerEventData eventData)
	{
		isTouchDown = false;
		Debug.Log("OnPointerUp");
	} 


	 void OnLongPress()
    {
		if(isTouchDown){
			//长按
			Debug.Log("OnLongPress");
			if(this.callbackPress!=null){
				this.callbackPress(this);
			}
		}
	}

}
