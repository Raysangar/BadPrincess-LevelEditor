using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;



public class ExportToPhysXScript
{
    public static string MaterialToString(PhysicMaterial physicMat, long idMaterial)
    {
        if (!physicMat)
        {
            return "####Error#### PHYSIC MATERIAL NOT FOUND";
        }

        StringBuilder sb = new StringBuilder();

        string fCombine = "";
        string bCombine = "";

        switch (physicMat.frictionCombine.ToString())
        {
            case "Average":
                fCombine = "eAVERAGE";
                break;

            case "Minimum":
                fCombine = "eMIN";
                break;

            case "Multiply":
                fCombine = "eMULTIPLY";
                break;

            case "Maximum":
                fCombine = "eMAX";
                break;

            default:
                break;
        }

        switch (physicMat.bounceCombine.ToString())
        {
            case "Average":
                bCombine = "eAVERAGE";
                break;

            case "Minimum":
                bCombine = "eMIN";
                break;

            case "Multiply":
                bCombine = "eMULTIPLY";
                break;

            case "Maximum":
                bCombine = "eMAX";
                break;

            default:
                break;
        }

        sb.Append("\t").Append("<PxMaterial>").Append("\n");

        sb.Append("\t\t").Append("<Id>").Append(idMaterial).Append("</Id>").Append("\n");

        sb.Append("\t\t").Append("<DynamicFriction>");
        sb.Append(physicMat.dynamicFriction);
        sb.Append("</DynamicFriction>").Append("\n");

        sb.Append("\t\t").Append("<StaticFriction>");
        sb.Append(physicMat.staticFriction);
        sb.Append("</StaticFriction>").Append("\n");

        sb.Append("\t\t").Append("<Restitution>");
        sb.Append(physicMat.bounciness);
        sb.Append("</Restitution>").Append("\n");

        sb.Append("\t\t").Append("<FrictionCombineMode>");
        sb.Append(fCombine);
        sb.Append("</FrictionCombineMode>").Append("\n");

        sb.Append("\t\t").Append("<RestitutionCombineMode>");
        sb.Append(bCombine);
        sb.Append("</RestitutionCombineMode>").Append("\n");

        sb.Append("\t").Append("</PxMaterial>").Append("\n");

        return sb.ToString();
    }

    public static string MeshToString(Mesh m, long idTriangleMesh)
    {
        if (!m)
        {
            return "####Error#### MESH NOT FOUND";
        }

        StringBuilder sb = new StringBuilder();

        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\t").Append("<PxTriangleMesh>").Append("\n");
            sb.Append("\t\t").Append("<Id>").Append(idTriangleMesh).Append("</Id>").Append("\n");
            sb.Append("\t\t").Append("<Points >");
            int increment = 2;
            for (int i = 0; i < m.vertices.Length; i += increment)
            {
                if (i > 0)
                    sb.Append("\t\t\t");
               
                sb.Append(m.vertices[i].x + " " + m.vertices[i].y + " " + m.vertices[i].z);

                if (i + increment >= m.vertices.Length)
                    increment = m.vertices.Length - i;

                for (int j = 1; j < increment; ++j)
                    sb.Append(string.Format(" {0} {1} {2}", m.vertices[i + j].x, m.vertices[i + j].y, m.vertices[i + j].z));

                if (i < m.vertices.Length - increment)
                    sb.Append("\n");
            }
            sb.Append("</Points>").Append("\n");

            sb.Append("\t\t").Append("<Triangles >");

            increment = 6;
            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += increment)
            {
                if (i > 0)
                    sb.Append("\t\t\t");

                if (i + increment >= triangles.Length)
                    increment = triangles.Length - i;

                sb.Append(triangles[i]);

                for (int j = 1; j < increment; ++j)
                    sb.Append(" " + triangles[i + j]);

                if (i < triangles.Length - increment)
                    sb.Append("\n");
            }

