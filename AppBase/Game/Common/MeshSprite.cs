using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshSprite : MonoBehaviour
{
    public GameObject objGame;
    public Camera mainCam;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    public List<Vector3> listPoint;

    // Use this for initialization
    void Start()
    {

        listPoint = new List<Vector3>();


        //   GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        // mesh.name = "WordWrite Mesh";
        mesh = GetComponent<MeshFilter>().mesh;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        //  mr.material = new Material(Shader.Find("Custom/ShaderMesh"));
        //  string pic = Common.GAME_RES_DIR + "/animal/draw/001.png";
        //  Texture2D tex = LoadTexture.LoadFromAsset(pic);
        //  mr.material.SetTexture("_MainTex",tex);

        int count = 2;
        Material[] listmat = new Material[count];
        //listmat[0] = mat;
        //Material [] listmat = {mat,mat};
        //new Material[count];
        //  mr.materials[0] = mat;
        // mr.materials = new Material[count];
        for (int i = 0; i < count; i++)
        {
            string strshader = "Custom/ShaderMesh";
            if (i == 0)
            {
                strshader = "Custom/ShaderMeshBg";
            }
            Material mat = new Material(Shader.Find(strshader));
            listmat[i] = mat;
            string pic = Common.GAME_RES_DIR + "/animal/draw/001.png";
            Texture2D tex = LoadTexture.LoadFromAsset(pic);
            mat.SetTexture("_MainTex", tex);

            string picmask = Common.GAME_RES_DIR + "/animal/mask/001.png";
            Texture2D texmask = LoadTexture.LoadFromAsset(picmask);
            mat.SetTexture("_TexMask", texmask);

            Color cr = new Color(0.2f, 0, 0, 1f);
            if (i == 0)
            {
                cr = new Color(0.0f, 0, 0.2f, 1f);
            }
            mat.SetColor("_ColorMask", cr);
        }
        mr.materials = listmat;


        AddPoint(new Vector3(-5, 0, 0));
        AddPoint(new Vector3(2, 0, 0));
        AddPoint(new Vector3(5, 5, 0));
        // AddPoint(new Vector3(4, 2, 0));
        Draw();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddPoint(Vector3 vec)
    {
        if (listPoint == null)
        {
            Debug.Log("AddPoint:listPoint=null");
            return;
        }

        listPoint.Add(vec);


    }


    public void Draw()
    {

        if (listPoint == null)
        {
            return;
        }

        mesh.Clear();
        int vlen = listPoint.Count;
        vertices = new Vector3[listPoint.Count];
        triangles = new int[listPoint.Count];
        for (int i = 0; i < listPoint.Count; i++)
        {
            vertices[i] = listPoint[i];
            //z固定为0
            vertices[i].z = 0f;

            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;



        Vector2[] uvs = new Vector2[vlen];
        for (int i = 0; i < vlen; i++)
        {
            //uvs [i] = new Vector2 (vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
            //(x+r)/2r  (y+r)/2r
            // uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].y / radius / 2 + 0.5f);
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        mesh.uv = uvs;
    }
}
