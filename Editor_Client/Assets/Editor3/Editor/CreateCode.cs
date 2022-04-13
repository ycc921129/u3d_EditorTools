//------------------------------
// Author: yangchengchao
// Data:   2020
//------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace MMYang
{
    public partial class CreateCode
    {
        static List<BindInfo> bindInfoList = new List<BindInfo>();
        [MenuItem("GameObject/@MMYang-Create Code", false, 0)]
        public static void CreateEditorCode()  
        {
            string scriptsPath = Application.dataPath + "/Scripts";
            if (!Directory.Exists(scriptsPath))
            {
                Directory.CreateDirectory(scriptsPath);
            }
            GameObject gameObject = Selection.objects[0] as GameObject;
            string prefabName = scriptsPath + $"/{gameObject.name}.cs"; 
            FileStream root = File.Create(prefabName);
            StreamWriter writer = new StreamWriter(root);

            bindInfoList.Clear();
            SearchTransform("", gameObject.transform);

            TeampCode.Write(writer, gameObject.name, bindInfoList);
            root.Close();
            EditorPrefs.SetString("CLASS_NAME", gameObject.name);
            AssetDatabase.Refresh();
        }
        static void SearchTransform(string basePath,Transform trans)
        {
            var bind = trans.GetComponent<Bind>();
            var root = string.IsNullOrWhiteSpace(basePath);
            if (bind && !root)
            {
                bindInfoList.Add(new BindInfo()
                {
                    findPath = basePath
                });
            }
            foreach (Transform childTrans in trans)
            {
                SearchTransform(root? childTrans.name : basePath + childTrans.name, childTrans);
            }
        }

        [DidReloadScripts] //每次unity编译完成后调用 - 系统事件
        static void AddComponentToGameObject()
        {
            string className = EditorPrefs.GetString("CLASS_NAME");
            EditorPrefs.DeleteKey("CLASS_NAME");
            if (string.IsNullOrWhiteSpace(className))
            {
                Debug.Log(string.Format("className is null"));
            }
            else
            {
                Debug.Log(string.Format("可以操作"));
                Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
                Type type = null;
                foreach (Assembly assembly in assemblys)
                {
                    if (assembly.GetName().Name == "Assembly-CSharp")
                    {
                        type = assembly.GetType(className);
                    }
                }
                GameObject obj = GameObject.Find(className);
                if (obj.GetComponent(type))
                {
                    UnityEngine.GameObject.Destroy(obj.GetComponent(type));
                }
                var scriptComponent = obj.AddComponent(type);
                SerializedObject serializedObj = new SerializedObject(scriptComponent);
                
                bindInfoList.Clear();
                SearchTransform("", obj.transform);

                foreach (var item in bindInfoList)
                {
                    string[] childNameArray = item.findPath.Split('/');
                    serializedObj.FindProperty(childNameArray[childNameArray.Length - 1]).objectReferenceValue =
                        GameObject.Find(item.findPath);
                    Debug.Log(item.findPath);
                }
                serializedObj.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}