            sb.Append("</Triangles>").Append("\n");
            sb.Append("\t").Append("</PxTriangleMesh>").Append("\n");
        }

        return sb.ToString();
    }

    public static string TransformToString(Transform t, long idRigidStatic, long idTriangleMesh, long idMaterial, Vector3 globalPosition)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("\t").Append("<PxRigidStatic>").Append("\n");
        sb.Append("\t\t").Append("<Id>").Append(idRigidStatic).Append("</Id>").Append("\n");
        sb.Append("\t\t").Append("<Name>").Append(t.name).Append("</Name>").Append("\n");
        sb.Append("\t\t").Append("<ActorFlags>").Append("eVISUALIZATION").Append("</ActorFlags>").Append("\n");
        sb.Append("\t\t").Append("<DominanceGroup>").Append(0).Append("</DominanceGroup>").Append("\n");
        sb.Append("\t\t").Append("<OwnerClient>").Append(0).Append("</OwnerClient>").Append("\n");
        sb.Append("\t\t").Append("<ClientBehaviorBits>").Append(0).Append("</ClientBehaviorBits>").Append("\n");

        sb.Append("\t\t").Append("<GlobalPose>");

        sb.Append(string.Format("{0} {1} {2} {3} {4} {5} {6}",
            t.rotation.x, -t.rotation.y, -t.rotation.z, t.rotation.w, -globalPosition.x, globalPosition.y, globalPosition.z));

        sb.Append("</GlobalPose>").Append("\n");

        sb.Append("\t\t").Append("<Shapes>").Append("\n");
        sb.Append("\t\t\t").Append("<PxShape>").Append("\n");
        sb.Append("\t\t\t\t").Append("<Geometry>").Append("\n");
        sb.Append("\t\t\t\t\t").Append("<PxTriangleMeshGeometry>").Append("\n");
        sb.Append("\t\t\t\t\t\t").Append("<Scale>").Append("\n");
        sb.Append("\t\t\t\t\t\t\t").Append("<Scale>");
        sb.Append(string.Format("{0} {1} {2}", t.localScale.x, t.localScale.y, t.localScale.z));
        sb.Append("</Scale>").Append("\n");

        sb.Append("\t\t\t\t\t\t\t").Append("<Rotation>");
        sb.Append(string.Format("{0} {1} {2} {3}", 0, 0, 0, 1));
        sb.Append("</Rotation>").Append("\n");
        sb.Append("\t\t\t\t\t\t").Append("</Scale>").Append("\n");
        sb.Append("\t\t\t\t\t\t").Append("<TriangleMesh>").Append(idTriangleMesh).Append("</TriangleMesh>").Append("\n");
        sb.Append("\t\t\t\t\t").Append("</PxTriangleMeshGeometry>").Append("\n");
        sb.Append("\t\t\t\t").Append("</Geometry>").Append("\n");
        sb.Append("\t\t\t\t").Append("<LocalPose>");
        sb.Append(string.Format("{0} {1} {2} {3} {4} {5} {6}", 0, 0, 0, 1, 0, 0, 0));
        sb.Append("</LocalPose>").Append("\n");
        sb.Append("\t\t\t\t").Append("<SimulationFilterData>");
        sb.Append(string.Format("{0} {1} {2} {3}", 8, 0, 8, 0));
        sb.Append("</SimulationFilterData>").Append("\n");
        sb.Append("\t\t\t\t").Append("<QueryFilterData>");
        sb.Append(string.Format("{0} {1} {2} {3}", 0, 0, 0, 0));
        sb.Append("</QueryFilterData>").Append("\n");

        sb.Append("\t\t\t\t").Append("<Materials>").Append("\n");
        sb.Append("\t\t\t\t\t").Append("<PxMaterialRef>").Append(idMaterial).Append("</PxMaterialRef>").Append("\n");
        sb.Append("\t\t\t\t").Append("</Materials>").Append("\n");
        sb.Append("\t\t\t\t").Append("<ContactOffset>").Append(1).Append("</ContactOffset>").Append("\n");
        sb.Append("\t\t\t\t").Append("<RestOffset>").Append(0).Append("</RestOffset>").Append("\n");
        sb.Append("\t\t\t\t").Append("<Flags>").Append("eSIMULATION_SHAPE|eSCENE_QUERY_SHAPE|eVISUALIZATION").Append("</Flags>").Append("\n");
        //sb.Append("\t\t\t\t").Append("<Flags>").Append("eVISUALIZATION").Append("</Flags>").Append("\n");
        sb.Append("\t\t\t\t").Append("<Name>").Append(t.name).Append("</Name>").Append("\n");
        sb.Append("\t\t\t").Append("</PxShape>").Append("\n");
        sb.Append("\t\t").Append("</Shapes>").Append("\n");
        sb.Append("\t").Append("</PxRigidStatic>").Append("\n");

        return sb.ToString();
    }

    public static Mesh createMesh(TerrainData t, Vector3 terrainPosition)
    {
        Mesh m = new Mesh();

        int w = t.heightmapWidth;
        int h = t.heightmapHeight;
        Vector3 meshScale = t.size;
        int tRes = 2;
        meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);

        float[,] tData = t.GetHeights(0, 0, w, h);

        w = (w - 1) / tRes + 1;
        h = (h - 1) / tRes + 1;

        Vector3[] tVertices = new Vector3[w * h];

        int[] tPolys;

        tPolys = new int[(w - 1) * (h - 1) * 6];

        // Build vertices
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(-y, tData[x * tRes, y * tRes], x)) + terrainPosition;
            }
        }

        int index = 0;

        // Build triangle indices: 3 indices into vertex array for each triangle
        for (int y = 0; y < h - 1; y++)
        {
            for (int x = 0; x < w - 1; x++)
            {
                // For each grid cell output two triangles
                tPolys[index++] = (y * w) + x;
                tPolys[index++] = ((y + 1) * w) + x;
                tPolys[index++] = (y * w) + x + 1;

                tPolys[index++] = ((y + 1) * w) + x;
                tPolys[index++] = ((y + 1) * w) + x + 1;
                tPolys[index++] = (y * w) + x + 1;
            }
        }

        m.vertices = tVertices;
        m.triangles = tPolys;
        m.subMeshCount = 1;

        return m;
    }
}

