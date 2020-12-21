using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating the path and holding a reference to it

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    // public Vector3 init_point = Vector3.zero;
    [HideInInspector]
    public Vector3 init_point = new Vector3(4, 0, 4);
    // public Vector3 end_point = (Vector3.back + Vector3.right) * 3;
    [HideInInspector]
    public Vector3 end_point = new Vector3(-4, 0, -4);

    [HideInInspector]
    public int nb_rows;

    [HideInInspector]
    public float inter_row_distance;

    // field containing the rows
    // used to constrain movements of points
    public GameObject field;

    // number of points at path initialization
    [Range(0, 10), HideInInspector]
    public int nb_points = 1;

    public int NbPoints
    {
        get
        {
            return nb_points;
        }
        set
        {
            nb_points = value;
        }
    }

    public void CreatePath()
    {
        if (nb_points > 2)
        {
            path = new Path(init_point, end_point, field, nb_points);
        }
        else
        {
            path = new Path(init_point, end_point, field);
        }
    }

    public void GenerateAllRows()
    {
        // get the direction of translation = orthogonal to the row axis
        Vector3 direction = (Quaternion.Euler(90, 0, 0) * (path.Points[0] - path.Points[path.Points.Count - 1])).normalized;
        Vector3 translation = direction * inter_row_distance;

        // instantiate duplications of the row
        for (int i = 0; i < nb_rows / 2; i++)
        {
            Path new_row_pos = path.Translate(translation);
            Path new_row_neg = path.Translate(-translation);

            GameObject obj_pos = new GameObject("Row" + i);
            GameObject obj_neg = new GameObject("Row" + (-i));

            obj_pos = Instantiate(obj_pos, Vector3.zero, Quaternion.identity);
            obj_neg = Instantiate(obj_neg, Vector3.zero, Quaternion.identity);

            translation += translation;
        }
    }

}
