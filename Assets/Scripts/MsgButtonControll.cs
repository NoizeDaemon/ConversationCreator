using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MsgButtonControll : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{

    public TextMeshProUGUI msg;
    public RectTransform text_rect;
    public RectTransform tmp_rect;
    private int active_page;
    private int page_count;
    private float page_h;
    private Vector3 text_rect_initPos;
    public GameObject prev_btn;
    public GameObject next_btn;
    public GameObject end_btn;
    public GameObject down_ind;
    public GameObject up_ind;
    public bool endIsClicked;

    // Use this for initialization
    void Start()
    {
        text_rect_initPos = text_rect.transform.localPosition;
        //Debug.Log(text_rect_initPos);
        //prev_btn = GameObject.Find("Message Window Test/Previous");
        //next_btn = GameObject.Find("Message Window Test/Next");
        //end_btn = GameObject.Find("Message Window Test/End");
        Initialize();
    }
    // Update is called once per frame

    public void Initialize()
    {
        msg.ForceMeshUpdate();
        page_count = msg.textInfo.pageCount;
        Debug.Log("PageCount = " + page_count);
        active_page = 1;
        msg.pageToDisplay = active_page;
        prev_btn.SetActive(false);
        end_btn.SetActive(false);
        next_btn.SetActive(false);
        CheckPageCount();
        AdjustScrollRect();
    }

    void LateUpdate()
    {
        
    }

    public void Next()
    {
        active_page += 1;
        active_page = Mathf.Clamp(active_page, 1, page_count);
        msg.pageToDisplay = active_page;
        CheckActivePage();
        AdjustScrollRect();
    }

    public void Prev()
    {
        active_page -= 1;
        active_page = Mathf.Clamp(active_page, 1, page_count);
        msg.pageToDisplay = active_page;
        CheckActivePage();
        AdjustScrollRect();
    }

    public void End()
    {
        endIsClicked = true;
    }

    public void CheckPageCount()
    {
        if(page_count > 1) next_btn.SetActive(true);
        else end_btn.SetActive(true);
    }

    public void CheckActivePage()
    {
        if(active_page == page_count)
        {
            next_btn.SetActive(false);
            end_btn.SetActive(true);
        }
        else
        {
            next_btn.SetActive(true);
            end_btn.SetActive(false);
        }
        if(active_page > 1) prev_btn.SetActive(true);
        else prev_btn.SetActive(false);
    }

    public void AdjustScrollRect()
    {
        msg.ForceMeshUpdate();
        int c = msg.textInfo.pageInfo[active_page-1].lastCharacterIndex;
        Debug.Log("Last Char = " + msg.textInfo.characterInfo[c].character);
        page_h = Mathf.Abs(msg.textInfo.characterInfo[c].bottomRight.y - text_rect_initPos.y)+5;
        text_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Clamp(page_h, 125, float.PositiveInfinity));
        text_rect.ForceUpdateRectTransforms();

        ShowScrollIndicator();
    }

    public void ShowScrollIndicator()
    {
        if(page_h > 125)
        {
            //Debug.Log("Page_h is bigger than 125");
            if (text_rect.localPosition.y > text_rect_initPos.y) up_ind.SetActive(true);
            else up_ind.SetActive(false);
            if (-text_rect.localPosition.y + text_rect.rect.height > 130) down_ind.SetActive(true);
            else down_ind.SetActive(false);
            //Debug.Log(text_rect.localPosition.y);
        }
        else
        {
            down_ind.SetActive(false);
            up_ind.SetActive(false);
        }
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    isHoveringObject = true;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    isHoveringObject = false;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    switch (link_index)
    //    {
    //        case -1:
    //            Debug.Log("No link is clicked.");
    //            break;
    //        case 0:
    //            Debug.Log("Link 0 is clicked");
    //            break;
    //        case 1:
    //            Debug.Log("Link 1 is clicked");
    //            break;
    //    }
    //}


    ////only for testing v
    //public void OnLinkSelectionTest()
    //{
    //    link_index = TMP_TextUtilities.FindIntersectingLink(msg, Input.mousePosition, null);
    //    if (link_index != -1 && link_index != selected_link)
    //    {
    //        selected_link = link_index;
    //        TMP_LinkInfo linkInfo = msg.textInfo.linkInfo[link_index];

    //        Debug.Log("Link ID: \"" + linkInfo.GetLinkID() + "\"   Link Text: \"" + linkInfo.GetLinkText() + "\"");
    //        for(int i = 0; i < linkInfo.linkTextLength; i++)
    //        {
    //            TMP_CharacterInfo cInfo = msg.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex + i];
    //            //if (!cInfo.isVisible) continue; // Skip invisible characters.
    //            Debug.Log(cInfo.character);
    //            cInfo.pointSize = cInfo.pointSize *2;//new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    //        }
    //    }

    //}
    //only for testing ^
}
