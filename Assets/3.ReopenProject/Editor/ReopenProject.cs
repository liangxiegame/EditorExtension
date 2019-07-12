using UnityEngine;
using UnityEditor;

namespace EditorExtension
{
    public class ReopenProject 
    {
        [MenuItem("编辑器扩展/3.ReopenProject &r")]
        static void DoReopenProject()
        {
            EditorApplication.OpenProject(Application.dataPath.Replace("Assets",string.Empty));
        }
    }
}