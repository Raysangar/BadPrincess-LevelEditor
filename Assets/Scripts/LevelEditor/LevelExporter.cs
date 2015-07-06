using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using AssemblyCSharp;
using System.IO;

public class ExportLevel 
{
    private static string prefabsDirectoryPath = "./Assets/Resources/Prefabs";

    [MenuItem("File/Export Map and Blueprints")]
    static void ExportMaps()
    {
        string pathName = "";
        createMap(ref pathName);
        createBlueprints(pathName);
        Debug.Log("Exportation finished.");
        
    }

    private static void createMap(ref string pathName)
    {
        pathName = EditorUtility.SaveFolderPanel("Export to", "", "");
        if (!pathName.Contains("media"))
        {
            Debug.LogError("LA RUTA DE DESTINO TIENE QUE SER Exes/media");
            return;
        }

        var path = EditorApplication.currentScene.Split(char.Parse("/"));
        string sceneName = (string)path[path.Length - 1].Remove(path[path.Length - 1].IndexOf('.'));

        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        ArrayList entitiesInfo = new ArrayList();

        List<string> forbiddenTags = new List<string>();
        forbiddenTags.Add("SmartPosition");
        forbiddenTags.Add("EditorOnly");
        forbiddenTags.Add("MainCamera");
        forbiddenTags.Add("Obstacle");

        foreach (GameObject go in sceneObjects)
        {
            if (go.activeInHierarchy && go.transform.parent == null  && !forbiddenTags.Contains(go.tag) )
            {
                string entityType = (PrefabUtility.GetPrefabParent(go)) ?
                    PrefabUtility.GetPrefabParent(go).name : go.name;
                ArrayList componentsInfo = new ArrayList();
                bool isPlayer = (go.tag.CompareTo("Player") == 0);
                bool isEnemy = (go.tag.CompareTo("Enemy") == 0);
                if (go.tag == "World")
                {
                    componentsInfo.Add(new TransformAttributesGetter().getAttributesFrom(go.transform));
                    if(go.GetComponent<CGraphics>() != null)
                        componentsInfo.Add(new ScriptAttributesGetter().getAttributesFrom(go.GetComponent<CGraphics>()));
                    
                    Dictionary<string, object> physicInfo = new Dictionary<string, object>();
                    physicInfo.Add("physic_entity", "fromFile");
                    physicInfo.Add("physic_file", "media/models/" + go.name + ".RepX");
                    physicInfo.Add("physic_group", go.layer);
                    physicInfo.Add("physic_normal", new Vector3(0, 1, 0));
                    if (go.GetComponent<CWorldComponent>())
                    {                       
                        componentsInfo.Add(new ScriptAttributesGetter().getAttributesFrom(go.GetComponent<CWorldComponent>()));
                        if(go.GetComponentInChildren<MeshCollider>())
                            physicInfo.Add("physic_bounds", go.GetComponentInChildren<MeshCollider>().bounds.size);
                    }
                    if (go.layer == 16)
                    {
                        componentsInfo.Add(new ScriptAttributesGetter().getAttributesFrom(go.GetComponent<CRemovableWorldComponent>()));
                    }
                   
                    componentsInfo.Add(physicInfo);
                   
                }
                else if (go.tag == "ResourceGroup")
                {
                    var resources = go.GetComponentsInChildren<Transform>();
                    int resIndex = 0;
                    foreach (Transform res in resources)
                    {
                        if (res.parent == go.transform && res != go.transform)
                        {
                            entityType = (PrefabUtility.GetPrefabParent(res)) ?
                                PrefabUtility.GetPrefabParent(res).name : res.name;
                            componentsInfo = new ArrayList();
                            isPlayer = (res.tag.CompareTo("Player") == 0);
                            isEnemy = (res.tag.CompareTo("Enemy") == 0);

                            foreach (Component component in res.GetComponents(typeof(Component)))
                            {
                                if (ComponentAttributesGettersDictionary.getAttributesGetterFor(component.GetType()) != null)
                                    componentsInfo.Add(ComponentAttributesGettersDictionary.getAttributesGetterFor(component.GetType()).getAttributesFrom(component));
                            }
                            if (res.transform.FindChild("smart_position_1"))
                            {
                                Dictionary<string, object> smartPointPosition = new Dictionary<string, object>();
                                GameObject[] smartPoints = GameObject.FindGameObjectsWithTag("SmartPosition");
                                int index = 0;
                                foreach (GameObject sp in smartPoints)
                                {
                                    if (sp.transform.parent == res.transform)
                                    {
                                        ++index;
                                        Vector3 locPos = sp.transform.localPosition;
                                        locPos.x *= -1;
                                        smartPointPosition.Add(sp.name, locPos);
                                    }
                                }
                                smartPointPosition.Add("smart_positions_num", index);
                                componentsInfo.Add(smartPointPosition);
                            }
                            entitiesInfo.Add(new EntityInfo(go.name + res.name + resIndex, entityType, isPlayer, isEnemy, componentsInfo, false));
                            ++resIndex;
                        }
                    }
                }
                else
                {
                    foreach (Component component in go.GetComponents(typeof(Component)))
                    {
                        if (ComponentAttributesGettersDictionary.getAttributesGetterFor(component.GetType()) != null)
                            componentsInfo.Add(ComponentAttributesGettersDictionary.getAttributesGetterFor(component.GetType()).getAttributesFrom(component));
                    }
                    if (go.transform.FindChild("smart_position_1"))
                    {
                        Dictionary<string, object> smartPointPosition = new Dictionary<string, object>();
                        GameObject[] smartPoints = GameObject.FindGameObjectsWithTag("SmartPosition");
                        int index = 0;
                        foreach (GameObject sp in smartPoints)
                        {
                            if (sp.transform.parent == go.transform)
                            {
                                ++index;
                                Vector3 locPos = sp.transform.localPosition;
                                locPos.x *= -1;
                                smartPointPosition.Add(sp.name, locPos);                                
                            }
                        }
                        smartPointPosition.Add("smart_positions_num", index);
                        componentsInfo.Add(smartPointPosition);
                    }

                }
                if (go.tag != "ResourceGroup")
                {
                    bool isGenerator = (go.transform.position.y < -50);
                    entitiesInfo.Add(new EntityInfo(go.name, entityType, isPlayer, isEnemy, componentsInfo, isGenerator));
                }
            }
        }
        entitiesInfo.Add(WayPointsExporter.exportWayPoints());

        System.IO.File.WriteAllText(pathName + "/maps/" + sceneName + "Map.txt", MapCreator.createMap(entitiesInfo));
    }

