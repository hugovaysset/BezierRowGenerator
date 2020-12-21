using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldCreator))]
public class FieldEditor : Editor
{
    public FieldCreator creator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        int nbr = EditorGUILayout.IntField("Number of rows", creator.nb_rows);
        if (nbr != creator.nb_rows)
        {
            creator.nb_rows = nbr;
        }

        float inter_row_distance = EditorGUILayout.FloatField("Inter row distance", creator.inter_row_distance);
        if (inter_row_distance != creator.inter_row_distance)
        {
            creator.inter_row_distance = inter_row_distance;
        }

        if (GUILayout.Button("Generate all rows"))
        {
            creator.GenerateAllRows();
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }
    private void OnEnable()
    {
        creator = (FieldCreator)target;
    }

}
