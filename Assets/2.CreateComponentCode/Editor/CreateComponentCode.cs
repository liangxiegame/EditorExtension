using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EditorExtension
{
    public class CreateComponentCode : EditorWindow
    {
        [MenuItem("编辑器扩展/2.NamespaceSetting")]
        static void Open()
        {
            var window = GetWindow<CreateComponentCode>();
            window.Show();
        }
        
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Namespace:");
            NamespaceSettingsData.Namespace = GUILayout.TextField(NamespaceSettingsData.Namespace);
            GUILayout.EndHorizontal();
        }

        private static List<BindInfo> mBindInfos = new List<BindInfo>();

        [MenuItem("GameObject/@EditorExtension-Bind", false, 0)]
        static void Bind()
        {
            var gameObject = Selection.objects.First() as GameObject;

            if (!gameObject)
            {
                Debug.LogWarning("需要选择 GameObject");
                return;
            }

            var view = gameObject.GetComponent<Bind>();

            if (!view)
            {
                gameObject.AddComponent<Bind>();
            }
        }

        
        [MenuItem("GameObject/@EditorExtension-Add Code Generate Info", false, 0)]
        static void AddView()
        {
            var gameObject = Selection.objects.First() as GameObject;

            if (!gameObject)
            {
                Debug.LogWarning("需要选择 GameObject");
                return;
            }

            var view = gameObject.GetComponent<CodeGenerateInfo>();

            if (!view)
            {
                gameObject.AddComponent<CodeGenerateInfo>();
            }
        }
        
        [MenuItem("GameObject/@EditorExtension-Create Code", false, 0)]
        static void CreateCode()
        {
            var gameObject = Selection.objects.First() as GameObject;

            if (!gameObject)
            {
                Debug.LogWarning("需要选择 GameObject");
                return;
            }

            Debug.Log("Create Code");

            var generateInfo = gameObject.GetComponent<CodeGenerateInfo>();

            var scriptsFolder = Application.dataPath + "/Scripts";

            if (generateInfo)
            {
                scriptsFolder = generateInfo.ScriptsFolder;
            }

            if (!Directory.Exists(scriptsFolder))
            {
                Directory.CreateDirectory(scriptsFolder);
            }


            mBindInfos.Clear();

            // 搜索所有绑定
            SearchBinds("", gameObject.transform, mBindInfos);

            ComponentTemplate.Write(gameObject.name, scriptsFolder);
            ComponentDesignerTemplate.Write(gameObject.name, scriptsFolder, mBindInfos);
            
            EditorPrefs.SetString("GENERATE_CLASS_NAME", gameObject.name);
            AssetDatabase.Refresh();
        }



        static void SearchBinds(string path, Transform transform, List<BindInfo> binds)
        {
            var bind = transform.GetComponent<Bind>();

            var isRoot = string.IsNullOrWhiteSpace(path);

            if (bind && !isRoot)
            {
                binds.Add(new BindInfo()
                {
                    FindPath = path,
                    Name = transform.name,
                    ComponentName = bind.ComponentName
                });
            }


            foreach (Transform childTrans in transform)
            {
                SearchBinds(isRoot ? childTrans.name : path + "/" + childTrans.name, childTrans, binds);
            }
        }

        [DidReloadScripts]
        static void AddComponent2GameObject()
        {
            Debug.Log("DidReloadScripts");
            var generateClassName = EditorPrefs.GetString("GENERATE_CLASS_NAME");
            Debug.Log(generateClassName);

            if (string.IsNullOrWhiteSpace(generateClassName))
            {
                Debug.Log("不继续操作");
                EditorPrefs.DeleteKey("GENERATE_CLASS_NAME");
            }
            else
            {
                Debug.Log("继续操作");

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                var defaultAssembly = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");

                var typeName = NamespaceSettingsData.Namespace + "." + generateClassName;
                
                var type = defaultAssembly.GetType(typeName);

                if (type == null)
                {
                    Debug.Log("编译失败");
                    return;
                }

                Debug.Log(type);

                var gameObject = GameObject.Find(generateClassName);

                var scriptComponent = gameObject.GetComponent(type);

                if (!scriptComponent)
                {
                    scriptComponent = gameObject.AddComponent(type);
                }

                var serialiedScript = new SerializedObject(scriptComponent);

                mBindInfos.Clear();

                // 搜索所有绑定
                SearchBinds("", gameObject.transform, mBindInfos);

                foreach (var bindInfo in mBindInfos)
                {
                    var name = bindInfo.Name;

                    Debug.Log(bindInfo.FindPath);
                    Debug.Log(name);
                    Debug.Log(serialiedScript.FindProperty(name));
                    Debug.Log(gameObject.transform.Find(bindInfo.FindPath));

                    serialiedScript.FindProperty(name).objectReferenceValue =
                        gameObject.transform.Find(bindInfo.FindPath).GetComponent(bindInfo.ComponentName);
                }


                var codeGenerateInfo = gameObject.GetComponent<CodeGenerateInfo>();

                if (codeGenerateInfo)
                {
                    serialiedScript.FindProperty("ScriptsFolder").stringValue = codeGenerateInfo.ScriptsFolder;
                    serialiedScript.FindProperty("PrefabFolder").stringValue = codeGenerateInfo.PrefabFolder;
                    serialiedScript.FindProperty("GeneratePrefab").boolValue = codeGenerateInfo.GeneratePrefab;

                    var generatePrefab = codeGenerateInfo.GeneratePrefab;
                    var prefabFolder = codeGenerateInfo.PrefabFolder;
                    var fullPrefabFolder = prefabFolder.Replace("Assets",Application.dataPath);

                    if (codeGenerateInfo.GetType() == type)
                    {
                        
                    }
                    else
                    {
                        DestroyImmediate(codeGenerateInfo, false);
                    }

                    serialiedScript.ApplyModifiedPropertiesWithoutUndo();

                    if (generatePrefab)
                    {
                        
                        
                        
                        
                        if (!Directory.Exists(fullPrefabFolder))
                        {
                            Directory.CreateDirectory(fullPrefabFolder);
                        }

                        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, fullPrefabFolder + "/" + gameObject.name + ".prefab",
                            InteractionMode.AutomatedAction);
                    }
                }
                else
                {
                    serialiedScript.FindProperty("ScriptsFolder").stringValue = "Assets/Scripts";
                    serialiedScript.ApplyModifiedPropertiesWithoutUndo();
                }
                
                EditorPrefs.DeleteKey("GENERATE_CLASS_NAME");
            }
        }
    }
}