    private static void createBlueprints(string pathName)
    {
        if (!pathName.Contains("media"))
        {            
            return;
        }

        List<string> typeRead = new List<string>();
        
        var path = EditorApplication.currentScene.Split(char.Parse("/"));
        string sceneName = (string)path[path.Length - 1].Remove(path[path.Length - 1].IndexOf('.'));

        DirectoryInfo directoryInfo = new DirectoryInfo(prefabsDirectoryPath);
        DirectoryInfo[] directories = directoryInfo.GetDirectories();

        GameObject go;
        string blueprintsText = "";

        List<string> forbiddenTags = new List<string>();
        forbiddenTags.Add("SmartPosition");
        forbiddenTags.Add("EditorOnly");
        forbiddenTags.Add("MainCamera");
        forbiddenTags.Add("Obstacle");

        foreach (DirectoryInfo d in directories)
        {
            FileInfo[] directoryFiles = d.GetFiles("*.prefab");
            foreach (FileInfo file in directoryFiles)
            {                                
                go = (GameObject)Resources.Load("Prefabs/" + d.Name + "/" + Path.GetFileNameWithoutExtension(file.FullName), typeof(GameObject));
                GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject gameObj in sceneObjects)
                {
                    if (gameObj.tag == "ResourceGroup")
                    {
                        
                        var resources = gameObj.GetComponentsInChildren<Transform>();

                        if (resources[1].parent == gameObj.transform)
                        {
                            if (PrefabUtility.GetPrefabParent(resources[1].gameObject).name == Path.GetFileNameWithoutExtension(file.FullName) && !typeRead.Contains(PrefabUtility.GetPrefabParent(resources[1].gameObject).name))
                            {
                                typeRead.Add(Path.GetFileNameWithoutExtension(file.FullName));
                                blueprintsText += "\n" + Path.GetFileNameWithoutExtension(file.FullName);
                                Component[] components = go.GetComponents<Component>();
                                foreach (Component component in components)
                                    blueprintsText += " " + ComponentTranslator.translate(component.GetType());
                                break;
                            }
                        }
                        
                    }
                    else if (gameObj.activeInHierarchy && gameObj.transform.parent == null && !forbiddenTags.Contains(gameObj.tag))
                    {
                        if (PrefabUtility.GetPrefabParent(gameObj).name == Path.GetFileNameWithoutExtension(file.FullName))
                        {
                            blueprintsText += "\n" + Path.GetFileNameWithoutExtension(file.FullName);
                            Component[] components = go.GetComponents<Component>();
                            foreach (Component component in components)
                                blueprintsText += " " + ComponentTranslator.translate(component.GetType());
                            break;
                        }
                    }

                }
            }
        }

        blueprintsText = blueprintsText.TrimStart('\n');
        System.IO.File.WriteAllText(pathName + "/maps/" + sceneName + "Blueprints.txt", StringFormatter.reduceWhitespaces(blueprintsText));
    }
 


}
