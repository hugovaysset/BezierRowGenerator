using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldCreator))]
public class FieldEditor : Editor
{
    public FieldCreator creator;
    public Field field;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Generate Field Rows", EditorStyles.boldLabel);

        using (new EditorGUI.IndentLevelScope())
        {
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
                // initialize the field
                creator.CreateField();
                field = creator.field;
                creator.field_is_initialized = true;

                // initialize the rows
                creator.rows_list = new List<GameObject>();
                creator.GenerateAllRows();
                creator.rows_are_initialiazed = true;
            }

            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Initialize crops", EditorStyles.boldLabel);

        using (new EditorGUI.IndentLevelScope())
        {

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
            else if (!creator.field_is_initialized)
            {
                Debug.Log("Field is not yet initialized. Please press the Generate all rows button before" +
                    "trying to initialize the crops");
            }
        }

        EditorGUILayout.EndVertical();

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
