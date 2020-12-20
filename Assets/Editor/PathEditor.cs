using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Working on the path in the scene view
// A path is defined by 
// - point A and point B
// - Number of intermediary points
// - Tangents orientation

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create new path"))
        {
            creator.CreatePath();
            path = creator.path;
            SceneView.RepaintAll();
        }

    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event gui_event = Event.current;
        Vector3 mouse_position = HandleUtility.GUIPointToWorldRay(
                                                    gui_event.mousePosition).origin;
        if ((gui_event.type == EventType.MouseDown && gui_event.button == 0 && gui_event.shift)
            || (gui_event.isMouse && gui_event.type == EventType.MouseDown && gui_event.clickCount == 5))
        {
            Undo.RecordObject(creator, "Add Segment");
            path.AddSegment(mouse_position);
        }
    }

    void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector3[] points = path.GetPointsInSegment(i);
            Handles.DrawBezier(points[0], points[3], points[1], points[2],
                                Color.blue, null, 2);
            Handles.color = Color.black;
            Handles.DrawLine(points[0], points[1]); // anchor to tangent
            Handles.DrawLine(points[2], points[3]);
        }

        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector3 new_pos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 
                              .1f, Vector3.zero, Handles.CylinderHandleCap);
            if (path[i] != new_pos)
            {
                Undo.RecordObject(creator, "Move Point");
                path.MovePoint(i, new_pos);
            }
        }
    }

    private void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }

        path = creator.path;
    }
}
