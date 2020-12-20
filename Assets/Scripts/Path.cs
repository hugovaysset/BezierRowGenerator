using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Managing the points in the path

[System.Serializable]
public class Path {

    // SerializedField : also serialize the private objects
    // HideInInspector : not shown in the inspector
    [SerializeField, HideInInspector]
    List<Vector3> points; // convention : one anchor, then its control points

    public Path(Vector3 init_point, Vector3 end_point)
    {
        // init/end_point : user-defined anchor points to instantiate Path
        // the two defined points are control points
        points = new List<Vector3>
        {
            init_point,
            init_point + (Vector3.forward + Vector3.right) * 0.5f,
            end_point + (Vector3.back + Vector3.left) * 0.5f,
            end_point
    };
    }

    public Vector3 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        // (n - 1) segments where n is #(anchor points)
        // and (3n - 2) in total in path.points
        get {
            return (points.Count - 1) / 3;
        }
    }

    public void AddSegment(Vector3 new_anchor)
    {
        // new control point for the previous extremity
        points.Add(2 * points[points.Count - 1] - points[points.Count - 2]); 
        // new control point half-way between new anchor and previous control
        points.Add((new_anchor + 0.5f * points[points.Count - 2]) / 1.5f);
        points.Add(new_anchor);
    }

    public Vector3[] GetPointsInSegment(int i)
    {
        return new Vector3[] {points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]};
    }

    public void MovePoint(int i, Vector3 pos)
    {
        points[i] = pos;
    }

}
