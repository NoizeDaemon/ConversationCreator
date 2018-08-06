using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour {

    public GameObject MessageBox;
    public Button OK;

    public bool OK_press;


    // Use this for initialization
    void Start () {
        OK.onClick.AddListener(OK_mouseup);
        OK_press = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OK_mouseup()
    {
        MessageBox.SetActive(false);
        OK_press = true;
    }
    
   
}
