using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCreator : MonoBehaviour
{

    public GameObject field;
    public GameObject row;
    [HideInInspector] public int nb_rows;
    [HideInInspector] public float inter_row_distance;
    [HideInInspector] public bool generate_all_raws;

    // Start is called before the first frame update
    void Start()
    {
        if (generate_all_raws)
        {
            /*GenerateAllRows();*/
        }
    }

/*    public void GenerateAllRows()
    {
        // get the direction of translation = orthogonal to the row axis
        Vector3 direction = (Quaternion.Euler(90, 0, 0) * (row.Points[0] - row.Points[row.Points.Count - 1])).normalized;
        Vector3 translation = direction * inter_row_distance;

        // instantiate duplications of the row
        for (int i = 0; i < nb_rows / 2; i++)
        {
            row new_row_pos = row.Translate(translation);
            row new_row_neg = row.Translate(-translation);

            GameObject obj_pos = new GameObject("Row" + (i + 1));
            GameObject obj_neg = new GameObject("Row" + (-(i + 1)));

            obj_pos.AddComponent<rowCreator>();
            obj_neg.AddComponent<rowCreator>();

            translation += translation;
        }
    }*/

}
