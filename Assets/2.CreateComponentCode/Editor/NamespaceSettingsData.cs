using UnityEditor;
using UnityEngine;

namespace EditorExtension
{
    public class NamespaceSettingsData
    {
        private readonly static string NAMESPACE_KEY = Application.productName + "@NAMESPACE";
        
        public static string Namespace
        {
            get
            {
                var retNamespace = EditorPrefs.GetString(NAMESPACE_KEY, "DefaultNamespace");
                 
                return string.IsNullOrWhiteSpace(retNamespace) ? "DefaultNamespace" : retNamespace;
            }
            set => EditorPrefs.SetString(NAMESPACE_KEY,value);
        }

        public static bool IsDefaultNamespace => Namespace == "DefaultNamespace";
    }
}