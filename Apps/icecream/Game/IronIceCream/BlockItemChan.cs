using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class BlockItemChan : UIView
{
    private Mesh mesh;
    MeshRenderer meshRender;
    Material meshMat;
    private Vector3[] vertices;
    private int[] triangles;
    public List<Vector3> listPoint;
    public float width = 2f;
    public float height = 2f;
    BoxCollider boxCollider;

    public int indexRow = 0;
    public int indexCol = 0;
    public int row;
    public int col;
    public Vector3 posCenter = Vector3.zero;
    public int percent = 100;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listPoint = new List<Vector3>();
        mesh = GetComponent<MeshFilter>().mesh;
        meshRender = GetComponent<MeshRenderer>();

        string strshader = "Custom/MeshTexture";
        meshMat = new Material(Shader.Find(strshader));
        meshRender.material = meshMat;
        AddPoint(Vector3.zero);
        // boxCollider = this.gameObject.AddComponent<BoxCollider>();

        //indexCol = 0;
        //indexRow = 0;

        //Debug.Log("col =" + col + " indexCol=" + indexCol);
    }

    // Use this for initialization
    void Start()
    {

        // Draw();
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

    Vector3[] GetverticeOfPoint(Vector2 pt)
    {
        int count = 4;
        float z = 0f;
        Vector3[] v = new Vector3[count];

        float y_bottom = (pt.y - height / 2) + height * (100 - percent) / 100f;
        //left_bottom
        v[0] = new Vector3(pt.x - width / 2, y_bottom, z);

        //right_bottom
        v[1] = new Vector3(pt.x + width / 2, y_bottom, z);


        float y_top = (pt.y + height / 2);
        //top_left
        //v[2] = new Vector3(pt.x - width / 2, pt.y + height / 2, z);
        v[2] = new Vector3(pt.x - width / 2, y_top, z);

        //top_right
        v[3] = new Vector3(pt.x + width / 2, y_top, z);


        return v;
    }

    public void UpdateMaterial(Material mat)
    {
        if (meshRender != null)
        {
            meshRender.material = mat;
        }
    }
    public void UpdateSize(float w, float h)
    {
        width = w;
        height = h;
        if (boxCollider != null)
        {
            boxCollider.size = new Vector2(w, h);
        }
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2(w, h);
        Draw();
    }
    public void Draw()
    {
        if ((row == 0) || (col == 0))
        {
            return;
        }
        //sideWidth = 0;
        mesh.Clear();
        int count = 1;
        vertices = new Vector3[count * 4];
        triangles = new int[count * 6];
        Vector2[] uvs = new Vector2[count * 4];
        int tri_index = 0;
        int i = 0;

      //  Debug.Log("Draw col =" + col + " indexCol=" + indexCol);
        float x, y;
        {
            Vector3[] v = GetverticeOfPoint(posCenter);

            for (int j = 0; j < 4; j++)
            {
                vertices[i * 4 + j] = v[j];
            }


            //纹理坐标
            {
                float tex_w = 1.0f / col;
                float tex_h = 1.0f / row;

                float y_bottom = (indexRow * tex_h) + tex_h * (100 - percent) / 100f;
                //left_bottom 
                x = indexCol * tex_w;
                y = y_bottom;
                uvs[i * 4 + 0] = new Vector2(x, y);//0f, 0f

                //right_bottom
                x = (indexCol + 1) * tex_w;
                y = y_bottom;
                uvs[i * 4 + 1] = new Vector2(x, y);//(1f, 0f

                //top_left
                x = indexCol * tex_w;
                y = (indexRow + 1) * tex_h;
                uvs[i * 4 + 2] = new Vector2(x, y);//(0f, 1f

                //top_right
                x = (indexCol + 1) * tex_w;
                y = (indexRow + 1) * tex_h;
                uvs[i * 4 + 3] = new Vector2(x, y);//1f, 1f

                // meshMat.SetFloat("uvCenterX", indexCol * tex_w + tex_w / 2);
                //  meshMat.SetFloat("uvCenterY", indexRow * tex_h + tex_h / 2);
            }

            int idx = 0;
            //三角型1
            {
                //top_left
                idx = i * 6 + 0;
                triangles[idx] = tri_index + 2;
                //right_bottom
                idx = i * 6 + 1;
                triangles[idx] = tri_index + 1;
                //left_bottom
                idx = i * 6 + 2;
                triangles[idx] = tri_index + 0;
            }

            //三角型2
            {
                //top_left
                idx = i * 6 + 3;
                triangles[idx] = tri_index + 2;
                //top_right
                idx = i * 6 + 4;
                triangles[idx] = tri_index + 3;
                //bottom_right
                idx = i * 6 + 5;
                triangles[idx] = tri_index + 1;
            }



            tri_index += 4;
        }



        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    public void UpdateTexture(Texture2D tex)
    {
        meshMat.SetTexture("_MainTex", tex);
        // meshMat.SetInt("indexRow", indexRow);
        // meshMat.SetInt("indexCol", indexCol);
        // meshMat.SetInt("row", row);
        // meshMat.SetInt("col", col);

        meshRender.material = meshMat;
        Draw();
    }

    public void UpdatePercent(int value)
    {
        if (value > percent)
        {
            //单向操作 只能越来越小
            return;
        }
        percent = value;
        Draw();
    }

}
