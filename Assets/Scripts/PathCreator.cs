using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating the path and holding a reference to the current path

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public Vector3 init_point = Vector3.zero;
    public Vector3 end_point = Vector3.forward;

    public void CreatePath()
    {
        path = new Path(init_point, end_point);
    }
}
