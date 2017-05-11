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
	public Vector3 moveInLargePos;
    public Vector3 moveInSmallPos;
    public Vector3 moveOutPos;

	public GameObject chairPrefab;

    public GameObject backgroundButton;

    public Transform reticule;

    public Transform selectedObject;

    RectTransform rect;

    bool isMoving;
    bool isRotating;

    bool menuOpen;
    bool mainMenuOpen;
    bool placingObject;

    GameObject objectToPlace;
    public enum MenuState
    {
        None,
        MainMenu,
        AddMenu
    }

    MenuState menuState;


	// Use this for initialization
	void Start () {
        rect = addMenu;
	}

	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!menuOpen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.gameObject.tag == "Furniture")
                {
                    ChangeMenu(1);
                    selectedObject = hit.collider.gameObject.transform.parent;
                } else if (hit.collider.gameObject.tag == "Floor" && !menuOpen)
                {
                    ChangeMenu(1);
                    reticule.position = hit.point;
                }
            }
        }
        if (menuOpen)
        {
            backgroundButton.SetActive(true);
        } else
        {
            backgroundButton.SetActive(false);
        }

        if (isMoving)
        {
            if (Input.GetButton("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.gameObject.tag == "Floor")
                {
                    selectedObject.position = hit.point;
                }
            }
        }

        if (isRotating)
        {
            selectedObject.eulerAngles = new Vector3(0, Mathf.Atan2((Input.mousePosition.y - transform.position.y), (Input.mousePosition.x - transform.position.x)) * -Mathf.Rad2Deg - 90, 0);
        }

    }

    public void ChangeMenu(int menuState)
    {
        switch (menuState)
        {
            case 0://no menu
                menuOpen = false;
                MoveOut(addMenu);
                MoveOut(mainMenu);
                mainMenuOpen = false;
                break;

            case 1://main menu
                menuOpen = true;
                MoveInSmall(mainMenu);
                MoveOut(addMenu);
                mainMenuOpen = true;
                break;

            case 2://add menu
                MoveOut(mainMenu);
                MoveInLarge(addMenu);
                mainMenuOpen = false;
                break;

            case 3://living menu
                MoveOut(mainMenu);
                MoveInLarge(addMenu);
                livingMenu.SetActive(true);
                kitchenMenu.SetActive(false);
                diningMenu.SetActive(false);
                break;

            case 4://kitchen menu
                MoveOut(mainMenu);
                MoveInLarge(addMenu);
                livingMenu.SetActive(false);
                kitchenMenu.SetActive(true);
                diningMenu.SetActive(false);
                break;

            case 5://dining menu
                MoveOut(mainMenu);
                MoveInLarge(addMenu);
                livingMenu.SetActive(false);
                kitchenMenu.SetActive(false);
                diningMenu.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    public void MoveInLarge (RectTransform rect) {
        LeanTween.move(rect, moveInLargePos, moveTime).setEase(LeanTweenType.easeInOutQuad);
    }

    public void MoveInSmall(RectTransform rect)
    {
        LeanTween.move(rect, moveInSmallPos, moveTime).setEase(LeanTweenType.easeInOutQuad);
    }

    public void MoveOut(RectTransform rect)
    {
        LeanTween.move(rect, moveOutPos, moveTime).setEase(LeanTweenType.easeInOutQuad);
    }

    public void PlaceObject(GameObject prefab)
    {
        Instantiate(prefab, reticule.position, Quaternion.identity);
        ChangeMenu(0);
    }

    public void ExitMenuCheck ()
    {
        if (mainMenuOpen)
        {
            ChangeMenu(0);
        } else
        {
            ChangeMenu(1);
        }
    }


    public void MoveObject()
    {
        isMoving = true;
    }

    public void RotatingObject()
    {
        isRotating = true;
    }
}
