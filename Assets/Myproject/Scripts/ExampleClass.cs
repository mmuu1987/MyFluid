using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

public class ExampleClass: MonoBehaviour
{
    public int instanceCount = 100000;
    public Mesh instanceMesh;
    public Material instanceMaterial;
    public int subMeshIndex = 0;

    private int cachedInstanceCount = -1;
    private int cachedSubMeshIndex = -1;
    private ComputeBuffer positionBuffer;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    void Start()
    {
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        UpdateBuffers();
    }

    void Update()
    {
        // Update starting position buffer
        if (cachedInstanceCount != instanceCount || cachedSubMeshIndex != subMeshIndex)
            UpdateBuffers();

        // Pad input
        if (Input.GetAxisRaw("Horizontal") != 0.0f)
            instanceCount = (int)Mathf.Clamp(instanceCount + Input.GetAxis("Horizontal") * 40000, 1.0f, 5000000.0f);

        // Render
        Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer,0,null,ShadowCastingMode.On,true);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(265, 25, 200, 30), "Instance Count: " + instanceCount.ToString());
        instanceCount = (int)GUI.HorizontalSlider(new Rect(25, 20, 200, 30), (float)instanceCount, 1.0f, 5000000.0f);
    }

    void UpdateBuffers()
    {
        // Ensure submesh index is in range
        if (instanceMesh != null)
            subMeshIndex = Mathf.Clamp(subMeshIndex, 0, instanceMesh.subMeshCount - 1);

        // Positions
        if (positionBuffer != null)
            positionBuffer.Release();
        int stride = Marshal.SizeOf(typeof(PosAndDir));
        positionBuffer = new ComputeBuffer(instanceCount, stride);
        PosAndDir[] positions = new PosAndDir[instanceCount];
        for (int i = 0; i < instanceCount; i++)
        {
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            float distance = Random.Range(20.0f, 100.0f);
            float height = Random.Range(-2.0f, 2.0f);
            float size = Random.Range(0.05f, 0.25f);
            positions[i].position = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
        }
        positionBuffer.SetData(positions);
        instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

        // Indirect args
        if (instanceMesh != null)
        {
            args[0] = (uint)instanceMesh.GetIndexCount(subMeshIndex);
            args[1] = (uint)instanceCount;
            args[2] = (uint)instanceMesh.GetIndexStart(subMeshIndex);
            args[3] = (uint)instanceMesh.GetBaseVertex(subMeshIndex);
        }
        else
        {
            args[0] = args[1] = args[2] = args[3] = 0;
        }
        argsBuffer.SetData(args);

        cachedInstanceCount = instanceCount;
        cachedSubMeshIndex = subMeshIndex;
    }

    void OnDisable()
    {
        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;
    }
}
/// <summary>
/// 传递给GPU的结构体，在不同的运动类型，变量的意义有些不一样
/// </summary>
public struct PosAndDir
{
    public Vector4 position;
    /// <summary>
    /// 一般指速度，在不同的运动类有不同的意义
    /// </summary>
    public Vector4 velocity;
    /// <summary>
    /// 物体初始速度
    /// </summary>
    public Vector3 initialVelocity;
    /// <summary>
    /// 初始状态的位置
    /// </summary>
    public Vector4 originalPos;

    /// <summary>
    /// 移动到的目标点
    /// </summary>
    public Vector3 moveTarget;

    /// <summary>
    /// 粒子靠这个向量来自动移动
    /// </summary>
    public Vector3 moveDir;

    /// <summary>
    /// 所在的行和列的位置
    /// </summary>
    public Vector2 indexRC;

    /// <summary>
    /// 索要表现的贴图
    /// </summary>
    public int picIndex;

    /// <summary>
    /// 显示图片局部的index
    /// </summary>
    public int bigIndex;
    /// <summary>
    /// 第一套 UV加UV偏移,可用来填装其他参数
    /// </summary>
    public Vector4 uvOffset;
    /// <summary>
    /// 第二套UV加UV偏移 ,可用来填装其他参数
    /// </summary>
    public Vector4 uv2Offset;

    /// <summary>
    /// 状态码
    /// </summary>
    public int stateCode;



    public PosAndDir(int id)
    {
        position = new Vector4();


        velocity = new Vector3();
        initialVelocity = new Vector3();
        originalPos = new Vector4();
        moveTarget = new Vector3();
        moveDir = new Vector3();
        indexRC = new Vector2();

        picIndex = id;
        bigIndex = 1;
        uvOffset = new Vector4();

        uv2Offset = new Vector4();
        stateCode = -1;
    }

    /// <summary>
    /// 跟另个数据进行排序
    /// </summary>
    public bool Sort(PosAndDir otherData)
    {
        if (this.position.z >= otherData.position.z) return false;
        return true;
    }


}
