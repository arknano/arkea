using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class MoveMenu : MonoBehaviour {

    public float moveTime;

    public Vector3 moveInPos;
    public Vector3 moveOutPos;

    RectTransform rect;

	// Use this for initialization
	void Start () {
        rect = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	public void MoveIn () {
        LeanTween.move(rect, moveInPos, moveTime).setEase(LeanTweenType.easeInOutQuad);
    }

    	public void MoveOut()
    {
        LeanTween.move(rect, moveOutPos, moveTime).setEase(LeanTweenType.easeInOutQuad);
    }
}
