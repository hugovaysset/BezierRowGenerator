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
            creator.rows_list = new List<GameObject>();
            creator.GenerateAllRows();
            creator.rows_are_initialiazed = true;
        }

        float inter_crop_distance = EditorGUILayout.FloatField("Inter crop distance", creator.inter_crop_distance);
        if (inter_crop_distance != creator.inter_crop_distance)
        {
            creator.inter_crop_distance = inter_crop_distance;
        }

        float res = EditorGUILayout.FloatField("Resolution", creator.resolution);
        if (res != creator.resolution)
        {
            creator.resolution = res;
        }

        if (GUILayout.Button("Initialize all crops") && creator.rows_are_initialiazed)
        {
            creator.InitializeCrops();
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
