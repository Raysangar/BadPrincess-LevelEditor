using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class WayPointsExporter{

    public static AssemblyCSharp.EntityInfo exportWayPoints()
    {
        ArrayList componentsInfo = new ArrayList();
        Dictionary<string, object> wayPointsList = new Dictionary<string, object>();
        int i = 0;
        string wayPointName = "WayPoint";
        Vector3[] vectors = new Vector3[NavMesh.CalculateTriangulation().vertices.Length];



        foreach (Vector3 v in NavMesh.CalculateTriangulation().vertices)
        {
            if (i > 0)
            {
                bool repeated = false;
                for (int j = 0; j < i; j++)
                {
                    if (v == vectors[j])
                    {
                        repeated = true;
                        break;
                    }
                }
                if (!repeated && v.y < 10 && !isInEdges(v))
                {
                    Vector3 v2 = v;
                    v2.x *= -1;
                    v2.y += 1;
                    wayPointsList.Add(wayPointName + i, v2);
                    vectors[i] = v;
                    i++;
                }
            }
            else
            {               
                Vector3 v2 = v;
                v2.x *= -1;
                v2.y += 1;
                wayPointsList.Add(wayPointName + i, v2);
                vectors[i] = v;
                i++;
            }
        }
        wayPointsList.Add("WayPointsNum", i);
        componentsInfo.Add(wayPointsList);

        return new AssemblyCSharp.EntityInfo("WayPointList", "WayPoint", false, false, componentsInfo, false);
    }

    private static bool isInEdges(Vector3 point)
    {
        if (GameObject.Find("Camera"))
        {
            float topLimit = GameObject.Find("Camera").GetComponent<CCamera>().topLimit;
            float bottomLimit = GameObject.Find("Camera").GetComponent<CCamera>().bottomLimit;
            float rightLimit = GameObject.Find("Camera").GetComponent<CCamera>().rightLimit;
            float leftLimit = GameObject.Find("Camera").GetComponent<CCamera>().leftLimit;

            Vector3 bounds = GameObject.Find("Ground").GetComponentInChildren<MeshCollider>().bounds.size;
            if ((point.x < -bounds.x / 2 + (bounds.x * rightLimit)) || (point.x > bounds.x / 2 - (bounds.x * leftLimit)) ||
                (point.z > bounds.z / 2 - (bounds.z * bottomLimit)) || (point.z < -bounds.z / 2 + (bounds.z * topLimit)))
                return true;

        }

        return false;
    }
    
}
