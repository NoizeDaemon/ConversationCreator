using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlighter : MonoBehaviour {

    public SpriteRenderer scene_Spriterenderer;
    public Sprite Highlightsprite;
    public AudioSource Speaker;
    public AudioSource Speaker2;
    public AudioClip SFX1;
    public AudioClip SFX2;
    public AudioClip SFX3;
    public string Message;
    public string Message2;
    public bool Special;

    public MessageHandler MessageHandler;

    public GameObject MessageBox;
    public Text display_Text;

    private bool prev_Interaction;
    private bool sec_Interaction;

 
	void Start () {
         
	}
	
	
	void Update () {

	}

    private void OnMouseOver()
    {
        if(prev_Interaction == false)
        {

        }
        else if (Message2 == "")
        {
            scene_Spriterenderer.color = new Color32(0x67, 0x67, 0x67, 0xFF);
        }
        else if (Message2 != "")
        {
            scene_Spriterenderer.color = new Color32(0x53, 0xFF, 0x00, 0xFF);
        }

        if (sec_Interaction == true)
        {
            scene_Spriterenderer.color = new Color32(0x67, 0x67, 0x67, 0xFF);
        }

        scene_Spriterenderer.sprite = Highlightsprite;
    }

    private void OnMouseExit()
    {
        scene_Spriterenderer.sprite = null;
        scene_Spriterenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    }

    private void OnMouseUpAsButton()
    {
        Speaker.clip = SFX1;
        Speaker.Play(0);
        if (prev_Interaction == false || Message2 == "")
        {
            display_Text.text = Message;
        }
        else
        {
            display_Text.text = Message2;
        }
        MessageBox.SetActive(true);
        StartCoroutine(Wait());

    }

    private IEnumerator Wait()
    {
        if(Special == true && prev_Interaction == false)
        {
            //print("Start to wait.");
            yield return new WaitForSeconds(1);
            //print("Finished waiting.");
            Speaker2.clip = SFX3;
            Speaker2.Play();
        }

        while (MessageHandler.OK_press == false)
        {
            yield return null;
        }

        OK_press();
        StopCoroutine(Wait());
    }

    private void OK_press()
    {
        if(prev_Interaction == false)
        {
            prev_Interaction = true;
        }
        else if(prev_Interaction == true && Message2 != "")
        {
            sec_Interaction = true;
        }

        Speaker.Stop();
        Speaker.clip = SFX2;
        Speaker.Play(0);
        MessageHandler.OK_press = false;
    }
}
