using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class Example : MonoBehaviour
{

    void Update()
    {
        Mesh mesh = this.transform.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int p = 0;
        int flag = -1;
        float maxheight = 0.0F;
        while (p < vertices.Length)
        {
            if (vertices[p].z > maxheight)
            {
                maxheight = vertices[p].z;
                flag = p;
            }
            p++;
        }
        //vertices[flag] += new Vector3(0, 0, maxheight);

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "Mesh"))
        {
            Mesh mesh = this.transform.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Debug.Log("vertices[" + i + "].x  " + vertices[i].x + "   vertices[" + i + "].y  " + vertices[i].y + "   vertices[" + i + "].z  " + vertices[i].z);
                vertices[i].x *= 2;
                vertices[i].y *= 2;
                vertices[i].z *= 2;

            }
            mesh.vertices = vertices;


        }
    }
}