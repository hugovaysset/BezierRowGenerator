using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropInitializer : MonoBehaviour
{

    public float inter_plant_distance = .1f;
    public float resolution = 1;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenelySpacePoints(inter_plant_distance, resolution);

        foreach (Vector3 p in points)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = p;
            g.transform.localScale = Vector3.one * inter_plant_distance * 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
