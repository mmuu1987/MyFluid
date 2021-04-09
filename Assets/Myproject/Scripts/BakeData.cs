using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 记录模型动画每个定点的位置的数据
/// </summary>
public class BakeData : MonoBehaviour
{
    public MeshFilter Mesh;

    private WaitForEndOfFrame _endFrame;
    List<Vector3Data> _posList = new List<Vector3Data>();

    public MegaPointCache MegaPointCache;
    public MegaModifyObject MegaModifier;

    /// <summary>
    /// 动画帧数
    /// </summary>
    public int FrameCount;

    /// <summary>
    /// 动画帧率
    /// </summary>
    private int _frame = 30;

    /// <summary>
    /// 动画时间
    /// </summary>
    private float _animatiomTime;
    // Start is called before the first frame update
    void Start()
    {
        _endFrame = new WaitForEndOfFrame();
        StartBake();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBake()
    {
       

        StartCoroutine(FrameEnd());

    }
    public IEnumerator FrameEnd()
    {
        float timeTemp = 0;
        MeshData data = new MeshData();

        data.VertexCount = Mesh.mesh.vertexCount;
        data.VertexSize = 3;

        _animatiomTime = FrameCount * 1f / _frame;

        MegaModifier.dynamicMesh = true;
        MegaPointCache.maxtime = _animatiomTime;


        string savePath = Application.streamingAssetsPath + "/data.txt";
        int count = 0;

        while (true)
        {
            timeTemp += Time.deltaTime;
            MegaPointCache.time = timeTemp;
            yield return _endFrame;

            //count++;
            //Debug.Log(timeTemp);



            Vector3[] temps = Mesh.mesh.vertices;
            Vector3Data [] des = new Vector3Data[temps.Length];
            //Debug.Log("temp[4] is " + temps[4]);
            for (int i = 0; i < temps.Length; i++)
            {
                des[i] = new Vector3Data(temps[i]);
            }
            _posList.AddRange(des);
            data.VertexPosData = _posList.ToArray();


            if (MegaPointCache.maxtime <=timeTemp)
            {

                using (Stream fStream = File.Create(savePath))
                {
                    BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                    binFormat.Serialize(fStream, data);
                    
                }

                yield break;
            }



        }
    }
}


[Serializable]
public class MeshData
{
    public int VertexCount;

    public int VertexSize;

    public Vector3Data[] VertexPosData;

}
[Serializable]
public class Vector3Data
{
    public float X;
    public float Y;
    public float Z;

    public Vector3Data(Vector3 vector3)
    {
        X = vector3.x;
        Y = vector3.y;
        Z = vector3.z;
    }

    public Vector3 ConevrtVector3()
    {
        Vector3 v = new Vector3(X, Y, Z);
        return v;
    }
}
