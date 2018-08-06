using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Conversation))]
[CanEditMultipleObjects]
public class ConversationEditor : Editor
{


    SerializedProperty npc_start_random_msg_prop;
    SerializedProperty npc_alt_start_random_msg_prop;
    SerializedProperty player_layer0_prop;
    SerializedProperty cp_prop;
    SerializedProperty cp_prop1;
    SerializedProperty cp_prop2;
    SerializedProperty cp_prop3;

    public bool toggle_default_inspector;
    public bool toggle_player_layer0;
    public bool cHelp;

    

    public bool[] path = new bool[10];
    public bool[] path0 = new bool[16];
    public bool[] foldout = new bool[64];
    public int[] custom_path = new int[4];


    public bool path_editor_toggle;



    //FlowChartSetup
    public bool flowchart_setup_toogle;
    public Text[] fc_test_player_layer0 = new Text[4];
    //SerializedProperty fc_p_l0;
    //SerializedProperty fc_n_l0;
    //SerializedProperty fc_p_l1;
    //SerializedProperty fc_n_l1;
    //SerializedProperty fc_p_l2;
    //SerializedProperty fc_n_l2;

    public bool test;
    public int testcount;
    public string test_string;



    public void OnEnable()
    {
        Conversation conv = (Conversation)target;
        //EditorStyles.textArea.wordWrap = true;

        npc_start_random_msg_prop = serializedObject.FindProperty("npc_start_random_msg");
        npc_alt_start_random_msg_prop = serializedObject.FindProperty("npc_alt_start_random_msg");
        player_layer0_prop = serializedObject.FindProperty("player_layer0");
        cp_prop = serializedObject.FindProperty("doubleList0");
        cp_prop1 = serializedObject.FindProperty("doubleList1");
        cp_prop2 = serializedObject.FindProperty("doubleList2");
        cp_prop3 = serializedObject.FindProperty("doubleList3");


        //fc_p_l0 = serializedObject.FindProperty("fc_player_layer0");
        //fc_n_l0 = serializedObject.FindProperty("fc_npc_layer0");
        //fc_p_l1 = serializedObject.FindProperty("fc_player_layer1");
        //fc_n_l1 = serializedObject.FindProperty("fc_npc_layer0");
        //fc_p_l2 = serializedObject.FindProperty("fc_player_layer2");
        //fc_n_l2 = serializedObject.FindProperty("fc_npc_layer2");


        //conv.player_layer0.Add("");


        //fc_test_player_layer0[0] = GameObject.Find("Text_P0").GetComponent<Text>();


    }

