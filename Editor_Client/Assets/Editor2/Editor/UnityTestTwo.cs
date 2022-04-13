//------------------------------
// Author: yangchengchao
// Data:   2020
//------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MMYang
{
	public partial class UnityTestTwo : EditorWindow
	{
        [MenuItem("[杨成超]/示例/Create UIRoot",true, 1)]
        static bool ValidateUIRoot()
        {
            return !GameObject.Find("UIRoot");
        }

        [MenuItem("[杨成超]/示例/Create UIRoot",false,1)]
        static void SetupUIRoot()
        {
            EditorWindow window = CreateWindow<UnityTestTwo>();
            window.Show();
        }

        private string width = "720";
        private string height = "1280";
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("width:", GUILayout.Width(45));
            width = GUILayout.TextField(width);
            GUILayout.Label("height:", GUILayout.Width(50));
            height = GUILayout.TextField(height);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Save"))
            {
                Setup(float.Parse(width), float.Parse(height));
                Close();
            }
        }
          
        static void Setup(float width,float height)
        {
            GameObject uiroot = new GameObject("UIRoot");
            uiroot.layer = LayerMask.NameToLayer("UI");
            UIRoot root = uiroot.AddComponent<UIRoot>();

            GameObject canvas = new GameObject("Canvas");
            canvas.layer = LayerMask.NameToLayer("UI");
            canvas.transform.SetParent(uiroot.transform);
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<GraphicRaycaster>();
            CanvasScaler canvasScaler = canvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(width, height);

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.layer = LayerMask.NameToLayer("UI");
            eventSystem.transform.SetParent(uiroot.transform);
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            GameObject bg = new GameObject("BG");
            bg.layer = LayerMask.NameToLayer("UI");
            bg.AddComponent<RectTransform>();
            bg.transform.SetParent(canvas.transform);
            bg.transform.localPosition = Vector3.zero;
            root.BG = bg;

            GameObject common = new GameObject("Common");
            common.layer = LayerMask.NameToLayer("UI");
            common.AddComponent<RectTransform>();
            common.transform.SetParent(canvas.transform);
            common.transform.localPosition = Vector3.zero;
            root.Common = common;

            GameObject forword = new GameObject("Forword");
            forword.layer = LayerMask.NameToLayer("UI");
            forword.AddComponent<RectTransform>();
            forword.transform.SetParent(canvas.transform);
            forword.transform.localPosition = Vector3.zero;
            root.Forword = forword;

            SerializedObject serializedObj = new SerializedObject(root);
            serializedObj.FindProperty("mCanvas").objectReferenceValue = canvas.GetComponent<Canvas>();
            serializedObj.ApplyModifiedPropertiesWithoutUndo();

            string path = Application.dataPath + "/Resources/Prefabs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            PrefabUtility.SaveAsPrefabAssetAndConnect(uiroot, path + "/UIRoot.prefab",InteractionMode.AutomatedAction);
        }
    }
}

