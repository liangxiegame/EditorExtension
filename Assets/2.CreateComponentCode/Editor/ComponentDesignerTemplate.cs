using System;
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
            
            writer.WriteLine($"// Generate Id:{Guid.NewGuid().ToString()}");
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine();

            if (NamespaceSettingsData.IsDefaultNamespace)
            {
                writer.WriteLine("// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间");
                writer.WriteLine("// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改");
            }

            writer.WriteLine($"namespace {NamespaceSettingsData.Namespace}");
            writer.WriteLine("{");
            writer.WriteLine($"\tpublic partial class {name}");
            writer.WriteLine("\t{");

            foreach (var bindInfo in bindInfos)
            {
                writer.WriteLine($"\t\tpublic {bindInfo.ComponentName} {bindInfo.Name};");
            }

            writer.WriteLine();
            writer.WriteLine("\t}");
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
            writer.WriteLine("using EditorExtension;");
            writer.WriteLine();

            if (NamespaceSettingsData.IsDefaultNamespace)
            {
                writer.WriteLine("// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间");
                writer.WriteLine("// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改");
            }

            writer.WriteLine($"namespace {NamespaceSettingsData.Namespace}");
            writer.WriteLine("{");
            writer.WriteLine($"\tpublic partial class {name} : CodeGenerateInfo");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tvoid Start()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t// Code Here");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            writer.Close();
        }
    }
}