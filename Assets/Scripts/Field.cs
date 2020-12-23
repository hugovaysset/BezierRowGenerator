using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    public GameObject field_go;

    public Vector3 left_sup_corner;
    public Vector3 left_inf_corner;
    public Vector3 right_sup_corner;
    public Vector3 right_inf_corner;

   public Field(GameObject go)
    {
        field_go = go;
        Vector3[] mesh = go.GetComponent<MeshFilter>().sharedMesh.vertices;
        
        left_sup_corner = go.transform.TransformPoint(mesh[0]);
        left_inf_corner = go.transform.TransformPoint(mesh[10]);
        right_sup_corner = go.transform.TransformPoint(mesh[110]);
        right_inf_corner = go.transform.TransformPoint(mesh[120]);
    }

    // check if a vector is inside a (X, Z) finite (plane)
    // suppose that the plane is exactly oriented on the (X, Z) plane
    public bool isInTheField(Vector3 v)
    {
        return (left_inf_corner.x <= v.x && v.x <= right_sup_corner.x
        && v.z <= left_inf_corner.z && right_sup_corner.z <= v.z);
    }
}
