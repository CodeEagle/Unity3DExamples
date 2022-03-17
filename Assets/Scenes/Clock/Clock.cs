using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
// from post https://mp.weixin.qq.com/s/QaEZuMRGTf07pml_h1rhxA
public class Clock : MonoBehaviour
{
    public Transform MainCamera;
    public bool Continuous = true;
    public Transform BasicNode;
    Transform hourTick;
    Transform minuteTick;
    Transform secondTick;

    float degreePerHour = 30f;
    float degreePerMinute = 6f;
    float degreePerSecond = 6f;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera.transform.position = new Vector3(0, 10, 0);
        // https://blog.csdn.net/icevmj/article/details/80280929
        MainCamera.transform.Rotate(new Vector3(90, 0, 90), Space.World);
        BasicNode.name = "Clock";
        CreateClockFace();
        CreateIndicator();
        CreateHourTick();
        CreateMinuteTick();
        CreateSecondTick();
        CreateSound();
    }


    // Update is called once per frame
    void Update()
    {   // - 3 是为了适应上面创建的时间指示所做的旋转
        float rotateOffset = 3;
        // 每个字数间,跳动五次
        float tickPerMinuteSecond = 5;
        float hour = 0, minute = 0, second = 0;

        if (Continuous)
        {

            TimeSpan n = DateTime.Now.TimeOfDay;
            hour = (float)n.TotalHours;
            minute = (float)n.TotalMinutes;
            second = (float)n.TotalSeconds;
        }
        else
        {

            DateTime n = DateTime.Now;
            hour = n.Hour;
            minute = n.Minute;
            second = n.Second;
        }
        hour = (hour - rotateOffset) * degreePerHour;
        minute = (minute - rotateOffset * tickPerMinuteSecond) * degreePerMinute;
        second = (second - rotateOffset * tickPerMinuteSecond) * degreePerSecond;

        hourTick.localRotation = Quaternion.Euler(0f, hour, 0f);
        minuteTick.localRotation = Quaternion.Euler(0f, minute, 0f);
        secondTick.localRotation = Quaternion.Euler(0f, second, 0f);

    }

    // 创建秒针声音
    private void CreateSound()
    {
        GameObject obj = new GameObject();
        obj.name = "TicTok";
        AudioSource a = obj.AddComponent<AudioSource>();
        // 要新建 Resources 文件夹
        // 资源不用加 后缀,如 tictok.mp3, 使用 tictok 即可
        a.clip = Resources.Load<AudioClip>("Clock/tictok");
        a.playOnAwake = true;
        a.loop = true;
        obj.transform.parent = MainCamera;
        a.Play();
    }

    // 创建表盘
    private void CreateClockFace()
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.parent = BasicNode;
        cylinder.transform.localScale = new Vector3(10f, 0.1f, 10f);
        cylinder.name = "Face";
    }

    // 小时指示器
    private void CreateIndicator()
    {
        Material myNewMaterial = new Material(Shader.Find("Standard"));
        myNewMaterial.color = new Color(0.28f, 0.28f, 0.28f);
        for (int i = 0; i < 12; i++)
        {
            // 制做一个临时的 parentCube
            // 实现旋转
            // 默认的中心点就是原点
            // 添加 Hour Indicator 后,设置好位置和材质
            // 然后添加旋转
            // 最后添加回去 ContentNode
            // 删除 parentCube
            GameObject parentCube = new GameObject();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = parentCube.transform;
            cube.transform.localScale = new Vector3(0.5f, 0.2f, 1f);
            cube.transform.position = new Vector3(0, 0.2f, 4f);
            // https://answers.unity.com/questions/59355/change-the-material-on-an-object-in-a-script.html
            cube.GetComponent<MeshRenderer>().material = myNewMaterial;
            cube.name = "Hour Indicator " + (i + 1);
            // i - 2 是为了 对应 Hour Indicator 的位置
            parentCube.transform.Rotate(new Vector3(0, (i - 2) * 30, 0), Space.World);
            cube.transform.parent = BasicNode;
            Destroy(parentCube);
        }
    }
    // 创建时针
    private void CreateHourTick()
    {
        Material myNewMaterial = new Material(Shader.Find("Standard"));
        myNewMaterial.color = new Color(0.28f, 0.28f, 0.28f);
        GameObject parentCube = new GameObject();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = parentCube.transform;
        cube.transform.localScale = new Vector3(0.3f, 0.2f, 2.5f);
        cube.transform.position = new Vector3(0, 0.2f, 0.7f);
        cube.GetComponent<MeshRenderer>().material = myNewMaterial;
        parentCube.name = "Hour Arm";
        cube.name = "Arm";
        parentCube.transform.parent = BasicNode;
        hourTick = parentCube.transform;
    }

    // 创建分针
    private void CreateMinuteTick()
    {

        Material myNewMaterial = new Material(Shader.Find("Standard"));
        myNewMaterial.color = new Color(0.28f, 0.28f, 0.28f);
        GameObject parentCube = new GameObject();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = parentCube.transform;
        cube.transform.localScale = new Vector3(0.2f, 0.15f, 4f);
        cube.transform.position = new Vector3(0, 0.375f, 1f);
        cube.GetComponent<MeshRenderer>().material = myNewMaterial;
        parentCube.name = "Minute Arm";
        cube.name = "Arm";
        parentCube.transform.parent = BasicNode;
        minuteTick = parentCube.transform;
    }

    // 创建秒针
    private void CreateSecondTick()
    {
        Material myNewMaterial = new Material(Shader.Find("Standard"));
        myNewMaterial.color = new Color(0.77f, 0, 0f);
        GameObject parentCube = new GameObject();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = parentCube.transform;
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 5f);
        cube.transform.position = new Vector3(0, 0.5f, 1.25f);
        cube.GetComponent<MeshRenderer>().material = myNewMaterial;
        parentCube.name = "Second Arm";
        cube.name = "Arm";
        parentCube.transform.parent = BasicNode;
        secondTick = parentCube.transform;
    }
}
