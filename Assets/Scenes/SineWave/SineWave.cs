using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// from post https://mp.weixin.qq.com/s/fE5BMEx_gSJ7GtcmxZE6sQ
public class SineWave : MonoBehaviour
{
    Transform pointPrefab;
    Transform[] points;
    [Range(10, 100)] public int resolution = 10;
    // Start is called before the first frame update
    void Start()
    {
        CraetePrefab();
        CreateGraph();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            point.localPosition = position;
        }
    }

    private void CreateGraph()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position = Vector3.zero;
        points = new Transform[resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            position.y = position.x * position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }
    private void CraetePrefab()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = "cube";
        obj.GetComponent<BoxCollider>().enabled = false;

        Shader s = Resources.Load<Shader>("SineWave/ColoredPoint");
        string matPath = "Assets/Resources/SineWave/ColoredPoint.mat";
        Material newMaterial = new Material(s);
        // https://answers.unity.com/questions/479611/setting-a-material-texture-and-saving-it.html
        AssetDatabase.CreateAsset(newMaterial, matPath);
        obj.GetComponent<MeshRenderer>().material = newMaterial;

        string pathToSave = "Assets/Resources/SineWave/" + obj.name + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(obj, pathToSave);
        Destroy(obj);

        pointPrefab = (Resources.Load("SineWave/cube") as GameObject).transform;
    }
}