public class ObjExporter : ScriptableObject
{

    private static Dictionary<int, long> materialIds;
    private static Dictionary<int, int> sharedMaterialIds;
    private static Dictionary<int, long> triangleMeshIds;
    private static Dictionary<int, long> rigidStaticIds;

    private static long idGenerator()
    {
        return (Random.Range(1, 10000) * Random.Range(1, 10000) * Random.Range(1, 10)) % 1000000000 + 1000000000;
    }

    public static void DoExport(GameObject selected, string pathName)
    {
        string meshName = selected.name;
        string fileName = pathName + "/models/" + meshName + ".repx";
                
        StringBuilder meshString = new StringBuilder();

        meshString.Append("<PhysX30Collection version='3.2.0'>").Append("\n");

        Transform t = selected.transform;
        Vector3 originalPosition = t.position;
        //t.position = Vector3.zero;        

        meshString.Append("\t").Append("<UpVector>");
        meshString.Append(string.Format("{0} {1} {2}", 0, 1, 0));
        meshString.Append("</UpVector>").Append("\n");
        meshString.Append("\t").Append("<Scale>").Append("\n");
        meshString.Append("\t\t").Append("<Length>").Append(1).Append("</Length>").Append("\n");
        meshString.Append("\t\t").Append("<Mass>").Append(1000).Append("</Mass>").Append("\n");
        meshString.Append("\t\t").Append("<Speed>").Append(10).Append("</Speed>").Append("\n");
        meshString.Append("\t").Append("</Scale>").Append("\n");

        materialIds = new Dictionary<int, long>();
        triangleMeshIds = new Dictionary<int, long>();
        rigidStaticIds = new Dictionary<int, long>();
        sharedMaterialIds = new Dictionary<int, int>();

        createDictionaries(t, ref sharedMaterialIds, ref materialIds, ref triangleMeshIds, ref rigidStaticIds);

        meshString.Append(processMaterial(t));
        meshString.Append(processMesh(t));
        meshString.Append(processTransform(t, Vector3.zero));

        meshString.Append("</PhysX30Collection>");

        WriteToFile(meshString.ToString(), fileName);

        t.position = originalPosition;

        Debug.Log("Exported Mesh: " + fileName);
    }

    static void createDictionaries(Transform t, ref Dictionary<int, int> meshesMaterialIds, ref Dictionary<int, long> materialIds, ref Dictionary<int, long> triangleMeshIds, ref Dictionary<int, long> rigidStaticIds)
    {
        Terrain terrain = t.gameObject.GetComponent<Terrain>();
        if (!terrain)
        {
            MeshFilter mf = t.GetComponent<MeshFilter>();

            if (mf)
            {
                meshesMaterialIds.Add(t.GetInstanceID(), mf.collider.sharedMaterial.GetInstanceID());
                triangleMeshIds.Add(t.GetInstanceID(), idGenerator());
                rigidStaticIds.Add(t.GetInstanceID(), idGenerator());
            }

            for (int i = 0; i < t.childCount; i++)
            {
                createDictionaries(t.GetChild(i), ref meshesMaterialIds, ref materialIds, ref triangleMeshIds, ref rigidStaticIds);
            }
        }
        else
        {
            meshesMaterialIds.Add(t.GetInstanceID(), terrain.terrainData.physicMaterial.GetInstanceID());
            triangleMeshIds.Add(t.GetInstanceID(), idGenerator());
            rigidStaticIds.Add(t.GetInstanceID(), idGenerator());
        }
    }

