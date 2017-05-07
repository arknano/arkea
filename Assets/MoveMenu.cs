using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class MoveMenu : MonoBehaviour {

	public RectTransform mainMenu;
	public RectTransform objectMenu;
	public RectTransform addMenu;

	public GameObject kitchenMenu;
	public GameObject diningMenu;
	public GameObject livingMenu;

	public float moveTime;
	public Vector3 moveInPos;
    public Vector3 moveOutPos;

	public GameObject chairPrefab;

    RectTransform rect;

	// Use this for initialization
	void Start () {
        rect = gameObject.GetComponent<RectTransform>();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			if (hit.collider.gameObject.tag == "Floor") {
				Instantiate (chairPrefab, hit.point, Quaternion.identity);
			}
		}
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
