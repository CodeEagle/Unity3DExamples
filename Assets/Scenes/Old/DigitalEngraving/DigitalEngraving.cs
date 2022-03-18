using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
// from post https://mp.weixin.qq.com/s/X5-_hf546UofK9fAdjQl1g
public class DigitalEngraving : MonoBehaviour {
    Transform pointPrefab;
    Transform[] points;
    [Range(10, 100)] public int resolution = 10;
    public GraphFunctionName function = GraphFunctionName.Sine;

    static GraphFunction[] functions = {
        SineFunction, Sine2DFunction, MultiSineFunction, MultiSine2DFunction, Ripple, Cylinder, Sphere, Tours
    };

    const float pi = Mathf.PI;
    // Start is called before the first frame update
    void Start() {
        CraetePrefab();
        CreateGraph();
    }

    // Update is called once per frame
    void Update() {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        float step = 2f / resolution;
        for (int i = 0, z = 0; z < points.Length; z++) {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++) {
                float u = (x + 0.5f) * step - 1f;
                if (i < points.Length) {
                    points[i].localPosition = f(u, v, t);
                }
            }
        }
    }

    private void CreateGraph() {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        points = new Transform[resolution * resolution];

        for (int i = 0; i < points.Length; i++) {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }
    private void CraetePrefab() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = "cube";
        obj.GetComponent<BoxCollider>().enabled = false;

        Shader s = Resources.Load<Shader>("DigitalEngraving/ColoredPoint");
        string matPath = "Assets/Resources/DigitalEngraving/ColoredPoint.mat";
        Material newMaterial = new Material(s);
        // https://answers.unity.com/questions/479611/setting-a-material-texture-and-saving-it.html
        AssetDatabase.CreateAsset(newMaterial, matPath);
        obj.GetComponent<MeshRenderer>().material = newMaterial;

        string pathToSave = "Assets/Resources/DigitalEngraving/" + obj.name + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(obj, pathToSave);
        Destroy(obj);

        pointPrefab = (Resources.Load("DigitalEngraving/cube") as GameObject).transform;
    }

    private static Vector3 SineFunction(float x, float z, float t) {
        Vector3 p = Vector3.zero;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }
    private static Vector3 Sine2DFunction(float x, float z, float t) {
        Vector3 p = Vector3.zero;
        p.x = x;
        p.z = z;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        return p;
    }


    private static Vector3 MultiSineFunction(float x, float z, float t) {
        Vector3 p = Vector3.zero;
        p.x = x;
        p.z = z;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        return p;
    }

    private static Vector3 MultiSine2DFunction(float x, float z, float t) {
        Vector3 p = Vector3.zero;
        p.x = x;
        p.z = z;
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        p.y = y;
        return p;
    }

    private static Vector3 Ripple(float x, float z, float t) {
        Vector3 p = Vector3.zero;
        p.x = x;
        p.z = z;
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * d - t));
        y /= 1f + 1f * d;
        p.y = y;
        return p;
    }

    private static Vector3 Cylinder(float u, float v, float t) {
        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) / 5f;
        Vector3 p = Vector3.zero;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    private static Vector3 Sphere(float u, float v, float t) {

        Vector3 p = Vector3.zero;
        // 球体
        // float r = Mathf.Cos(pi * 0.5f * v);
        // p.x = r * Mathf.Sin(pi * u);
        // p.y = Mathf.Sin(pi * v * 0.5f);
        // p.z = r * Mathf.Cos(pi * u);

        // 脉冲球
        float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        // https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/
        // float r = 0.9f + 0.1f * Mathf.Sin(pi * (6f * u + 4f * v + t));
        r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(pi * 0.5f * v);
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * v * 0.5f);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    private static Vector3 Tours(float u, float v, float t) {
        Vector3 p = Vector3.zero;
        // 环环面
        // float r1 = 1f;
        // float r2 = 0.5f;

        // 动态
        // float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        // float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.1f;

        // 
        // https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/
        float r1 = 0.7f + 0.1f * Mathf.Sin(pi * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Mathf.Sin(pi * (8f * u + 4f * v + 2f * t));

        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }
}
