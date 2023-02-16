using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class meshGenerator : MonoBehaviour
{
    Mesh mesh;

    public List<Vector3> vertices;
    public int[] triangles;



    public float x;
    public float y;
    public float z;
    public float xi;
    public float yi;
    public float zi;
    public float Speed = .01f;
    public float yaw = 0;
    public float pitch = 0;
    public float roll = 0;

    // Start is called before the first frame update
    void Start()
    {
        //string file = System.IO.File.ReadAllText("Assets/Gnome.cstl");
        byte[] file = System.IO.File.ReadAllBytes("Assets/dragonbin.cstl");
        
        //print(cstlfile);
        //ParseFileASCII();
        ParseFileBin(file);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        UpdateMesh();

        x = Camera.main.transform.position.x;
        y = Camera.main.transform.position.y;
        z = Camera.main.transform.position.z;
        xi = Camera.main.transform.position.x;
        yi = Camera.main.transform.position.y;
        zi = Camera.main.transform.position.z;
        Speed = .01F;
    }

    void ParseFileASCII(string inputFile)
    {
        string cstlfile = inputFile;
        string[] splitfile1 = cstlfile.Split("\n");
        print(splitfile1.Length);
        string[,] splitfile2 = new string[4, 2];
        string[] xsplit = new string[0];
        string[] ysplit = new string[0];
        string[] zsplit = new string[0];
        string[] tessSplit = new string[0];

        List<double> xFinal = new List<double>();
        List<double> yFinal = new List<double>();
        List<double> zFinal = new List<double>();
        List<int> tessFinal = new List<int>();
        for (int i = 0; i < splitfile1.Length; i++)
        {
            string strs = splitfile1[i];
            splitfile2[i, 0] = strs.Split("  ")[0].Trim();
            splitfile2[i, 1] = strs.Split("  ")[1].Trim();
        }
        for (int i = 0; i < 4; i++)
        {
            if (splitfile2[i, 0] == "x_list")
            {
                xsplit = splitfile2[i, 1].Split(", ");
            }
            else if (splitfile2[i, 0] == "y_list")
            {
                ysplit = splitfile2[i, 1].Split(", ");
            }
            else if (splitfile2[i, 0] == "z_list")
            {
                zsplit = splitfile2[i, 1].Split(", ");
            }
            else if (splitfile2[i, 0] == "tess_list")
            {
                tessSplit = splitfile2[i, 1].Split(", ");
            }
        }
        print(splitfile2[3, 1]);
        for (int i = 0; i < xsplit.Length; i++)
        {
            string tempx = xsplit[i].Trim().Replace("]", "").Replace("[", "");
            string tempy = ysplit[i].Trim().Replace("]", "").Replace("[", "");
            string tempz = zsplit[i].Trim().Replace("]", "").Replace("[", "");


            xFinal.Add((System.Convert.ToDouble(tempx)));
            yFinal.Add(System.Convert.ToDouble(tempy));
            zFinal.Add(System.Convert.ToDouble(tempz));




        }

        for (int i = 0; i < tessSplit.Length; i++)
        {
            string tempTess = tessSplit[i].Trim().Replace("]", "").Replace("[", "");
            tessFinal.Add(System.Convert.ToInt32(tempTess));
        }

        print(xFinal.Count);
        for (int i = 0; i < xFinal.Count; i++)
        {
            vertices.Add(new Vector3((float)xFinal[i], (float)yFinal[i], (float)zFinal[i]));
        }


        triangles = tessFinal.ToArray();
    }

    void ParseFileBin(byte[] inputFile)
    {
        byte[] cstlfile = inputFile;
        int[] splits = SplitByteArray(cstlfile, new byte[] { 0xff, 0xff, 0xff, 0xff });
        byte[] xPoints = cstlfile[0..splits[0]];
        byte[] yPoints = cstlfile[(splits[0]+4)..splits[1]];
        byte[] zPoints = cstlfile[(splits[1]+4)..splits[2]];
        print(yPoints.Length);
        byte[] tess = cstlfile[(splits[2]+4)..(Buffer.ByteLength(cstlfile))];

        int points = Buffer.ByteLength(xPoints)/4;
        int tri = Buffer.ByteLength(tess)/4;
        float[] xFinal = new float[points];
        float[] yFinal = new float[points];
        float[] zFinal = new float[points];


        int[] tessFinal = new int[tri];
        
        for (int i = 0; i < points; i++)
        {

            int pos1 = 4 * i;
            
            xFinal[i] = BitConverter.ToSingle(xPoints, pos1);
            yFinal[i] = BitConverter.ToSingle(yPoints, pos1);
            zFinal[i] = BitConverter.ToSingle(zPoints, pos1);

        }

        for (int i = 0; i < tri; i++)
        {
            int pos1 = 4 * i;
            tessFinal[i] = BitConverter.ToInt32(tess, pos1);
        }
        print(tessFinal.Length);
        for (int i = 0; i < xFinal.Length; i++)
        {
            vertices.Add(new Vector3(xFinal[i], yFinal[i], zFinal[i]));
        }


        triangles = tessFinal;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }

    int[] SplitByteArray(byte[] array, byte[] delim)
    {
        List<int> splits = new List<int>();
        int delimLen = Buffer.ByteLength(delim);
        if (Buffer.ByteLength(array) == 0 || Buffer.ByteLength(delim) == 0)
        {
            return splits.ToArray();
        }
        
        for (int i = 0; i < Buffer.ByteLength(array) - delimLen; i++)
        {
            bool check = true;
            for (int j = 0; j < delimLen; j++)
            {
                if (array[i + j] != delim[j])
                {
                    check = false;
                }
            }
            if (check == true)
            {
                splits.Add(i);
            }
        }
        return splits.ToArray();
    }


    // Update is called once per frame
    void Update()
    {
    }
}
