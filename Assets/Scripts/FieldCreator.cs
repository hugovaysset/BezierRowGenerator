using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCreator : MonoBehaviour
{

    public GameObject field;
    // row that serves as origin to instantiate all the others
    public GameObject origin;
    [HideInInspector] public int nb_rows;
    [HideInInspector] public float inter_row_distance;

    // Start is called before the first frame update
/*    void Start()
    {
       
    }*/

    public void GenerateAllRows()
    {
        
        // Get the points list of the origin row
        PathCreator pc = origin.GetComponent<PathCreator>();
        Path p = pc.path;
        List<Vector3> points = pc.path.Points;

        // get the direction of translation = orthogonal to the row axis
        Vector3 direction = (Quaternion.Euler(90, 0, 0) * (points[0] - points[points.Count - 1])).normalized;
        Vector3 delta = direction * inter_row_distance;

        // instantiate duplications of the rows
        for (int i = 0; i < nb_rows / 2; i++)
        {
            Vector3 translation = (i + 1) * delta;
            GameObject obj_pos = new GameObject("Row" + (i + 1));
            GameObject obj_neg = new GameObject("Row" + (-(i + 1)));

            obj_pos.AddComponent<PathCreator>();
            obj_neg.AddComponent<PathCreator>();

            obj_pos.GetComponent<PathCreator>().CreatePath(p);
            obj_neg.GetComponent<PathCreator>().CreatePath(p);

            obj_pos.GetComponent<PathCreator>().TranslatePath(translation);
            obj_neg.GetComponent<PathCreator>().TranslatePath(-translation);
        }
    }

}
