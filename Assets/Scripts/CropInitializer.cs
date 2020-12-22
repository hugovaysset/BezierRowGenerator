using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropInitializer : MonoBehaviour
{
    [HideInInspector]
    public float inter_plant_distance;
    // [HideInInspector]
    public float spacing = .1f;
    // [HideInInspector]
    public float resolution = 1;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacePoints(spacing, resolution);
        // Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenelySpacePoints(inter_plant_distance);

        foreach (Vector3 p in points)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = p;
            g.transform.localScale = Vector3.one * spacing * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