    public override void OnInspectorGUI()
    {

        Conversation conv = (Conversation)target;
        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.HelpBox("To do: Implement string split system and syntax for new message window, facial expression, variable change, etc.", MessageType.Info);
        EditorGUILayout.Space();

        //<---------------------->NPC Starter<----------------------> 
        EditorGUILayout.LabelField("(Optional) NPC Conversation Starter", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        conv.npc_starts = EditorGUILayout.Toggle("Does NPC start conversation?", conv.npc_starts);
        if (conv.npc_starts)
        {
            conv.npc_start_random = EditorGUILayout.Toggle("Randomize multiple msgs instead?", conv.npc_start_random);
            EditorGUILayout.EndHorizontal();

            if (conv.npc_start_random)
            {
                EditorGUILayout.PropertyField(npc_start_random_msg_prop, new GUIContent("Random Messages"), true);
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                conv.npc_start_msg1 = EditorGUILayout.DelayedTextField("NPC Message 1", conv.npc_start_msg1);
            }


            EditorGUILayout.BeginHorizontal();
            conv.npc_alt_start = EditorGUILayout.Toggle("Previous interaction affect msg?", conv.npc_alt_start);
            if (conv.npc_alt_start)
            {
                conv.npc_alt_start_random =
                    EditorGUILayout.Toggle("Randomize multiple msgs instead?", conv.npc_alt_start_random);
                EditorGUILayout.EndHorizontal();

                if (conv.npc_start_random)
                {
                    EditorGUILayout.HelpBox(
                        "It doesn't make much sense to have multiple possible \"first time only\" conversation starters if only one will ever be read.",
                        MessageType.Warning);
                }

                if (conv.npc_alt_start_random)
                {
                    EditorGUILayout.PropertyField(npc_alt_start_random_msg_prop, new GUIContent("Random Messages"),
                        true);
                    serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    conv.npc_start_msg2 = EditorGUILayout.DelayedTextField("NPC Message 2", conv.npc_start_msg2);
                }


                conv.npc_alt_start_var =
                    EditorGUILayout.Toggle("Variable affects NPC Message?", conv.npc_alt_start_var);
                if (conv.npc_alt_start_var)
                {
                    EditorGUILayout.BeginHorizontal();
                    conv.npc_alt_start_var_name =
                        EditorGUILayout.DelayedTextField("Name of the variable:", conv.npc_alt_start_var_name);
                    conv.npc_alt_start_var_condition =
                        EditorGUILayout.Toggle("Condition for message:", conv.npc_alt_start_var_condition);
                    EditorGUILayout.EndHorizontal();
                    conv.npc_start_msg_var =
                        EditorGUILayout.DelayedTextField("NPC Message Variable", conv.npc_start_msg_var);
                }
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        else EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Conversation By Path:", EditorStyles.boldLabel);

        toggle_player_layer0 = EditorGUILayout.Toggle("Show all path beginnings", toggle_player_layer0);
        if (toggle_player_layer0)
        {
            EditorGUILayout.PropertyField(player_layer0_prop, new GUIContent("Player Message 0"), true);
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();

        //<----------------------> Path 0 (Layer 0)<----------------------> 

        path[0] = EditorGUILayout.Foldout(path[0], "Path 0");
        if (path[0])
        {
            if (conv.player_layer0.Count < 1)
            {
                conv.player_layer0.Add("");
            }
            else
            {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player L0.0", GUILayout.MaxWidth(90));
                conv.player_layer0[0] = EditorGUILayout.TextArea(conv.player_layer0[0]);
                EditorGUILayout.EndHorizontal();
            }
            if (conv.npc_layer0.Count < 1)
            {
                conv.npc_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC L0.0", GUILayout.MaxWidth(90));
                conv.npc_layer0[0] = EditorGUILayout.TextArea(conv.npc_layer0[0], GUILayout.MinWidth(200));
                EditorGUILayout.EndHorizontal();
            }

            conv.player_msg_count[0] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[0]);

            //<----------------------> Path 0.0 (Layer 1) <---------------------->             
            if (conv.player_msg_count[0] >= 1)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[0] = EditorGUILayout.Foldout(path0[0], "Path 0.0");
                if (path0[0])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.0", GUILayout.MaxWidth(90));
                    conv.player_layer1[0] = EditorGUILayout.TextArea(conv.player_layer1[0]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.0", GUILayout.MaxWidth(90));
                    conv.npc_layer1[0] = EditorGUILayout.TextArea(conv.npc_layer1[0]);
                    EditorGUILayout.EndHorizontal();


                    conv.player_msg_count[1] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[1]);

                    //<----------------------> Path 0.0.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[1] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[0] = EditorGUILayout.TextArea(conv.player_layer2[0]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[0] = EditorGUILayout.TextArea(conv.npc_layer2[0]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.0.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[1] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[1] = EditorGUILayout.TextArea(conv.player_layer2[1]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[1] = EditorGUILayout.TextArea(conv.npc_layer2[1]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.0.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[1] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[2] = EditorGUILayout.TextArea(conv.player_layer2[2]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[2] = EditorGUILayout.TextArea(conv.npc_layer2[2]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.0.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[1] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[3] = EditorGUILayout.TextArea(conv.player_layer2[3]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[3] = EditorGUILayout.TextArea(conv.npc_layer2[3]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 0.1 (Layer 1) <----------------------> 
            if (conv.player_msg_count[0] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[1] = EditorGUILayout.Foldout(path0[1], "Path 0.1");
                if (path0[1])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.1", GUILayout.MaxWidth(90));
                    conv.player_layer1[1] = EditorGUILayout.TextArea(conv.player_layer1[1]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.1", GUILayout.MaxWidth(90));
                    conv.npc_layer1[1] = EditorGUILayout.TextArea(conv.npc_layer1[1]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[2] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[2]);

                    //<----------------------> Path 0.1.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[2] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[4] = EditorGUILayout.TextArea(conv.player_layer2[4]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[4] = EditorGUILayout.TextArea(conv.npc_layer2[4]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.1.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[2] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[5] = EditorGUILayout.TextArea(conv.player_layer2[5]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[5] = EditorGUILayout.TextArea(conv.npc_layer2[5]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.1.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[2] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[6] = EditorGUILayout.TextArea(conv.player_layer2[6]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[6] = EditorGUILayout.TextArea(conv.npc_layer2[6]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.1.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[2] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[7] = EditorGUILayout.TextArea(conv.player_layer2[7]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[7] = EditorGUILayout.TextArea(conv.npc_layer2[7]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 0.2 (Layer 1) <----------------------> 
            if (conv.player_msg_count[0] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[2] = EditorGUILayout.Foldout(path0[2], "Path 0.2");
                if (path0[2])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[2] = EditorGUILayout.TextArea(conv.player_layer1[2]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[2] = EditorGUILayout.TextArea(conv.npc_layer1[2]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[3] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[3]);

                    //<----------------------> Path 0.2.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[3] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[8] = EditorGUILayout.TextArea(conv.player_layer2[8]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[8] = EditorGUILayout.TextArea(conv.npc_layer2[8]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.2.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[3] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[9] = EditorGUILayout.TextArea(conv.player_layer2[9]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[9] = EditorGUILayout.TextArea(conv.npc_layer2[9]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.2.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[3] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[10] = EditorGUILayout.TextArea(conv.player_layer2[10]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[10] = EditorGUILayout.TextArea(conv.npc_layer2[10]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.2.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[3] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[11] = EditorGUILayout.TextArea(conv.player_layer2[11]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[11] = EditorGUILayout.TextArea(conv.npc_layer2[11]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 0.3 (Layer 1) <----------------------> 
            if (conv.player_msg_count[0] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[3] = EditorGUILayout.Foldout(path0[3], "Path 0.3");
                if (path0[3])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[3] = EditorGUILayout.TextArea(conv.player_layer1[3]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[3] = EditorGUILayout.TextArea(conv.npc_layer1[3]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[4] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[4]);

                    //<----------------------> Path 0.3.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[4] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[12] = EditorGUILayout.TextArea(conv.player_layer2[12]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[12] = EditorGUILayout.TextArea(conv.npc_layer2[12]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.3.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[4] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[13] = EditorGUILayout.TextArea(conv.player_layer2[13]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[13] = EditorGUILayout.TextArea(conv.npc_layer2[13]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.3.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[4] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[14] = EditorGUILayout.TextArea(conv.player_layer2[14]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[14] = EditorGUILayout.TextArea(conv.npc_layer2[14]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.3.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[4] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[15] = EditorGUILayout.TextArea(conv.player_layer2[15]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[15] = EditorGUILayout.TextArea(conv.npc_layer2[15]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        //<----------------------> Path 1 <----------------------> 

        path[1] = EditorGUILayout.Foldout(path[1], "Path 1");
        if (path[1])
        {
            if (conv.player_layer0.Count < 2)
            {
                conv.player_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player L0.1", GUILayout.MaxWidth(90));
                conv.player_layer0[1] = EditorGUILayout.TextArea(conv.player_layer0[1]);
                EditorGUILayout.EndHorizontal();
            }
            if (conv.npc_layer0.Count < 2)
            {
                conv.npc_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC L0.0", GUILayout.MaxWidth(90));
                conv.npc_layer0[1] = EditorGUILayout.TextArea(conv.npc_layer0[1]);
                EditorGUILayout.EndHorizontal();
            }

            conv.player_msg_count[5] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[5]);

            //<----------------------> Path 1.0 (Layer 1) <---------------------->   
            if (conv.player_msg_count[5] >= 1)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[4] = EditorGUILayout.Foldout(path0[4], "Path 1.0");
                if (path0[4])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.0", GUILayout.MaxWidth(90));
                    conv.player_layer1[4] = EditorGUILayout.TextArea(conv.player_layer1[4]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.0", GUILayout.MaxWidth(90));
                    conv.npc_layer1[4] = EditorGUILayout.TextArea(conv.npc_layer1[4]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[6] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[6]);

                    //<----------------------> Path 1.0.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[6] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[16] = EditorGUILayout.TextArea(conv.player_layer2[16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[16] = EditorGUILayout.TextArea(conv.npc_layer2[16]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.0.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[6] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[17] = EditorGUILayout.TextArea(conv.player_layer2[17]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[17] = EditorGUILayout.TextArea(conv.npc_layer2[17]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.0.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[6] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[18] = EditorGUILayout.TextArea(conv.player_layer2[18]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[18] = EditorGUILayout.TextArea(conv.npc_layer2[18]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.0.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[6] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[19] = EditorGUILayout.TextArea(conv.player_layer2[19]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[19] = EditorGUILayout.TextArea(conv.npc_layer2[19]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 1.1 (Layer 1) <----------------------> 
            if (conv.player_msg_count[5] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[5] = EditorGUILayout.Foldout(path0[5], "Path 1.1");
                if (path0[5])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.1", GUILayout.MaxWidth(90));
                    conv.player_layer1[5] = EditorGUILayout.TextArea(conv.player_layer1[5]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.1", GUILayout.MaxWidth(90));
                    conv.npc_layer1[5] = EditorGUILayout.TextArea(conv.npc_layer1[5]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[7] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[7]);

                    //<----------------------> Path 1.1.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[7] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[20] = EditorGUILayout.TextArea(conv.player_layer2[20]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[20] = EditorGUILayout.TextArea(conv.npc_layer2[20]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.1.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[7] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[21] = EditorGUILayout.TextArea(conv.player_layer2[21]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[21] = EditorGUILayout.TextArea(conv.npc_layer2[21]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.1.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[7] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[22] = EditorGUILayout.TextArea(conv.player_layer2[22]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[22] = EditorGUILayout.TextArea(conv.npc_layer2[22]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.1.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[7] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[23] = EditorGUILayout.TextArea(conv.player_layer2[23]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[23] = EditorGUILayout.TextArea(conv.npc_layer2[23]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 1.2 (Layer 1) <----------------------> 
            if (conv.player_msg_count[5] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[6] = EditorGUILayout.Foldout(path0[6], "Path 1.2");
                if (path0[6])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[6] = EditorGUILayout.TextArea(conv.player_layer1[6]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[6] = EditorGUILayout.TextArea(conv.npc_layer1[6]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[8] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[8]);

                    //<----------------------> Path 1.2.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[8] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[24] = EditorGUILayout.TextArea(conv.player_layer2[24]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[24] = EditorGUILayout.TextArea(conv.npc_layer2[24]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.2.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[8] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[25] = EditorGUILayout.TextArea(conv.player_layer2[25]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[25] = EditorGUILayout.TextArea(conv.npc_layer2[25]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.2.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[8] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[26] = EditorGUILayout.TextArea(conv.player_layer2[26]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[26] = EditorGUILayout.TextArea(conv.npc_layer2[26]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.2.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[8] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[27] = EditorGUILayout.TextArea(conv.player_layer2[27]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[27] = EditorGUILayout.TextArea(conv.npc_layer2[27]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 1.3 (Layer 1) <----------------------> 
            if (conv.player_msg_count[5] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[7] = EditorGUILayout.Foldout(path0[7], "Path 1.3");
                if (path0[7])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[7] = EditorGUILayout.TextArea(conv.player_layer1[7]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[7] = EditorGUILayout.TextArea(conv.npc_layer1[7]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[9] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[9]);

                    //<----------------------> Path 1.3.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[9] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[28] = EditorGUILayout.TextArea(conv.player_layer2[28]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[28] = EditorGUILayout.TextArea(conv.npc_layer2[28]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.3.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[9] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[29] = EditorGUILayout.TextArea(conv.player_layer2[29]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[29] = EditorGUILayout.TextArea(conv.npc_layer2[29]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.3.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[9] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[30] = EditorGUILayout.TextArea(conv.player_layer2[30]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[30] = EditorGUILayout.TextArea(conv.npc_layer2[30]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.3.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[9] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[31] = EditorGUILayout.TextArea(conv.player_layer2[31]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[31] = EditorGUILayout.TextArea(conv.npc_layer2[31]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        //<----------------------> Path 2 <----------------------> 


        path[2] = EditorGUILayout.Foldout(path[2], "Path 2");
        if (path[2])
        {
            if (conv.player_layer0.Count < 3)
            {
                conv.player_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player L0.2", GUILayout.MaxWidth(90));
                conv.player_layer0[2] = EditorGUILayout.TextArea(conv.player_layer0[2]);
                EditorGUILayout.EndHorizontal();
            }
            if (conv.npc_layer0.Count < 3)
            {
                conv.npc_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC L0.2", GUILayout.MaxWidth(90));
                conv.npc_layer0[2] = EditorGUILayout.TextArea(conv.npc_layer0[2]);
                EditorGUILayout.EndHorizontal();
            }

            conv.player_msg_count[10] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[10]);

            //<----------------------> Path 2.0 (Layer 1) <---------------------->   
            if (conv.player_msg_count[10] >= 1)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[8] = EditorGUILayout.Foldout(path0[8], "Path 2.0");
                if (path0[8])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.0", GUILayout.MaxWidth(90));
                    conv.player_layer1[8] = EditorGUILayout.TextArea(conv.player_layer1[8]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.0", GUILayout.MaxWidth(90));
                    conv.npc_layer1[8] = EditorGUILayout.TextArea(conv.npc_layer1[8]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[11] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[11]);

                    //<----------------------> Path 2.0.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[11] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[32] = EditorGUILayout.TextArea(conv.player_layer2[32]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[32] = EditorGUILayout.TextArea(conv.npc_layer2[32]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.0.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[11] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[33] = EditorGUILayout.TextArea(conv.player_layer2[33]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[33] = EditorGUILayout.TextArea(conv.npc_layer2[33]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.0.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[11] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[34] = EditorGUILayout.TextArea(conv.player_layer2[34]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[34] = EditorGUILayout.TextArea(conv.npc_layer2[34]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.0.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[11] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[35] = EditorGUILayout.TextArea(conv.player_layer2[35]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[35] = EditorGUILayout.TextArea(conv.npc_layer2[35]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 2.1 (Layer 1) <----------------------> 
            if (conv.player_msg_count[10] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[9] = EditorGUILayout.Foldout(path0[9], "Path 2.1");
                if (path0[9])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.1", GUILayout.MaxWidth(90));
                    conv.player_layer1[9] = EditorGUILayout.TextArea(conv.player_layer1[9]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.1", GUILayout.MaxWidth(90));
                    conv.npc_layer1[9] = EditorGUILayout.TextArea(conv.npc_layer1[9]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[12] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[12]);

                    //<----------------------> Path 2.1.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[12] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[36] = EditorGUILayout.TextArea(conv.player_layer2[36]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[36] = EditorGUILayout.TextArea(conv.npc_layer2[36]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.1.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[12] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[37] = EditorGUILayout.TextArea(conv.player_layer2[37]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[37] = EditorGUILayout.TextArea(conv.npc_layer2[37]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.1.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[12] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[38] = EditorGUILayout.TextArea(conv.player_layer2[38]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[38] = EditorGUILayout.TextArea(conv.npc_layer2[38]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.1.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[12] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[39] = EditorGUILayout.TextArea(conv.player_layer2[39]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[39] = EditorGUILayout.TextArea(conv.npc_layer2[39]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 2.2 (Layer 1) <---------------------->
            if (conv.player_msg_count[10] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[10] = EditorGUILayout.Foldout(path0[10], "Path 2.2");
                if (path0[10])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[10] = EditorGUILayout.TextArea(conv.player_layer1[10]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[10] = EditorGUILayout.TextArea(conv.npc_layer1[10]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[13] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[13]);

                    //<----------------------> Path 2.2.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[13] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[40] = EditorGUILayout.TextArea(conv.player_layer2[40]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[40] = EditorGUILayout.TextArea(conv.npc_layer2[40]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.2.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[13] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[41] = EditorGUILayout.TextArea(conv.player_layer2[41]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[41] = EditorGUILayout.TextArea(conv.npc_layer2[41]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.2.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[13] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[42] = EditorGUILayout.TextArea(conv.player_layer2[42]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[42] = EditorGUILayout.TextArea(conv.npc_layer2[42]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.2.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[13] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[43] = EditorGUILayout.TextArea(conv.player_layer2[43]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[43] = EditorGUILayout.TextArea(conv.npc_layer2[43]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 2.3 (Layer 1) <---------------------->
            //below
            if (conv.player_msg_count[10] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[11] = EditorGUILayout.Foldout(path0[11], "Path 2.3");
                if (path0[11])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[11] = EditorGUILayout.TextArea(conv.player_layer1[11]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[11] = EditorGUILayout.TextArea(conv.npc_layer1[11]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[14] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[14]);

                    //<----------------------> Path 2.3.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[14] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[44] = EditorGUILayout.TextArea(conv.player_layer2[44]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[44] = EditorGUILayout.TextArea(conv.npc_layer2[44]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.3.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[14] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[45] = EditorGUILayout.TextArea(conv.player_layer2[45]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[45] = EditorGUILayout.TextArea(conv.npc_layer2[45]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.3.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[14] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[46] = EditorGUILayout.TextArea(conv.player_layer2[46]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[46] = EditorGUILayout.TextArea(conv.npc_layer2[46]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.3.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[14] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[47] = EditorGUILayout.TextArea(conv.player_layer2[47]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[47] = EditorGUILayout.TextArea(conv.npc_layer2[47]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        //<----------------------> Path 3 <----------------------> 


        path[3] = EditorGUILayout.Foldout(path[3], "Path 3");
        if (path[3])
        {
            if (conv.player_layer0.Count < 4)
            {
                conv.player_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player L0.3", GUILayout.MaxWidth(90));
                conv.player_layer0[3] = EditorGUILayout.TextArea(conv.player_layer0[3]);
                EditorGUILayout.EndHorizontal();
            }
            if (conv.npc_layer0.Count < 4)
            {
                conv.npc_layer0.Add("");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC L0.3", GUILayout.MaxWidth(90));
                conv.npc_layer0[3] = EditorGUILayout.TextArea(conv.npc_layer0[3]);
                EditorGUILayout.EndHorizontal();
            }

            conv.player_msg_count[15] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[15]);

            //<----------------------> Path 3.0 (Layer 1) <---------------------->   
            if (conv.player_msg_count[15] >= 1)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[12] = EditorGUILayout.Foldout(path0[12], "Path 3.0");
                if (path0[12])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.0", GUILayout.MaxWidth(90));
                    conv.player_layer1[8 + 4] = EditorGUILayout.TextArea(conv.player_layer1[8 + 4]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.0", GUILayout.MaxWidth(90));
                    conv.npc_layer1[8 + 4] = EditorGUILayout.TextArea(conv.npc_layer1[8 + 4]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[16] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[16]);

                    //<----------------------> Path 3.0.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[16] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.0.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[32 + 16] = EditorGUILayout.TextArea(conv.player_layer2[32 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[32 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[32 + 16]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.0.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[16] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.0.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[33 + 16] = EditorGUILayout.TextArea(conv.player_layer2[33 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[33 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[33 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.0.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[16] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.0.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[34 + 16] = EditorGUILayout.TextArea(conv.player_layer2[34 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[34 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[34 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.0.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[16] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.0.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[35 + 16] = EditorGUILayout.TextArea(conv.player_layer2[35 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[35 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[35 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 3.1 (Layer 1) <----------------------> 
            if (conv.player_msg_count[15] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[13] = EditorGUILayout.Foldout(path0[13], "Path 3.1");
                if (path0[13])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.1", GUILayout.MaxWidth(90));
                    conv.player_layer1[9 + 4] = EditorGUILayout.TextArea(conv.player_layer1[9 + 4]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.1", GUILayout.MaxWidth(90));
                    conv.npc_layer1[9 + 4] = EditorGUILayout.TextArea(conv.npc_layer1[9 + 4]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[17] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[17]);

                    //<----------------------> Path 3.1.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[17] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.1.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[36 + 16] = EditorGUILayout.TextArea(conv.player_layer2[36 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[36 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[36 + 16]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.1.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[17] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.1.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[37 + 16] = EditorGUILayout.TextArea(conv.player_layer2[37 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[37 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[37 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.1.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[17] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.1.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[38 + 16] = EditorGUILayout.TextArea(conv.player_layer2[38 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[38 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[38 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.1.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[17] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.1.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[39 + 16] = EditorGUILayout.TextArea(conv.player_layer2[39 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[39 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[39 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 3.2 (Layer 1) <---------------------->
            if (conv.player_msg_count[15] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[14] = EditorGUILayout.Foldout(path0[14], "Path 3.2");
                if (path0[14])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[10 + 4] = EditorGUILayout.TextArea(conv.player_layer1[10 + 4]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[10 + 4] = EditorGUILayout.TextArea(conv.npc_layer1[10 + 4]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[18] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[18]);

                    //<----------------------> Path 3.2.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[18] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.2.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[40 + 16] = EditorGUILayout.TextArea(conv.player_layer2[40 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[40 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[40 + 16]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.2.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[18] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.2.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[41 + 16] = EditorGUILayout.TextArea(conv.player_layer2[41 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[41 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[41 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.2.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[18] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.2.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[42 + 16] = EditorGUILayout.TextArea(conv.player_layer2[42 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[42 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[42 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.2.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[18] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.2.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[43 + 16] = EditorGUILayout.TextArea(conv.player_layer2[43 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[43 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[43 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 3.3 (Layer 1) <---------------------->
            //below
            if (conv.player_msg_count[15] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[15] = EditorGUILayout.Foldout(path0[15], "Path 3.3");
                if (path0[15])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Player L1.2", GUILayout.MaxWidth(90));
                    conv.player_layer1[11 + 4] = EditorGUILayout.TextArea(conv.player_layer1[11 + 4]);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("NPC L1.2", GUILayout.MaxWidth(90));
                    conv.npc_layer1[11 + 4] = EditorGUILayout.TextArea(conv.npc_layer1[11 + 4]);
                    EditorGUILayout.EndHorizontal();

                    conv.player_msg_count[19] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", conv.player_msg_count[19]);

                    //<----------------------> Path 3.3.0 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[19] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.3.0");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.0", GUILayout.MaxWidth(90));
                        conv.player_layer2[44 + 16] = EditorGUILayout.TextArea(conv.player_layer2[44 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.0", GUILayout.MaxWidth(90));
                        conv.npc_layer2[44 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[44 + 16]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.3.1 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[19] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.3.1");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.1", GUILayout.MaxWidth(90));
                        conv.player_layer2[45 + 16] = EditorGUILayout.TextArea(conv.player_layer2[45 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.1", GUILayout.MaxWidth(90));
                        conv.npc_layer2[45 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[45 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.3.2 (Layer 2) <----------------------> 
                    if (conv.player_msg_count[19] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.3.2");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.2", GUILayout.MaxWidth(90));
                        conv.player_layer2[46 + 16] = EditorGUILayout.TextArea(conv.player_layer2[46 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.2", GUILayout.MaxWidth(90));
                        conv.npc_layer2[46 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[46 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 3.3.3 (Layer 3) <----------------------> 
                    if (conv.player_msg_count[19] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 3.3.3");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Player L2.3", GUILayout.MaxWidth(90));
                        conv.player_layer2[47 + 16] = EditorGUILayout.TextArea(conv.player_layer2[47 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("NPC L2.3", GUILayout.MaxWidth(90));
                        conv.npc_layer2[47 + 16] = EditorGUILayout.TextArea(conv.npc_layer2[47 + 16]);
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        //-------------------------------------------------------------------------//
        //-------------------------------------------------------------------------//
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Create a custom path!", EditorStyles.boldLabel);
        if (GUILayout.Button("With this button!"))
        {
            if(conv.cp_count < 4) conv.cp_count += 1;
            else EditorGUILayout.HelpBox("You can't add more than 4 custom paths.", MessageType.Info);
            while (conv.player_layer0.Count < 4 + conv.cp_count)
            {
                conv.player_layer0.Add("");
            }
            while (conv.npc_layer0.Count < 4 + conv.cp_count)
            {
                conv.npc_layer0.Add("");
            }
            while (conv.cp_struct.Count < conv.cp_count)
            {
                conv.cp_struct.Add("");
            }


        }
        EditorGUILayout.EndHorizontal();

        
        EditorGUILayout.Space();
        if (conv.cp_count > 0)
        {
            path[4] = EditorGUILayout.Foldout(path[4], "Custom Path 0");

            if (path[4])
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player Message", GUILayout.MaxWidth(120));
                conv.player_layer0[4] = EditorGUILayout.TextArea(conv.player_layer0[4]);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC Message", GUILayout.MaxWidth(120));
                conv.npc_layer0[4] = EditorGUILayout.TextArea(conv.npc_layer0[4]);
                EditorGUILayout.EndHorizontal();



                //conv.cp_struct[0] = EditorGUILayout.TextField("Structure:", conv.cp_struct[0]);

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(cp_prop, new GUIContent("List of all text-pairs belonging to this path:"), true);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            if (conv.cp_count > 1) path[5] = EditorGUILayout.Foldout(path[5], "Custom Path 1");
            if (path[5])
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player Message", GUILayout.MaxWidth(120));
                conv.player_layer0[5] = EditorGUILayout.TextArea(conv.player_layer0[5]);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC Message", GUILayout.MaxWidth(120));
                conv.npc_layer0[5] = EditorGUILayout.TextArea(conv.npc_layer0[5]);
                EditorGUILayout.EndHorizontal();
                //conv.cp_struct[1] = EditorGUILayout.TextField("Structure:", conv.cp_struct[1]);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(cp_prop1, new GUIContent("Seperate player & npc text by ♥"), true);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            if (conv.cp_count > 2) path[6] = EditorGUILayout.Foldout(path[6], "Custom Path 2");
            if (path[6])
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player Message", GUILayout.MaxWidth(120));
                conv.player_layer0[6] = EditorGUILayout.TextArea(conv.player_layer0[6]);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC Message", GUILayout.MaxWidth(120));
                conv.npc_layer0[6] = EditorGUILayout.TextArea(conv.npc_layer0[6]);
                EditorGUILayout.EndHorizontal();
                //conv.cp_struct[2] = EditorGUILayout.TextField("Structure:", conv.cp_struct[2]);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(cp_prop2, new GUIContent("Seperate player & npc text by ♥"), true);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            if (conv.cp_count > 3) path[7] = EditorGUILayout.Foldout(path[4], "Custom Path 3");
            if (path[7])
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Player Message", GUILayout.MaxWidth(120));
                conv.player_layer0[7] = EditorGUILayout.TextArea(conv.player_layer0[7]);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("NPC Message", GUILayout.MaxWidth(120));
                conv.npc_layer0[7] = EditorGUILayout.TextArea(conv.npc_layer0[7]);
                EditorGUILayout.EndHorizontal();
                //conv.cp_struct[3] = EditorGUILayout.TextField("Structure:", conv.cp_struct[3]);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(cp_prop3, new GUIContent("Seperate player & npc text by ♥"), true);
                EditorGUI.indentLevel--;
            }
        }

        cHelp = EditorGUILayout.Foldout(cHelp, "Show cStruct help.");
        if (cHelp)
        {
            EditorGUILayout.LabelField("Just like the fixed 4-4-4 structure is labeled in the \"non-custom\" part. So the index of the path, of the subpath, subsubpath, etc. seperated by a period.\n\n" +
                            "An example:\n\n\n" +
                            "P: What is your favourite band?\n" +
                            "N: Nickelback. Yours?\n" +
                            "Answer Options:\n" +
                            "	○ P: Rammstein.\n" +
                            "	• N: Nice. Good old germany, am I right?\n" +
                            "		○ P: Yeah. Sad we lost WWII.\n" +
                            "		• N: Dafuq is wrong with you?\n" +
                            "		○ P: Well, I just like the band. Fuck germany though.\n" +
                            "		• N: Yeah, those germans. I don't like them either.\n" +
                            "		○ P: Mother Russia > Germany.\n" +
                            "		• N: Cyka blyat!\n" +
                            "			\n" +
                            "			○ P: Rush B!\n" +
                            "			• N: Okay.\n" +
                            "			○ P: Don't rush B though.\n" +
                            "			• N: What is wrong with you?\n" +
                            "	○ P: Canibal Corpse.\n" +
                            "	• N: Brütal \\m/\n" +
                            "		○ P: You know what else is brütal?\n" +
                            "		• N: Your dick?\n" +
                            "		○ P: I think people who use expressions like \"brütal\" & \"trve\" should be executed.\n" +
                            "		• N: ...\n" +
                            "	○ P: Slayer.\n" +
                            "	• N: Meh. Overrated.\n" +
                            "	○ P: Iron Maiden.\n" +
                            "	• N: Love it. What is your favourite song?\n" +
                            "		○ P: Afraid to shoot strangers.\n" +
                            "		• N: I want to shoot my cum in your face.\n" +
                            "		○ P: Fear of the dark.\n" +
                            "		• N: *sings* FEEEAAAAR OF THEEEEE DAAAAAAAARK!\n" +
                            "		○ P: Hallowed be thy name.\n" +
                            "		• N: What a deep and powerful song.\n" +
                            "		○ P: Aces High.\n" +
                            "		• N: Neat.\n" +
                            "		○ P: 2 Minutes to midnight.\n" +
                            "		• N: YAS! Powerslave was my fav album!\n" +
                            "		\n" +
                            "	○ P: Skrillex.\n" +
                            "	• N: That is no band. Now get out.\n" +
                            "	○ P: My dick. Heard they have a gig tonight.\n" +
                            "	• N: Never heard of them. In town?\n" +
                            "		○ P: No, in your ass. With your allowance.\n" +
                            "		• N: He he, sure.\n" +
                            "		\n" +
                            "		○ P: I actually made a stupid joke and I deeply apologize.\n" +
                            "		• N: Ahaha, np.<page>Just don't do it again.\n" +
                            "	○ P: Some Band\n" +
                            "	• N: Some Comment\n\n\n" +
                            "The size of list is the number of total PN-pairs, so 21\n" +
                            "The Rammstein-pair's cStruct is \"0\"\n" +
                            "The Iron Maiden's is \"3\", since it is the 4th pair on the first layer.\n" +
                            "The suboption 2 Minutes to Midnight has the cStruct \"3.4\", because it's the 5th option in the Iron Maiden-path.\n" +
                            "Rush B, located on the third layer: \"0.2.0\"\n\n\n" +
                            "There is no need to follow a specific order when filling in the pairs, but doing so at least at the beginning will give you a better feeling for the cStruct. I suggest you layout your conversation like the one above and start from the top. If I did that, I could simply fill in my cStructs like this:\n\n" +
                            "0 - 0.0 - 0.1 - 0.2 - 0.2.0 - 0.2.1 - 1 - 1.0 - 1.1 - 2 - 3 - 3.0 - 3.1 - 3.2 - 3.3 - 4 - 5 - 5.0 - 5.1 - 6");
        }

        EditorGUILayout.Space();

        toggle_default_inspector = EditorGUILayout.Toggle("Show default inspector?", toggle_default_inspector);
        if (toggle_default_inspector)
        {
            base.OnInspectorGUI();
        }

        serializedObject.ApplyModifiedProperties();
    }
}