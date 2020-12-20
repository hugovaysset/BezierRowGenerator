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

    [SerializeField, HideInInspector]
    bool auto_set_control_points;

    public Transform field;
    // List<Vector3> corner_list = new List<Vector3>();
    
    public int nb_points;

    public Path(Vector3 init_point, Vector3 end_point, Transform f)
    {
        // init/end_point : user-defined anchor points to instantiate Path
        // field : transform of the plane containing the row
        // the two defined points are control points
        field = f;

        points = new List<Vector3>
        {
            init_point,
            init_point + (Vector3.forward + Vector3.right) * 0.5f,
            end_point + (Vector3.back + Vector3.left) * 0.5f,
            end_point
        };

        nb_points = 2;
    }

    public Path(Vector3 init_point, Vector3 end_point, Transform f, int nb_points_in_path)
    {
        // init/end_point : user-defined anchor points to instantiate Path
        // field : transform of the plane containing the row
        // nb_points_in_path : #(points to initialize path)
        // the two defined points are control points
        field = f;
        nb_points = nb_points_in_path;

        float row_length = (end_point - init_point).magnitude;
        points = new List<Vector3>
        {
            init_point,
            init_point + (end_point - init_point) / (2f * row_length),
            end_point - (end_point - init_point) / (2f * row_length),
            end_point
        };

        // stride to place each anchor point between init and end
        Vector3 stride = (end_point - init_point) / (nb_points - 1);
        for (int i = 1; i < nb_points - 1; i++) // nb_points > 2
        {
            // add jitter for each anchor point position
            Vector3 rand_x = Random.Range(- stride.magnitude, stride.magnitude) * Vector3.right;
            Vector3 rand_z = Random.Range(- stride.magnitude, stride.magnitude) * Vector3.forward;
            
            // compute anchor point and new control points positions
            Vector3 prev_anchor = points[(i - 1) * 3];
            Vector3 next_anchor = points[i * 3];
            Vector3 new_anchor = stride * i + init_point + rand_x + rand_z ;
            Vector3 control1 = new_anchor - (next_anchor - prev_anchor) / (5 * stride.magnitude);
            Vector3 control2 = 2 * new_anchor - control1;

            // insert the three new points in the points list
            points.Insert(i * 3 - 1, control1);
            points.Insert(i * 3, new_anchor);
            points.Insert(i * 3 + 1, control2);
        }
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

    public bool AutoSetControlPoints
    {
        get
        {
            return auto_set_control_points;
        }
        set
        {
            if (auto_set_control_points != value)
            {
                auto_set_control_points = value;
                if (auto_set_control_points)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }


    public void AddSegment(Vector3 new_anchor)
    {
        // To add element in the end of the path
        // Used for mouse-click generation

        // new control point for the previous extremity
        points.Add(2 * points[points.Count - 1] - points[points.Count - 2]); 
        // new control point half-way between new anchor and previous control
        points.Add((new_anchor + 0.5f * points[points.Count - 2]) / 1.5f);
        points.Add(new_anchor);

        if (auto_set_control_points)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }

        nb_points += 1;
    }

    public Vector3[] GetPointsInSegment(int i)
    {
        return new Vector3[] {points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]};
    }

    public void MovePoint(int i, Vector3 pos)
    {
        Vector3 delta_move = pos - points[i];

        if (i % 3 == 0 || !auto_set_control_points)
        {
            points[i] = pos;

            if (auto_set_control_points)
            {
                AutoSetAllAffectedControlPoints(i);
            }

            if (i % 3 == 0)
            {
                if (i + 1 < points.Count)
                {
                    points[i + 1] += delta_move;
                }
                if (i - 1 > 0)
                {
                    points[i - 1] += delta_move;
                }
            }
            else
            {
                bool next_point_is_anchor = (i + 1) % 3 == 0;
                int corresponding_control_index = (next_point_is_anchor) ? (i + 2) : i - 2;
                int anchor_index = (next_point_is_anchor) ? (i + 1) : (i - 1);

                if (corresponding_control_index > 0 && corresponding_control_index < points.Count)
                {
                    float dist = (points[anchor_index] - points[corresponding_control_index]).magnitude;
                    Vector3 dir = (points[anchor_index] - pos).normalized;
                    points[corresponding_control_index] = points[anchor_index] + dir * dist;
                }
            }
        }
    }

    void AutoSetAllAffectedControlPoints(int updated_anchor_index)
    {
        for (int i = updated_anchor_index - 3; i <= updated_anchor_index + 3; i += 3)
        {
            if (i >= 0 && i < points.Count)
            {
                AutoSetAnchorControlPoints(i);
            }
        }
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControlPoints(i);
        }
    }

    void AutoSetAnchorControlPoints(int anchor_index)
    {
        Vector3 anchor_pos = points[anchor_index];
        Vector3 dir = Vector3.zero;
        float[] neighbour_distances = new float[2];

        if (anchor_index - 3 >= 0)
        {
            Vector3 offset = points[anchor_index - 3] - anchor_pos;
            dir += offset.normalized;
            neighbour_distances[0] = offset.magnitude;
        }

        if (anchor_index + 3 < points.Count)
        {
            Vector3 offset = points[anchor_index + 3] - anchor_pos;
            dir -= offset.normalized;
            neighbour_distances[1] = - offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int control_index = anchor_index + i * 2 - 1;
            if (control_index >= 0 && control_index < points.Count)
            {
                points[control_index] = anchor_pos + dir * neighbour_distances[i] * 0.5f;
            }
        }
    }

}
