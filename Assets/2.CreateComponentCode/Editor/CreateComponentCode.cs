using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EditorExtension
{
    public static class CreateComponentCode
    {
        private static List<BindInfo> mBindInfos = new List<BindInfo>();

        [MenuItem("GameObject/@EditorExtension-CreateCode", false, 0)]
        static void CreateCode()
        {
            var gameObject = Selection.objects.First() as GameObject;

            if (!gameObject)
            {
                Debug.LogWarning("需要选择 GameObject");
                return;
            }

            Debug.Log("Create Code");

            var scriptsFolder = Application.dataPath + "/Scripts";

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
                    FindPath = path
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
            EditorPrefs.DeleteKey("GENERATE_CLASS_NAME");

            if (string.IsNullOrWhiteSpace(generateClassName))
            {
                Debug.Log("不继续操作");
            }
            else
            {
                Debug.Log("继续操作");

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                var defaultAssembly = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");

                var type = defaultAssembly.GetType(generateClassName);

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
                    var name = bindInfo.FindPath.Split('/').Last();

                    Debug.Log(bindInfo.FindPath);
                    Debug.Log(name);
                    Debug.Log(serialiedScript.FindProperty(name));
                    Debug.Log(gameObject.transform.Find(bindInfo.FindPath));

                    serialiedScript.FindProperty(name).objectReferenceValue =
                        gameObject.transform.Find(bindInfo.FindPath).gameObject;
                }

                serialiedScript.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}