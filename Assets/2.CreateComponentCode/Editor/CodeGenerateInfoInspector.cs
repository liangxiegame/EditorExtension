using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtension
{
    [CustomEditor(typeof(CodeGenerateInfo),editorForChildClasses:true)]
    public class CodeGenerateInfoInspector : Editor
    {
        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            
            var codeGenerateInfo = target as CodeGenerateInfo;
            
            
            
            GUILayout.BeginVertical("box");
            
            GUILayout.Label("代码生成部分",new GUIStyle()
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15
            });
            GUILayout.BeginHorizontal();
            GUILayout.Label("Scripts Generate Folder:",GUILayout.Width(150));
            codeGenerateInfo.ScriptsFolder = GUILayout.TextField(codeGenerateInfo.ScriptsFolder);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            codeGenerateInfo.GeneratePrefab = GUILayout.Toggle(codeGenerateInfo.GeneratePrefab,"Generete Prefab");
            GUILayout.EndHorizontal();

            
            if (codeGenerateInfo.GeneratePrefab)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Prefab Generate Folder:",GUILayout.Width(150));
                codeGenerateInfo.PrefabFolder =  GUILayout.TextField(codeGenerateInfo.PrefabFolder);
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndHorizontal();

            
        }
    }

}