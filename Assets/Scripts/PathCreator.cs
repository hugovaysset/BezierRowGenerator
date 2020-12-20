using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating the path and holding a reference to it

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    // public Vector3 init_point = Vector3.zero;
    public Vector3 init_point = new Vector3(4, 0, 4);
    // public Vector3 end_point = (Vector3.back + Vector3.right) * 3;
    public Vector3 end_point = new Vector3(-4, 0, -4);

    // field containing the rows
    // used to constrain movements of points
    public Transform field;

    // number of points at path initialization
    [Range(0, 10)]public int nb_points = 7;

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
}
