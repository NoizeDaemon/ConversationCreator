using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using TMPro;

public class PlayerSelectionButtonBehaviour : MonoBehaviour {

    public List<Button> selectionBtn;
    public int btnCount;
    public RectTransform textRect;
    public GameObject upInd;
    public GameObject downInd;

    public bool btnIsClicked;
    public int choice;

    private Vector3 textRectInitPos;
    private float page_h;
	
	void Start ()
    {
        textRectInitPos = textRect.transform.localPosition;
    }

    public void Initialize()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);

        GameObject go = this.gameObject;
        selectionBtn.AddRange(go.GetComponentsInChildren<Button>());
        foreach (Button btn in selectionBtn)
        {
            btn.onClick.AddListener(delegate { OnClick(btn.gameObject.name); });
        }
        //refresh content size fitter
        //textRect.GetComponent<ContentSizeFitter>().enabled = false;
        
        
        //Get vertical size of scroll rect
        page_h = textRect.rect.height;
        ShowScrollIndicator();
    }

    // Update is called once per frame
    void Update ()
    {
        //if(Input.GetKey("down") || Input.GetKey("s"))
        //{
        //    Debug.Log("Down!");
        //}
        //if (Input.GetKey("up") || Input.GetKey("w"))
        //{
        //    Debug.Log("Up!");
        //}
    }

    //public void BtnSelected(string btnName)
    //{
    //    //Debug.Log("Selected =" + btnName);
    //}

    public void OnClick(string btnName)
    {
        //Debug.Log("Clicked =" + btnName);
        choice = int.Parse(btnName.Substring(6));
        //Debug.Log("Choice -> " + choice);
        btnIsClicked = true;
    }

    public void AdjustScrollRect()
    {
        //Still do to
    }

    public void ShowScrollIndicator()
    {
        if (page_h > 125)
        {
            //Debug.Log("Page_h is bigger than 125");
            if (textRect.localPosition.y > textRectInitPos.y) upInd.SetActive(true);
            else upInd.SetActive(false);
            if (-textRect.localPosition.y + textRect.rect.height > 130) downInd.SetActive(true);
            else downInd.SetActive(false);
            //Debug.Log(textRect.localPosition.y);
        }
        else
        {
            downInd.SetActive(false);
            upInd.SetActive(false);
        }
    }
}
