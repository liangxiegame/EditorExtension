using UnityEngine;

namespace EditorExtension
{
    [ExecuteInEditMode]
    public class CodeGenerateInfo : MonoBehaviour
    {
        private void Awake()
        {
            ScriptsFolder = "Assets/Scripts";
            PrefabFolder = "Assets/Prefabs";
        }

        [HideInInspector]
        public string ScriptsFolder;

        [HideInInspector]
        public bool GeneratePrefab = false;
        
        
        [HideInInspector]
        public string PrefabFolder;
    }
}