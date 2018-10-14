using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViewLoading : MonoBehaviour {

	// Use this for initialization
	public Image imageBg;
	public Image imageLoading;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show(bool isShow){
		this.gameObject.SetActive(isShow);
	}
}
