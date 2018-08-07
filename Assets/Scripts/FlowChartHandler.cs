using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class FlowChartHandler : MonoBehaviour {


    //flowchart
    public Text[] fc_player_layer0 = new Text[4];
    public Text[] fc_npc_layer0 = new Text[4];
    public Text[] fc_player_layer1 = new Text[16];
    public Text[] fc_npc_layer1 = new Text[16];
    public Text[] fc_player_layer2 = new Text[64];
    public Text[] fc_npc_layer2 = new Text[64];

    private bool doThisOnce;

    public Conversation conv;

    // Use this for initialization
    void Awake () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (!doThisOnce)
        {
            GameObject go = this.gameObject;
            conv = go.GetComponent<Conversation>();
            doThisOnce = true;
        }
	}

    [ExecuteInEditMode]
    public void Initialize()
    {
        //This could be fully variable
        //but for the sake of simplicity we'll stick to a 4-4-4 chart for now
        //resulting in 64 elements in layer 2

        //i < # of paths (Layer 0)
        for(int i = 0; i < 4; i++)
        {
            //Layer 0
            fc_player_layer0[i] = GameObject.Find("Text_P" + i).GetComponent<Text>();
            Debug.Log(string.Format("Text_P{0} initialized!", i));
            fc_npc_layer0[i] = GameObject.Find("Text_N" + i).GetComponent<Text>();
            Debug.Log(string.Format("Text_N{0} initialized!", i));

            //n < # of subpaths (Layer 1)
            for (int n = 0; n < 4; n++)
            {
                //Layer 1
                //i * # of subpaths
                fc_player_layer1[i*4 + n] = GameObject.Find("Text_P"+i+"_"+n).GetComponent<Text>();
                Debug.Log("Text_P" + i + "_" + n);
                fc_npc_layer1[i * 4 + n] = GameObject.Find("Text_N" + i + "_" + n).GetComponent<Text>();
                Debug.Log("Text_N" + i + "_" + n);

                //m < # of subsubpaths (Layer 2)
                for (int m = 0; m < 4; m++)
                {
                    //Layer 2
                    //i * # of subpaths * # subsubpaths + n * # of subsubpaths
                    fc_player_layer2[i*16 + n*4 + m] = GameObject.Find("Text_P" + i + "_" + n+"_"+m).GetComponent<Text>();
                    Debug.Log("Text_P" + i + "_" + n + "_" + m);
                    fc_npc_layer2[i * 16 + n * 4 + m] = GameObject.Find("Text_N" + i + "_" + n + "_" + m).GetComponent<Text>();
                    Debug.Log("Text_N" + i + "_" + n + "_" + m);
                }
            }
        }
    }

    [ExecuteInEditMode]
    public void UpdateChart()
    {
        for (int i = 0; i < 4; i++)
        {
            if (conv.player_layer0.Count > i)
            {
                Debug.Log("Layer 0 -" + i);
                fc_player_layer0[i].text = conv.player_layer0[i];
                fc_npc_layer0[i].text = conv.npc_layer0[i];
            }
            else Debug.Log("Layer 0 -" + i + " does not exist atm.");
       
        }
        for(int i = 0; i < 16; i++)
        {
            Debug.Log("Layer 1 -" + i);
            fc_player_layer1[i].text = conv.player_layer1[i];
            fc_npc_layer1[i].text = conv.npc_layer1[i];
        }
        for (int i = 0; i < 64; i++)
        {
            Debug.Log("Layer 2 -" + i);
            fc_player_layer2[i].text = conv.player_layer2[i];
            fc_npc_layer2[i].text = conv.npc_layer2[i];
        }
    }

    [ExecuteInEditMode]
    public void ClearChart()
    {
        for (int i = 0; i < 4; i++)
        {
            if (conv.player_layer0.Count > i)
            {
                Debug.Log("Layer 0 -" + i);
                fc_player_layer0[i].text = "";
                fc_npc_layer0[i].text = "";
            }
            else Debug.Log("Layer 0 -" + i + " does not exist atm.");

        }
        for (int i = 0; i < 16; i++)
        {
            Debug.Log("Layer 1 -" + i);
            fc_player_layer1[i].text = "";
            fc_npc_layer1[i].text = "";
        }
        for (int i = 0; i < 64; i++)
        {
            Debug.Log("Layer 2 -" + i);
            fc_player_layer2[i].text = "";
            fc_npc_layer2[i].text = "";
        }
    }
}
