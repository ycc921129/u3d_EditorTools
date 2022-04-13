//------------------------------
// Author: yangchengchao
// Data:   2020
//------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MMYang
{
	public partial class TeampCode : MonoBehaviour
	{
        public static void Write(StreamWriter writer,string name,List<BindInfo> bindInfoList)
        {
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine("");
            writer.WriteLine($"public partial class {name} : MonoBehaviour");
            writer.WriteLine("{");
            foreach (var bindinfo in bindInfoList) 
            {
                var objName = bindinfo.findPath.Split('/');
                writer.WriteLine($"\tpublic GameObject {objName[objName.Length - 1]};");
            }
            writer.WriteLine("}");
            writer.Close();
        }
	}
}

