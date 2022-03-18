using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// based on Building A Graph
// https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/
public class MathematicalSurfacesGraph : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10, 100)]
    int resolution = 10;
    // Start is called before the first frame update

    Transform[] points;

    [SerializeField]
    FunctionLibrary.FunctionName function = FunctionLibrary.FunctionName.Wave;

    void Start() {
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i] = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    void Update() {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time);
        }
    }
}
