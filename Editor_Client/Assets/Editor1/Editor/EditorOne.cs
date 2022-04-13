//------------------------------
// Author: yangchengchao
// Data:   2020
//------------------------------

using UnityEngine;
using UnityEditor;

namespace MMYang
{
    public partial class 测试 : ScriptableWizard
    {
        [MenuItem("[杨成超]/测试/TestOne", false, 2)]
        static void TestOne()
        {
            Debug.Log("testOne");
        }
        [MenuItem("[杨成超]/测试/TestOne", true, 2)]
        static void TestOneCheck()
        {
            Debug.Log("testOne");
        }

        [MenuItem("[杨成超]/测试/TestTwo %q", false, 3)]
        static void TestTwo()
        {
            Debug.Log("testTwo");
        }

        [MenuItem("[杨成超]/测试/TestThree #w", false, 4)]
        static void TestThree()
        {
            Debug.Log("testThree");
        }

        [MenuItem("[杨成超]/测试/TestFour &e", false, 5)]
        static void TestFour()
        {
            Debug.Log("testFour");
        }

        [MenuItem("CONTEXT/UnityChanControlScriptWithRgidBody/InitPlayerValue")]
        static void InitPlayerValue(MenuCommand command)
        {
            UnityTestOne tx = command.context as UnityTestOne;
            tx.AnimSpeed = 0; 
        }

        [MenuItem("CONTEXT/Rigidbody/SetMass")]
        static void SetMass(MenuCommand command)
        {
            Rigidbody rd = command.context as Rigidbody;
            rd.mass = 0;
        }

        [MenuItem("[杨成超]/测试/Delete", false, 5)]
        static void DeleteGameObject()
        {
            foreach (var ob in Selection.objects)
            {
                Undo.DestroyObjectImmediate(ob);
            }
        }

        public int speed = 10;
        [MenuItem("[杨成超]/测试/ChangeWizard",false,23)]
        static void ChangeScriptableWizard()
        {
            ScriptableWizard.DisplayWizard<测试>("修改敌人", "OnWizardCreate", "OnWizardOtherButton");
        }

        private void OnWizardCreate()
        {
            GameObject[] trans = Selection.gameObjects;

            foreach (var tr in trans)
            {
                UnityTestOne t = tr.GetComponent<UnityTestOne>();
                Undo.RecordObject(t, "UnityTestOne_save");  
                t.AnimSpeed += speed;
            }
            EditorPrefs.SetInt("SaveAnimSpeed", speed);
        }
        private void OnEnable()
        {
            speed = EditorPrefs.GetInt("SaveAnimSpeed", speed);
        }

        private void OnWizardUpdate()
        {
            //当选择的时候持续更新
        }

        private void OnSelectionChange()
        {
            //更改的时候更新
            helpString = "更改正确";
            errorString = "更改错误";
        }

        private void OnWizardOtherButton() //窗口不会关闭
        {
            GameObject[] trans = Selection.gameObjects;
            EditorUtility.DisplayProgressBar("进度：", trans.Length + "个", 0);
            int count = 0;
            foreach (var tr in trans)
            {
                UnityTestOne t = tr.GetComponent<UnityTestOne>();
                Undo.RecordObject(t, "UnityTestOne_save");
                t.AnimSpeed += speed;
                count++;
                EditorUtility.DisplayProgressBar("进度：", trans.Length + "个", count);
            }
            EditorUtility.ClearProgressBar();
            ShowNotification(new GUIContent("增加了+" + speed));
        }
    }


    public class MyWindow : EditorWindow
    {
        [MenuItem("[杨成超]/测试/ShowMyWindow", false, 125)]
        static void ShowMyWindow()
        {
            MyWindow mw = EditorWindow.GetWindow<MyWindow>();
            mw.Show();
        }

        string info = "";
        private void OnGUI()
        {
            GUILayout.Label("my window");
            info = GUILayout.TextField(info);
            if (GUILayout.Button("创建"))
            {
                GameObject go = new GameObject(info);
                Undo.RegisterCreatedObjectUndo(go, "windowOBJ");
            }
        }
    }
}

