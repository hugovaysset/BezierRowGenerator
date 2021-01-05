using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCreator : MonoBehaviour
{
    public GameObject field_go;
    public Field field;
    // row that serves as origin to instantiate all the others
    public GameObject origin;
    public GameObject crop_model;
    [HideInInspector, Range(1, 15)] public int nb_rows;
    [HideInInspector] public float inter_row_distance;
    [HideInInspector] public List<GameObject> rows_list = new List<GameObject>();
    [HideInInspector] public List<GameObject> crops_list = new List<GameObject>();  // to store crops or crop positions ?
    [HideInInspector] public float inter_crop_distance;
    [HideInInspector] public float resolution;
    [HideInInspector] public bool rows_are_initialiazed = false;
    [HideInInspector] public bool field_is_initialized = false;
    [HideInInspector] public bool crops_are_initialized = false;

    public void CreateField()
    {
        field = new Field(field_go);
    }

    public void GenerateAllRows()
    {
        // Get the points list of the origin row
        PathCreator pc = origin.GetComponent<PathCreator>();
        Path p = pc.path;
        List<Vector3> points = pc.path.Points;

        // the origin row is also added into the field.
        rows_list.Add(origin);

        // get the direction of translation = orthogonal to the row axis
        Vector3 direction = Vector3.Cross(points[0] - points[points.Count - 1], Vector3.up).normalized;
        Vector3 delta = direction * inter_row_distance;
        Vector3 translation = delta;

        // instantiate duplications of the rows

        for (int i = 0; i < nb_rows - 1; i += 2)
        {
            GameObject obj_pos = new GameObject("Row" + (i+1));
            obj_pos.AddComponent<PathCreator>();
            obj_pos.GetComponent<PathCreator>().CreatePath(p);
            obj_pos.GetComponent<PathCreator>().TranslatePath(translation);
            obj_pos.transform.parent = field_go.transform;
            rows_list.Add(obj_pos);
            // generate only one row to achieve an even number of rows in the end
            if (nb_rows % 2 == 0 && i == nb_rows - 2) 
            {
                continue;
            }
            // generate rows two by two (on both sides of the origin)
            else
            {
                GameObject obj_neg = new GameObject("Row" + -(i+1));
                obj_neg.AddComponent<PathCreator>();
                obj_neg.GetComponent<PathCreator>().CreatePath(p);
                obj_neg.GetComponent<PathCreator>().TranslatePath(-translation);
                obj_neg.transform.parent = field_go.transform;
                rows_list.Add(obj_neg);
            }

            translation += delta;
        }
    }

    public void DeleteRows()
    {
        if (rows_list.Count > 1)
        {
            for (int i = 1; i < rows_list.Count; i++)
            {
                DestroyImmediate(rows_list[i]);
            }
            rows_list = new List<GameObject>();
            rows_list.Add(origin);
        }
    }

    public void InitializeCrops()
    {
        // iterate on each row of the field
        foreach (GameObject row in rows_list)
        {
            Vector3[] points = row.GetComponent<PathCreator>().path.CalculateEvenlySpacePoints(inter_crop_distance, resolution);

            // Set spheres on the path
            foreach (Vector3 p in points)
            {
                if (field.isInTheField(p))
                {
                    GameObject g = GameObject.Instantiate(crop_model);
                    g.transform.position = p;
                    g.transform.localScale = Vector3.one * inter_crop_distance * 0.5f;
                    g.transform.parent = row.transform;
                    crops_list.Add(g);
                }
            }
        }
    }

    public void DeleteCrops()
    {
        if (crops_list.Count > 0)
        {
            for (int i = 0; i < crops_list.Count; i++)
            {
                DestroyImmediate(crops_list[i]);
            }
            crops_list = new List<GameObject>();
        }
    }
}
