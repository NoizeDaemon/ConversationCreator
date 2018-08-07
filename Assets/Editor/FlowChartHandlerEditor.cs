using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


[CustomEditor(typeof(FlowChartHandler))]
[CanEditMultipleObjects]
public class FlowChartHandlerEditor : Editor {

    public bool toggle_default_inspector;
    public bool fc_initialized;


    public override void OnInspectorGUI()
    {
        FlowChartHandler flow = (FlowChartHandler)target;


        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Initialize"))
        {
            flow.Initialize();
            fc_initialized = true;
        }

        if (GUILayout.Button("Update"))
        {
            flow.UpdateChart();
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Clear"))
        {
            flow.ClearChart();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        if (fc_initialized)
        {
            EditorGUILayout.HelpBox("Chart succesfully initialized!", MessageType.Info);
        }

        toggle_default_inspector = EditorGUILayout.Toggle("Toggle default inspector.", toggle_default_inspector);
        if (toggle_default_inspector)
        {
            base.OnInspectorGUI();
        }
        

    }
}
