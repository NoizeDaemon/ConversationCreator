using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conversation : MonoBehaviour {

    //---SETUP/EDITOR---

    //starting
    public bool prev_Interaction;
    public bool npc_starts;
    public string npc_start_msg1;
    public bool npc_start_random;
    [TextArea(0, 3)]
    public List<string> npc_start_random_msg;
    public bool npc_alt_start;
    public string npc_start_msg2;
    public bool npc_alt_start_random;
    [TextArea(0, 3)]
    public List<string> npc_alt_start_random_msg;
    public bool npc_alt_start_var;
    public string npc_start_msg_var;
    public string npc_alt_start_var_name;
    public bool npc_alt_start_var_condition;

    //pathwise
    public int path_count;
    public int[] player_msg_count = new int[25];
    public List<string> player_layer0;
    public string[] player_layer1 = new string[16];
    public string[] player_layer2 = new string[64];
    public List<string> npc_layer0;
    public string[] npc_layer1 = new string[16];
    public string[] npc_layer2 = new string[64];

    //custom paths
    [TextArea(0, 3)]
    public List<string> cp_struct;
    public int cp_count;
    [System.Serializable]
    public class stringDouble
    {
        public string cStruct;
        [TextArea(1,3)]
        public string playerText;
        [TextArea(1, 3)]
        public string npcText;
    }
    public List<stringDouble> doubleList0, doubleList1, doubleList2, doubleList3;

    //test
    public string testfield;

    //---PROCESSING/INGAME---
    private Animator anim_npc;
    private Animator anim_player;
    private GameObject msg_win_npc;
    private TextMeshProUGUI npc_text;
    private MsgButtonControll npcMBC;
    public Transform containerTransform;
    //private TextMeshProUGUI player_text;
    private GameObject msg_win_player;
    private PlayerSelectionButtonBehaviour playerSBB;
    private static bool conversation_active;

    //player window
    public GameObject btnPrefab;
    private List<GameObject> btnGo;

    //path and layer handling
    private int layer;

    private int path;
    private int sPath;
    private int ssPath;


    // Use this for initialization
    void Start () {
        msg_win_npc = GameObject.Find("Message Window NPC");
        npc_text = GameObject.Find("TextMeshPro_NPC").GetComponent<TextMeshProUGUI>();
        npcMBC = msg_win_npc.GetComponent<MsgButtonControll>();
        msg_win_player = GameObject.Find("Message Window Player");
        anim_npc = msg_win_npc.GetComponent<Animator>();
        anim_player = msg_win_player.GetComponent<Animator>();
        playerSBB = msg_win_player.GetComponentInChildren<PlayerSelectionButtonBehaviour>();
        btnPrefab = Resources.Load<GameObject>("Prefabs/ConversationSystem/Choice_0");
        containerTransform = GameObject.Find("ButtonContainer_Adjustable").GetComponent<Transform>();
        //instantiate list
        btnGo = new List<GameObject>();
        if (player_layer0.Count > 4) CustomPathInitialization();
    }

    // Update is called once per frame
    void Update () {
		
	}

    //private void OnMouseUpAsButton()
    //{
    //    if (!conversation_active) StartConversation();
    //}

    public void StartConversation()
    {
        if (!conversation_active)
        {
            conversation_active = true;
            

            if (npc_starts)
            {
                layer = -1;
                if (!npc_start_random)
                {
                    npc_text.text = npc_start_msg1;
                }
                else
                {
                    npc_text.text = npc_start_random_msg[Random.Range(0, npc_start_random_msg.Count)];
                }
                npcMBC.Initialize();
                StartCoroutine(WaitForNpcStop());
                anim_npc.SetBool("IsOpen", true);
            }
            else
            {
                layer = 0;
                PlayerWindowInitialization();
            }
        }
        else Debug.Log("There is an active conversation!");

        
    }

    public void PlayerWindowInitialization()
    {
        if (layer == 0)
            for (int i = 0; i < player_layer0.Count; i++)
            {
                if (player_layer0[i] == "") continue;
                GameObject clone = Instantiate(btnPrefab, containerTransform);
                clone.name = "Choice " + i;
                clone.GetComponent<TextMeshProUGUI>().text = "<sprite=\"6\" index=0> " + player_layer0[i];
                btnGo.Add(clone);
            }
        else if (layer == 1)
        {
            for (int i = 4 * path; i < 4 * path + 4; i++)
            {
                if (player_layer1[i] == "") continue;
                GameObject clone = Instantiate(btnPrefab, containerTransform);
                clone.name = "Choice " + i;
                clone.GetComponent<TextMeshProUGUI>().text = "<sprite=\"6\" index=0> " + player_layer1[i];
                btnGo.Add(clone);
            }
        }
        else if (layer == 2)
        {
            for (int i = 16 * path + 4 * sPath; i < 16 * path + 4 * sPath + 4; i++)
            {
                if (player_layer2[i] == "") continue;
                GameObject clone = Instantiate(btnPrefab, containerTransform);
                clone.name = "Choice " + i;
                clone.GetComponent<TextMeshProUGUI>().text = "<sprite=\"6\" index=0> " + player_layer2[i];
                btnGo.Add(clone);
            }
        }
        playerSBB.Initialize();
        anim_player.SetBool("IsOpen", true);
        StartCoroutine(WaitForPlayerChoice());
    }

    public void NpcWindowInitialization(int choice)
    {
        switch (layer)
        {
            case 0:
                npc_text.text = npc_layer0[choice];
                break;
            case 1:
                npc_text.text = npc_layer1[choice];
                break;
            case 2:
                npc_text.text = npc_layer2[choice];
                break;
        }
        npcMBC.Initialize();
        anim_npc.SetBool("IsOpen", true);
        StartCoroutine(WaitForNpcStop());
    }

    public void NpcToPlayerTransition()
    {
        if (path < 4)
        {
            int checkSum = 0;
            if (layer == 0)
            {
                for (int i = 4 * path; i < 4 * path + 4; i++)
                {
                    if (player_layer1[i] == "") continue;
                    else checkSum += 1;
                }

            }
            else if (layer == 1)
            {
                for (int i = 4 * sPath; i < 4 * sPath + 4; i++)
                {
                    Debug.Log(4 * sPath + 4);
                    if (player_layer2[i] == "") continue;
                    else checkSum += 1;
                }
            }
            else if (layer == 2) EndConversation();
            Debug.Log("CheckSum = " + checkSum);
            if (checkSum != 0 || layer == -1)
            {
                layer += 1;
                PlayerWindowInitialization();
            }
            else EndConversation();
        }
        else //CUSTOM PATH HANDLING
        {
            switch (path)
            {
                case 4:
                    if (doubleList0.Count == 0) EndConversation();
                    else CustomPathPlayerWindowInitialization(doubleList0);
                    break;
                case 5:
                    if (doubleList1.Count == 0) EndConversation();
                    else CustomPathPlayerWindowInitialization(doubleList1);
                    break;
                case 6:
                    if (doubleList2.Count == 0) EndConversation();
                    else CustomPathPlayerWindowInitialization(doubleList2);
                    break;
                case 7:
                    if (doubleList3.Count == 0) EndConversation();
                    else CustomPathPlayerWindowInitialization(doubleList3);
                    break;
            }
        }
    }

    public void EndConversation()
    {
        prev_Interaction = true;
        conversation_active = false;
        Debug.Log("Conversation ended.");
    }

    IEnumerator WaitForPlayerChoice()
    {
        Debug.Log("Started waiting for Player Choice");
        yield return new WaitUntil(() => playerSBB.btnIsClicked == true);
        Debug.Log("Player made their choice!");
        NpcWindowInitialization(playerSBB.choice);
        switch (layer)
        {
            case 0: path = playerSBB.choice;
                break;
            case 1: sPath = playerSBB.choice;
                break;
            case 2: ssPath = playerSBB.choice;
                break;
        }
        playerSBB.btnIsClicked = false;
        anim_player.SetBool("IsOpen", false);
        foreach(GameObject clone in btnGo) Destroy(clone);
        StopCoroutine(WaitForPlayerChoice());
    }

    IEnumerator WaitForNpcStop()
    {
        Debug.Log("Started waiting for player to end NPC part...");
        yield return new WaitUntil(() => npcMBC.endIsClicked == true);
        Debug.Log("NPC part ended!");
        anim_npc.SetBool("IsOpen", false);
        npcMBC.endIsClicked = false;
        NpcToPlayerTransition();
        StopCoroutine(WaitForNpcStop());
    }

    public void CustomPathInitialization()
    {
        List<string> numStruct = new List<string>();
        numStruct.AddRange(cp_struct[0].Split('>', '<', 'd', 't', 'q', 'p'));
        //Debug
        foreach(string s in numStruct) Debug.Log(s);
        //
        List<char> opStruct = new List<char>();
        string str = Regex.Replace(cp_struct[0], @"[\d-]", string.Empty);
        foreach (char c in str) opStruct.Add(c);
        //Debug
        Debug.Log(str);
        foreach (char c in opStruct) Debug.Log(c);
        //
        Debug.Log("Index = " + doubleList0.FindIndex(x => x.cStruct == "0.1"));
    }

    public void CustomPathPlayerWindowInitialization(List<stringDouble> l)
    {
        Debug.Log("This list contains " + l.Count + " elements.");
        EndConversation();
    }
}
