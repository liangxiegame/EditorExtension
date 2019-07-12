using UnityEngine;
using UnityEngine.Internal;

namespace EditorExtension
{
    [ExecuteInEditMode]
    public class CodeGenerateInfo : MonoBehaviour
    {
        [HideInInspector]
        public string ScriptsFolder = "Assets/Scripts";

        [HideInInspector]
        public bool GeneratePrefab = false;
        
        
        [HideInInspector]
        public string PrefabFolder = "Assets/Prefabs";
    }
}