using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using AssemblyCSharp;
using System.IO;


public static class MinimapExporter {

    public static void createMiniMap(string pathName)
    {
        if (string.IsNullOrEmpty(pathName))
            pathName = EditorUtility.SaveFolderPanel("Export to", "", "");

        if (!pathName.Contains("media"))
        {
            Debug.LogError("LA RUTA DE DESTINO TIENE QUE SER Exes/media");
            return;
        }

        GameObject terrain = GameObject.Find("GroundMesh");

        MeshCollider m = terrain.GetComponent<MeshCollider>();

        float terrainWidth = m.bounds.size.x;
        float terrainHeight = m.bounds.size.z;

        bool mapIsWide = terrainWidth > terrainHeight ? true : false;

        float fixedWidth = 512;
        float fixedHeight = 512;
        
        if(mapIsWide)
            fixedHeight = (terrainHeight * fixedWidth) / terrainWidth;
        else
            fixedWidth = (terrainWidth * fixedHeight) / terrainHeight;

        int resWidth = 512;
        int resHeight = 512;

        Texture2D minimap = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

        for (int i = 0; i < resWidth; ++i)
        {
            for (int j = 0; j < resHeight; ++j)
            {
                if (mapIsWide)
                {
                    if (j >= (int)((resHeight - fixedHeight) / 2) && j <= (int)((resHeight + fixedHeight) / 2))
                        minimap.SetPixel(i, j, new Color(0, 0.7f, 0));
                        
                    else
                        minimap.SetPixel(i, j, new Color(0, 0, 0));
                }
                else 
                {
                    if (i >= (int)((resWidth - fixedWidth) / 2) && i <= (int)((resWidth + fixedWidth) / 2))
                        minimap.SetPixel(i, j, new Color(0, 0.7f, 0));
                    else
                        minimap.SetPixel(i, j, new Color(0, 0, 0));
                }
            }
        }
        
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();        

        foreach (GameObject go in sceneObjects)
        {
            if (go.tag.Equals("Resource"))
            {
                Color color = new Color();

                switch (PrefabUtility.GetPrefabParent(go).name)
                {
                    case "Tree":
                        color = new Color(0,0.4f,0);
                        break;

                    case "Rock":
                        color = new Color(0.4f,0.4f,0.4f);
                        break;

                    case "Iron":
                        color = new Color(0.78f,0.78f,0.78f);
                        break;
                }

                BoxCollider b = go.GetComponent<BoxCollider>();
                Vector2 pixelCenter = new Vector2(-1 * (go.transform.position.x + b.center.x), -1 * (go.transform.position.z + b.center.z));
                pixelCenter.x = (pixelCenter.x * fixedWidth) / terrainWidth;
                pixelCenter.y = (pixelCenter.y * fixedHeight) / terrainHeight;
                pixelCenter.x = pixelCenter.x + resWidth / 2;
                pixelCenter.y = pixelCenter.y + resHeight / 2;

                Vector2 pixelSize = new Vector2(b.size.x, b.size.z);
                pixelSize.x = (pixelSize.x * fixedWidth) / terrainWidth;
                pixelSize.y = (pixelSize.y * fixedHeight) / terrainHeight;

                int startX = (int)(pixelCenter.x - pixelSize.x / 2);
                int startY = (int)(pixelCenter.y - pixelSize.y / 2);

                for(int i = startX; i< startX + (int)pixelSize.x; ++i)
                    for(int j = startY; j < startY + (int)pixelSize.y; ++j)
                        minimap.SetPixel(i, j, color);
            }
        }


        byte[] bytes = minimap.EncodeToPNG();
        var path = EditorApplication.currentScene.Split(char.Parse("/"));
        string sceneName = (string)path[path.Length - 1].Remove(path[path.Length - 1].IndexOf('.'));

        string filename = pathName + "/gui/imagesets/" + sceneName + "Minimap.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        Debug.Log(string.Format("Took screenshot to: {0}", filename));
                
    }

   
}
