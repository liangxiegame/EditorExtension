using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EditorExtension
{
    public class ComponentDesignerTemplate
    {
        public static void Write(string name,string scriptsFolder, List<BindInfo> bindInfos)
        {
            
            var scriptFile = scriptsFolder + $"/{name}.Designer.cs";
            var writer = File.CreateText(scriptFile);
            
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine();

            writer.WriteLine($"public partial class {name}");
            writer.WriteLine("{");

            foreach (var bindInfo in bindInfos)
            {
                writer.WriteLine($"\tpublic GameObject {bindInfo.FindPath.Split('/').Last()};");
            }

            writer.WriteLine();
            writer.WriteLine("}");
            
            writer.Close();

        }
    }
    
    public class ComponentTemplate
    {
        public static void Write(string name,string scriptsFolder)
        {
            
            var scriptFile = scriptsFolder + $"/{name}.cs";

            if (File.Exists(scriptFile))
            {
                return;
            }
            
            var writer = File.CreateText(scriptFile);
            
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine();

            writer.WriteLine($"public partial class {name} : MonoBehaviour");
            writer.WriteLine("{");
            writer.WriteLine("\tvoid Start()");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\t// Code Here");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            writer.Close();
        }
    }
}