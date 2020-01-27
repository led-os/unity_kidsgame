using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutLine : MonoBehaviour {
private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		bool outline = true;
       Color color = Color.blue;
		spriteRenderer = GetComponent<SpriteRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
       // spriteRenderer.SetPropertyBlock(mpb);
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