    static string processMaterial(Transform t)
    {
        StringBuilder meshString = new StringBuilder();
        Terrain terrain = t.gameObject.GetComponent<Terrain>();

        if (!terrain)
        {
            MeshFilter mf = t.GetComponent<MeshFilter>();

            if (mf)
            {
                if (!materialIds.ContainsKey(mf.collider.sharedMaterial.GetInstanceID()))
                {
                    long idMaterial = idGenerator();
                    materialIds.Add(mf.collider.sharedMaterial.GetInstanceID(), idMaterial);
                    meshString.Append(ExportToPhysXScript.MaterialToString(mf.collider.sharedMaterial, idMaterial));
                }
            }

            for (int i = 0; i < t.childCount; i++)
            {
                meshString.Append(processMaterial(t.GetChild(i)));
            }
        }
        else
        {
            if (!materialIds.ContainsKey(terrain.terrainData.physicMaterial.GetInstanceID()))
            {
                long idMaterial = idGenerator();
                materialIds.Add(terrain.terrainData.physicMaterial.GetInstanceID(), idMaterial);
                meshString.Append(ExportToPhysXScript.MaterialToString(terrain.terrainData.physicMaterial, idMaterial));
            }
        }

        return meshString.ToString();
    }

    static string processMesh(Transform t)
    {
        StringBuilder meshString = new StringBuilder();
        Terrain terrain = t.gameObject.GetComponent<Terrain>();

        if (!terrain)
        {
            MeshFilter mf = t.GetComponent<MeshFilter>();

            if (mf)
            {
                long idTriangleMesh;
                triangleMeshIds.TryGetValue(t.GetInstanceID(), out idTriangleMesh);
                meshString.Append(ExportToPhysXScript.MeshToString(mf.sharedMesh, idTriangleMesh));
            }

            for (int i = 0; i < t.childCount; i++)
            {
                meshString.Append(processMesh(t.GetChild(i)));
            }
        }
        else
        {
            long idTriangleMesh;
            triangleMeshIds.TryGetValue(t.GetInstanceID(), out idTriangleMesh);
            Mesh m = ExportToPhysXScript.createMesh(terrain.terrainData, t.position);
            meshString.Append(ExportToPhysXScript.MeshToString(m, idTriangleMesh));
        }

        return meshString.ToString();
    }

    static string processTransform(Transform t, Vector3 parentGlobalPosition)
    {
        StringBuilder meshString = new StringBuilder();
        Vector3 globalPosition = t.localPosition + parentGlobalPosition;
        Terrain terrain = t.gameObject.GetComponent<Terrain>();
        if (!terrain)
        {
            MeshFilter mf = t.GetComponent<MeshFilter>();

            if (mf)
            {
                int key;
                long idRigidStatic, idTriangleMesh, idMaterial;
                sharedMaterialIds.TryGetValue(t.GetInstanceID(), out key);
                materialIds.TryGetValue(key, out idMaterial);
                triangleMeshIds.TryGetValue(t.GetInstanceID(), out idTriangleMesh);
                rigidStaticIds.TryGetValue(t.GetInstanceID(), out idRigidStatic);
                meshString.Append(ExportToPhysXScript.TransformToString(t, idRigidStatic, idTriangleMesh, idMaterial, globalPosition));
            }

            for (int i = 0; i < t.childCount; i++)
            {
                meshString.Append(processTransform(t.GetChild(i), globalPosition));
            }
        }
        else
        {
            int key;
            long idRigidStatic, idTriangleMesh, idMaterial;
            sharedMaterialIds.TryGetValue(t.GetInstanceID(), out key);
            materialIds.TryGetValue(key, out idMaterial);
            triangleMeshIds.TryGetValue(t.GetInstanceID(), out idTriangleMesh);
            rigidStaticIds.TryGetValue(t.GetInstanceID(), out idRigidStatic);
            meshString.Append(ExportToPhysXScript.TransformToString(t, idRigidStatic, idTriangleMesh, idMaterial, globalPosition));
        }

        return meshString.ToString();
    }

    static void WriteToFile(string s, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(s);
        }
    }
}