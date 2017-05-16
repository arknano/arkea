using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveMenu : MonoBehaviour {

	public RectTransform mainMenu;
	public RectTransform objectMenu;
    public GameObject moveRotRect;
	public RectTransform addMenu;
    public RectTransform materialMenu;

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

    public Text finishText;

    public GameObject topCam;
    public GameObject viewCam;

    bool isMoving;
    bool isRotating;
    bool moveRot;

    bool menuOpen;
    bool mainMenuOpen;
    bool placingObject;
    bool objectMenuOpen;
    bool materialMenuOpen;

    bool inAR;

    GameObject objectToPlace;

    public bool introPlaying;

    public RectTransform schnabel;
    public RectTransform arkea;
    public RectTransform blackBG;



	// Use this for initialization
	void Start () {
        StartCoroutine(PlayIntro());
    }

	void Update () {
        if (Application.isEditor || Input.touchCount == 1)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                if (!menuOpen && !moveRot)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit);
                    if (hit.collider != null && hit.collider.gameObject.tag == "Furniture" && !moveRot)
                    {
                        ChangeMenu(6);
                        selectedObject = hit.collider.gameObject.transform;
                    }
                    else if (hit.collider != null && !menuOpen && !moveRot)
                    {
                        //viewCam.GetComponent<Cardboard>().Recenter();
                        ChangeMenu(1);
                        if (hit.collider.gameObject.tag == "Floor")
                        {
                            reticule.position = hit.point;
                            LeanTween.move(topCam, new Vector3(reticule.position.x, topCam.transform.position.y, reticule.position.z), 0.5f).setEase(LeanTweenType.easeInOutQuad);
                        }
                    }
                }
            }

            if (moveRot)
            {
                if (Input.GetButton("Fire1"))
                {
                    print("fire");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit);
                    if (hit.collider != null && hit.collider.gameObject.tag == "Floor")
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        {
                            if (isMoving)
                            {
                                selectedObject.position = hit.point;
                            }
                            else
                            {
                                selectedObject.eulerAngles = new Vector3(0, Mathf.Atan2((Input.mousePosition.y - transform.position.y), (Input.mousePosition.x - transform.position.x)) * -Mathf.Rad2Deg - 90, 0);
                            }
                        }
                    }
                }
            }
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
                MoveOut(objectMenu);
                MoveOut(materialMenu);
                mainMenuOpen = false;
                objectMenuOpen = false;
                Timer.Register(0.1f, () =>
                    backgroundButton.SetActive(false)
                );
                moveRotRect.SetActive(false);
                materialMenuOpen = false;
                break;

            case 1://main menu
                menuOpen = true;
                MoveInSmall(mainMenu);
                MoveOut(addMenu);
                mainMenuOpen = true;
                Timer.Register(0.1f, () =>
                    backgroundButton.SetActive(true)
                );  
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

            case 6://object menu
                objectMenuOpen = true;
                menuOpen = true;
                MoveInSmall(objectMenu);
                MoveOut(materialMenu);
                materialMenuOpen = false;
                Timer.Register(0.1f, () =>
                    backgroundButton.SetActive(true)
                );
                break;

            case 7://moverot out
                MoveOut(objectMenu);
                objectMenuOpen = false;
                backgroundButton.SetActive(false);
                moveRotRect.SetActive(true);
                break;

            case 8://material menu
                MoveInLarge(materialMenu);
                MoveOut(objectMenu);
                objectMenuOpen = false;
                materialMenuOpen = true;
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
        if (mainMenuOpen || objectMenuOpen)
        {
            ChangeMenu(0);
        } else if (materialMenuOpen)
        {
            ChangeMenu(6);
        } else
        {
            ChangeMenu(1);
        }
    }


    public void MoveObject()
    {
        finishText.text = "FINISH MOVING";
        ChangeMenu(7);
        moveRot = true;
        isMoving = true;
    }

    public void RotatingObject()
    {
        finishText.text = "FINISH ROTATING";
        ChangeMenu(7);
        moveRot = true;
        isRotating = true;
    }

    public void FinishMoveRot()
    {
        ChangeMenu(0);
        moveRot = false;
        isRotating = false;
        isMoving = false;
    }

    public void SwitchToAR()
    {
        ChangeMenu(0);
        if (!inAR)
        {
            viewCam.transform.position = new Vector3(reticule.position.x, 1.8f, reticule.position.z);
            topCam.SetActive(false);
            viewCam.SetActive(true);
            viewCam.GetComponent<Cardboard>().Recenter();
            inAR = true;
        }
        else
        {
            viewCam.SetActive(false);
            topCam.SetActive(true);
            inAR = false;
        }
    }

    public void ChangeMaterial(Material mat)
    {
        selectedObject.GetComponent<Renderer>().material = mat;
    }

    IEnumerator PlayIntro()
    {
        LeanTween.alpha(schnabel, 1, 1);
        yield return new WaitForSeconds(3f);
        LeanTween.alpha(schnabel, 0, 1);
        yield return new WaitForSeconds(1f);
        LeanTween.alpha(arkea, 1, 1);
        yield return new WaitForSeconds(3f);
        LeanTween.alpha(arkea, 0, 1);
        yield return new WaitForSeconds(1f);
        ChangeMenu(0);
        LeanTween.alpha(blackBG, 0, 1);
    }
}
