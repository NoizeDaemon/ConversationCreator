using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Conversation))]
[CanEditMultipleObjects]
public class ConversationEditor : Editor {


    SerializedProperty npc_start_random_msg_prop;
    SerializedProperty npc_alt_start_random_msg_prop;
    SerializedProperty player_layer0_prop;
    SerializedProperty cp_prop;

    public bool toggle_default_inspector;
    public bool toggle_player_layer0;

    public int[] player_msg_count = new int[20];

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
        cp_prop = serializedObject.FindProperty("cp");


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


                conv.npc_alt_start_random = EditorGUILayout.Toggle("Randomize multiple msgs instead?", conv.npc_alt_start_random);
                EditorGUILayout.EndHorizontal();

                if (conv.npc_start_random)
                {
                    EditorGUILayout.HelpBox("It doesn't make much sense to have multiple possible \"first time only\" conversation starters if only one will ever be read.", MessageType.Warning);

                }

                if (conv.npc_alt_start_random)
                {


                    EditorGUILayout.PropertyField(npc_alt_start_random_msg_prop, new GUIContent("Random Messages"), true);
                    serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    conv.npc_start_msg2 = EditorGUILayout.DelayedTextField("NPC Message 2", conv.npc_start_msg2);
                }


                conv.npc_alt_start_var = EditorGUILayout.Toggle("Variable affects NPC Message?", conv.npc_alt_start_var);
                if (conv.npc_alt_start_var)
                {
                    EditorGUILayout.BeginHorizontal();
                    conv.npc_alt_start_var_name = EditorGUILayout.DelayedTextField("Name of the variable:", conv.npc_alt_start_var_name);
                    conv.npc_alt_start_var_condition = EditorGUILayout.Toggle("Condition for message:", conv.npc_alt_start_var_condition);
                    EditorGUILayout.EndHorizontal();
                    conv.npc_start_msg_var = EditorGUILayout.DelayedTextField("NPC Message Variable", conv.npc_start_msg_var);
                }
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }

        }
        else
        {
            EditorGUILayout.EndHorizontal();
        }

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

            player_msg_count[0] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[0]);

            //<----------------------> Path 0.0 (Layer 1) <---------------------->             
            if (player_msg_count[0] >= 1)
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


                    player_msg_count[1] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[1]);

                    //<----------------------> Path 0.0.0 (Layer 2) <----------------------> 
                    if (player_msg_count[1] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.0");

                        conv.player_layer2[0] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[0]);
                        conv.npc_layer2[0] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[0]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.0.1 (Layer 2) <----------------------> 
                    if (player_msg_count[1] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.1");
                        conv.player_layer2[1] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[1]);
                        conv.npc_layer2[1] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[1]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.0.2 (Layer 2) <----------------------> 
                    if (player_msg_count[1] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.2");
                        conv.player_layer2[2] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[2]);
                        conv.npc_layer2[2] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[2]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.0.3 (Layer 3) <----------------------> 
                    if (player_msg_count[1] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.0.3");
                        conv.player_layer2[3] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[3]);
                        conv.npc_layer2[3] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[3]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 0.1 (Layer 1) <----------------------> 
            if (player_msg_count[0] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[1] = EditorGUILayout.Foldout(path0[1], "Path 0.1");
                if (path0[1])
                {
                    conv.player_layer1[1] = EditorGUILayout.TextField("Player L1.1", conv.player_layer1[1]);
                    conv.npc_layer1[1] = EditorGUILayout.TextField("NPC L1.1", conv.npc_layer1[1]);

                    player_msg_count[2] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[2]);

                    //<----------------------> Path 0.1.0 (Layer 2) <----------------------> 
                    if (player_msg_count[2] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.0");

                        conv.player_layer2[4] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[4]);
                        conv.npc_layer2[4] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[4]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.1.1 (Layer 2) <----------------------> 
                    if (player_msg_count[2] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.1");
                        conv.player_layer2[5] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[5]);
                        conv.npc_layer2[5] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[5]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.1.2 (Layer 2) <----------------------> 
                    if (player_msg_count[2] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.2");
                        conv.player_layer2[6] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[6]);
                        conv.npc_layer2[6] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[6]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.1.3 (Layer 3) <----------------------> 
                    if (player_msg_count[2] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.1.3");
                        conv.player_layer2[7] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[7]);
                        conv.npc_layer2[7] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[7]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 0.2 (Layer 1) <----------------------> 
            if (player_msg_count[0] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[2] = EditorGUILayout.Foldout(path0[2], "Path 0.2");
                if (path0[2])
                {
                    conv.player_layer1[2] = EditorGUILayout.TextField("Player L1.2", conv.player_layer1[2]);
                    conv.npc_layer1[2] = EditorGUILayout.TextField("NPC L1.2", conv.npc_layer1[2]);

                    player_msg_count[3] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[3]);

                    //<----------------------> Path 0.2.0 (Layer 2) <----------------------> 
                    if (player_msg_count[3] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.0");

                        conv.player_layer2[8] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[8]);
                        conv.npc_layer2[8] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[8]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.2.1 (Layer 2) <----------------------> 
                    if (player_msg_count[3] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.1");
                        conv.player_layer2[9] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[9]);
                        conv.npc_layer2[9] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[9]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.2.2 (Layer 2) <----------------------> 
                    if (player_msg_count[3] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.2");
                        conv.player_layer2[10] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[10]);
                        conv.npc_layer2[10] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[10]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.2.3 (Layer 3) <----------------------> 
                    if (player_msg_count[3] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.2.3");
                        conv.player_layer2[11] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[11]);
                        conv.npc_layer2[11] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[11]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 0.3 (Layer 1) <----------------------> 
            if (player_msg_count[0] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[3] = EditorGUILayout.Foldout(path0[3], "Path 0.3");
                if (path0[3])
                {
                    conv.player_layer1[3] = EditorGUILayout.TextField("Player L1.2", conv.player_layer1[3]);
                    conv.npc_layer1[3] = EditorGUILayout.TextField("NPC L1.2", conv.npc_layer1[3]);

                    player_msg_count[4] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[4]);

                    //<----------------------> Path 0.3.0 (Layer 2) <----------------------> 
                    if (player_msg_count[4] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.0");

                        conv.player_layer2[12] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[12]);
                        conv.npc_layer2[12] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[12]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.3.1 (Layer 2) <----------------------> 
                    if (player_msg_count[4] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.1");
                        conv.player_layer2[13] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[13]);
                        conv.npc_layer2[13] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[13]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.3.2 (Layer 2) <----------------------> 
                    if (player_msg_count[4] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.2");
                        conv.player_layer2[14] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[14]);
                        conv.npc_layer2[14] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[14]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 0.3.3 (Layer 3) <----------------------> 
                    if (player_msg_count[4] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 0.3.3");
                        conv.player_layer2[15] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[15]);
                        conv.npc_layer2[15] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[15]);
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
                conv.player_layer0[1] = EditorGUILayout.TextField("Player L0.1", conv.player_layer0[1]);
            }
            if (conv.npc_layer0.Count < 2)
            {
                conv.npc_layer0.Add("");
            }
            else
            {
                conv.npc_layer0[1] = EditorGUILayout.TextField("NPC L0.0", conv.npc_layer0[1]);
            }

            player_msg_count[5] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[5]);

            //<----------------------> Path 1.0 (Layer 1) <---------------------->   
            if (player_msg_count[5] >= 1)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[4] = EditorGUILayout.Foldout(path0[4], "Path 1.0");
                if (path0[4])
                {
                    conv.player_layer1[4] = EditorGUILayout.TextField("Player L1.0", conv.player_layer1[4]);
                    conv.npc_layer1[4] = EditorGUILayout.TextField("NPC L1.0", conv.npc_layer1[4]);

                    player_msg_count[6] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[6]);

                    //<----------------------> Path 1.0.0 (Layer 2) <----------------------> 
                    if (player_msg_count[6] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.0");

                        conv.player_layer2[16] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[16]);
                        conv.npc_layer2[16] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[16]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.0.1 (Layer 2) <----------------------> 
                    if (player_msg_count[6] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.1");
                        conv.player_layer2[17] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[17]);
                        conv.npc_layer2[17] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[17]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.0.2 (Layer 2) <----------------------> 
                    if (player_msg_count[6] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.2");
                        conv.player_layer2[18] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[18]);
                        conv.npc_layer2[18] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[18]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.0.3 (Layer 3) <----------------------> 
                    if (player_msg_count[6] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.0.3");
                        conv.player_layer2[19] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[19]);
                        conv.npc_layer2[19] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[19]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 1.1 (Layer 1) <----------------------> 
            if (player_msg_count[5] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[5] = EditorGUILayout.Foldout(path0[5], "Path 1.1");
                if (path0[5])
                {
                    conv.player_layer1[5] = EditorGUILayout.TextField("Player L1.1", conv.player_layer1[5]);
                    conv.npc_layer1[5] = EditorGUILayout.TextField("NPC L1.1", conv.npc_layer1[5]);

                    player_msg_count[7] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[7]);

                    //<----------------------> Path 1.1.0 (Layer 2) <----------------------> 
                    if (player_msg_count[7] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.0");

                        conv.player_layer2[20] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[20]);
                        conv.npc_layer2[20] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[20]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.1.1 (Layer 2) <----------------------> 
                    if (player_msg_count[7] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.1");
                        conv.player_layer2[21] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[21]);
                        conv.npc_layer2[21] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[21]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.1.2 (Layer 2) <----------------------> 
                    if (player_msg_count[7] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.2");
                        conv.player_layer2[22] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[22]);
                        conv.npc_layer2[22] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[22]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.1.3 (Layer 3) <----------------------> 
                    if (player_msg_count[7] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.1.3");
                        conv.player_layer2[23] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[23]);
                        conv.npc_layer2[23] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[23]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 1.2 (Layer 1) <----------------------> 
            if (player_msg_count[5] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[6] = EditorGUILayout.Foldout(path0[6], "Path 1.2");
                if (path0[6])
                {
                    conv.player_layer1[6] = EditorGUILayout.TextField("Player L1.2", conv.player_layer1[6]);
                    conv.npc_layer1[6] = EditorGUILayout.TextField("NPC L1.2", conv.npc_layer1[6]);

                    player_msg_count[8] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[8]);

                    //<----------------------> Path 1.2.0 (Layer 2) <----------------------> 
                    if (player_msg_count[8] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.0");

                        conv.player_layer2[24] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[24]);
                        conv.npc_layer2[24] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[24]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.2.1 (Layer 2) <----------------------> 
                    if (player_msg_count[8] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.1");
                        conv.player_layer2[25] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[25]);
                        conv.npc_layer2[25] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[25]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.2.2 (Layer 2) <----------------------> 
                    if (player_msg_count[8] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.2");
                        conv.player_layer2[26] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[26]);
                        conv.npc_layer2[26] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[26]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.2.3 (Layer 3) <----------------------> 
                    if (player_msg_count[8] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.2.3");
                        conv.player_layer2[27] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[27]);
                        conv.npc_layer2[27] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[27]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 1.3 (Layer 1) <----------------------> 
            if (player_msg_count[5] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[7] = EditorGUILayout.Foldout(path0[7], "Path 1.3");
                if (path0[7])
                {
                    conv.player_layer1[7] = EditorGUILayout.TextField("Player L1.2", conv.player_layer1[7]);
                    conv.npc_layer1[7] = EditorGUILayout.TextField("NPC L1.2", conv.npc_layer1[7]);

                    player_msg_count[9] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[9]);

                    //<----------------------> Path 1.3.0 (Layer 2) <----------------------> 
                    if (player_msg_count[9] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.0");

                        conv.player_layer2[28] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[28]);
                        conv.npc_layer2[28] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[28]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.3.1 (Layer 2) <----------------------> 
                    if (player_msg_count[9] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.1");
                        conv.player_layer2[29] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[29]);
                        conv.npc_layer2[29] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[29]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.3.2 (Layer 2) <----------------------> 
                    if (player_msg_count[9] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.2");
                        conv.player_layer2[30] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[30]);
                        conv.npc_layer2[30] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[30]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 1.3.3 (Layer 3) <----------------------> 
                    if (player_msg_count[9] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 1.3.3");
                        conv.player_layer2[31] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[31]);
                        conv.npc_layer2[31] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[31]);
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
                conv.player_layer0[2] = EditorGUILayout.TextField("Player L0.2", conv.player_layer0[2]);
            }
            if (conv.npc_layer0.Count < 3)
            {
                conv.npc_layer0.Add("");
            }
            else
            {
                conv.npc_layer0[2] = EditorGUILayout.TextField("NPC L0.2", conv.npc_layer0[2]);
            }

            player_msg_count[10] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[10]);

            //<----------------------> Path 2.0 (Layer 1) <---------------------->   
            if (player_msg_count[10] >= 1)
            {

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[8] = EditorGUILayout.Foldout(path0[8], "Path 2.0");
                if (path0[8])
                {
                    conv.player_layer1[8] = EditorGUILayout.TextField("Player L1.0", conv.player_layer1[8]);
                    conv.npc_layer1[8] = EditorGUILayout.TextField("NPC L1.0", conv.npc_layer1[8]);

                    player_msg_count[11] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[11]);

                    //<----------------------> Path 2.0.0 (Layer 2) <----------------------> 
                    if (player_msg_count[11] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.0");

                        conv.player_layer2[32] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[32]);
                        conv.npc_layer2[32] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[32]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.0.1 (Layer 2) <----------------------> 
                    if (player_msg_count[11] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.1");
                        conv.player_layer2[33] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[33]);
                        conv.npc_layer2[33] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[33]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.0.2 (Layer 2) <----------------------> 
                    if (player_msg_count[11] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.2");
                        conv.player_layer2[34] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[34]);
                        conv.npc_layer2[34] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[34]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.0.3 (Layer 3) <----------------------> 
                    if (player_msg_count[11] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.0.3");
                        conv.player_layer2[35] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[35]);
                        conv.npc_layer2[35] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[35]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 2.1 (Layer 1) <----------------------> 
            if (player_msg_count[10] >= 2)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[9] = EditorGUILayout.Foldout(path0[9], "Path 2.1");
                if (path0[9])
                {
                    conv.player_layer1[9] = EditorGUILayout.TextField("Player L1.1", conv.player_layer1[9]);
                    conv.npc_layer1[9] = EditorGUILayout.TextField("NPC L1.1", conv.npc_layer1[9]);

                    player_msg_count[12] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[12]);

                    //<----------------------> Path 2.1.0 (Layer 2) <----------------------> 
                    if (player_msg_count[12] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.0");

                        conv.player_layer2[36] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[36]);
                        conv.npc_layer2[36] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[36]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.1.1 (Layer 2) <----------------------> 
                    if (player_msg_count[12] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.1");
                        conv.player_layer2[37] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[37]);
                        conv.npc_layer2[37] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[37]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.1.2 (Layer 2) <----------------------> 
                    if (player_msg_count[12] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.2");
                        conv.player_layer2[38] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[38]);
                        conv.npc_layer2[38] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[38]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.1.3 (Layer 3) <----------------------> 
                    if (player_msg_count[12] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.1.3");
                        conv.player_layer2[39] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[39]);
                        conv.npc_layer2[39] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[39]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 2.2 (Layer 1) <---------------------->
            if (player_msg_count[10] >= 3)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[10] = EditorGUILayout.Foldout(path0[10], "Path 2.2");
                if (path0[10])
                {
                    conv.player_layer1[10] = EditorGUILayout.TextField("Player L1.2", conv.player_layer1[10]);
                    conv.npc_layer1[10] = EditorGUILayout.TextField("NPC L1.2", conv.npc_layer1[10]);

                    player_msg_count[13] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[13]);

                    //<----------------------> Path 2.2.0 (Layer 2) <----------------------> 
                    if (player_msg_count[13] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.0");

                        conv.player_layer2[40] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[40]);
                        conv.npc_layer2[40] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[40]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.2.1 (Layer 2) <----------------------> 
                    if (player_msg_count[13] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.1");
                        conv.player_layer2[41] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[41]);
                        conv.npc_layer2[41] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[41]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.2.2 (Layer 2) <----------------------> 
                    if (player_msg_count[13] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.2");
                        conv.player_layer2[42] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[42]);
                        conv.npc_layer2[42] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[42]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.2.3 (Layer 3) <----------------------> 
                    if (player_msg_count[13] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.2.3");
                        conv.player_layer2[43] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[43]);
                        conv.npc_layer2[43] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[43]);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
            //<----------------------> Path 2.3 (Layer 1) <---------------------->
            //below
            if (player_msg_count[10] >= 4)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                path0[11] = EditorGUILayout.Foldout(path0[11], "Path 2.3");
                if (path0[11])
                {
                    conv.player_layer1[11] = EditorGUILayout.TextField("Player L1.2", conv.player_layer1[11]);
                    conv.npc_layer1[11] = EditorGUILayout.TextField("NPC L1.2", conv.npc_layer1[11]);

                    player_msg_count[14] = EditorGUILayout.DelayedIntField("# of answer options (max 4)", player_msg_count[14]);

                    //<----------------------> Path 2.3.0 (Layer 2) <----------------------> 
                    if (player_msg_count[14] >= 1)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.0");

                        conv.player_layer2[44] = EditorGUILayout.TextField("Player L2.0", conv.player_layer2[44]);
                        conv.npc_layer2[44] = EditorGUILayout.TextField("NPC L2.0", conv.npc_layer2[44]);

                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.3.1 (Layer 2) <----------------------> 
                    if (player_msg_count[14] >= 2)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.1");
                        conv.player_layer2[45] = EditorGUILayout.TextField("Player L2.1", conv.player_layer2[45]);
                        conv.npc_layer2[45] = EditorGUILayout.TextField("NPC L2.1", conv.npc_layer2[45]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.3.2 (Layer 2) <----------------------> 
                    if (player_msg_count[14] >= 3)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.2");
                        conv.player_layer2[46] = EditorGUILayout.TextField("Player L2.2", conv.player_layer2[46]);
                        conv.npc_layer2[46] = EditorGUILayout.TextField("NPC L2.2", conv.npc_layer2[46]);
                        EditorGUI.indentLevel--;
                    }
                    //<----------------------> Path 2.3.3 (Layer 3) <----------------------> 
                    if (player_msg_count[14] >= 4)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Path 2.3.3");
                        conv.player_layer2[47] = EditorGUILayout.TextField("Player L2.3", conv.player_layer2[47]);
                        conv.npc_layer2[47] = EditorGUILayout.TextField("NPC L2.3", conv.npc_layer2[47]);
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
        if(GUILayout.Button("With this button!"))
        {
            
            while(conv.player_layer0.Count < 4 + conv.cp_count)
            {
                conv.player_layer0.Add("");
            }
            while (conv.npc_layer0.Count < 4 + conv.cp_count)
            {
                conv.npc_layer0.Add("");
            }
            conv.cp_count += 1;
            while (conv.cp_struct.Count < conv.cp_count)
            {
                conv.cp_struct.Add("");
            }
            
            
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        path[4] = EditorGUILayout.Foldout(path[4], "Custom Path 0");
        if (path[4])
        {
            conv.player_layer0[4] = EditorGUILayout.TextField("Player Message", conv.player_layer0[4]);
            conv.npc_layer0[4] = EditorGUILayout.TextField("NPC Message", conv.npc_layer0[4]);

            EditorGUILayout.HelpBox("For help, see my instruction video :P \n Structure syntax: \"# of paths $ # of subpaths for path 0, ..., # of subpaths for path n $ # of subsubpaths for subpath 0, ..., # of subsubpaths for subpath m $ # of subsubsubpaths for subsubpath 0 ....\" and so on.. to recreate the flowchart, it would be \"4$4,4,4,4$4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4\". In this case you'd need to write 84 strings containing each player message and npc answer :P I will provide a better example in a newer version.", MessageType.Info);

            conv.cp_struct[0] = EditorGUILayout.TextField("Structure:", conv.cp_struct[0]);

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(cp_prop, new GUIContent("Seperate player & npc text by ♥"), true);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Test:");
        conv.testfield = EditorGUILayout.TextArea(conv.testfield);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        toggle_default_inspector = EditorGUILayout.Toggle("Show default inspector?", toggle_default_inspector);
        if (toggle_default_inspector)
        {
            base.OnInspectorGUI();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
