using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EditorExtension
{

    public class CreateUIRootWindow : EditorWindow
    {
        [MenuItem("编辑器扩展/1.SetupUIRoot", true)]
        static bool ValidateUIRoot()
        {
            return !GameObject.Find("UIRoot");
        }

        private string mWidth  = "720";
        private string mHeight = "1280";

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("width:", GUILayout.Width(45));
            mWidth = GUILayout.TextField(mWidth);
            GUILayout.Label("x", GUILayout.Width(10));
            GUILayout.Label("height:", GUILayout.Width(50));
            mHeight = GUILayout.TextField(mHeight);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Setup"))
            {
                var width = float.Parse(mWidth);
                var height = float.Parse(mHeight);

                Setup(width, height);

                Close();
            }
        }

        [MenuItem("编辑器扩展/1.SetupUIRoot")]
        static void SetupUIRoot()
        {
            var window = GetWindow<CreateUIRootWindow>();

            window.Show();
        }

        static void Setup(float width, float height)
        {
            // UIRoot
            var uiRootObj = new GameObject("UIRoot");

            var uirootScript = uiRootObj.AddComponent<UIRoot>();

            uiRootObj.layer = LayerMask.NameToLayer("UI");

            // Canvas
            var canvas = new GameObject("Canvas");

            canvas.transform.SetParent(uiRootObj.transform);

            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            // CanvasScaler
            var canvasScaler = canvas.AddComponent<CanvasScaler>();

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(width, height);

            canvas.AddComponent<GraphicRaycaster>();

            canvas.layer = LayerMask.NameToLayer("UI");

            // EventSystem
            var eventSystem = new GameObject("EventSystem");

            eventSystem.transform.SetParent(uiRootObj.transform);

            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.layer = LayerMask.NameToLayer("UI");

            // Bg
            var bgObj = new GameObject("Bg");
            bgObj.AddComponent<RectTransform>();
            bgObj.transform.SetParent(canvas.transform);
            bgObj.transform.localPosition = Vector3.zero;

            uirootScript.Bg = bgObj.transform;
            // Common

            var commonObj = new GameObject("Common");
            commonObj.AddComponent<RectTransform>();
            commonObj.transform.SetParent(canvas.transform);
            commonObj.transform.localPosition = Vector3.zero;

            uirootScript.Common = commonObj.transform;
            // PopUI
            var popUp = new GameObject("PopUp");
            popUp.AddComponent<RectTransform>();
            popUp.transform.SetParent(canvas.transform);
            popUp.transform.localPosition = Vector3.zero;

            uirootScript.PopUp = popUp.transform;

            // Forward
            var forwardObj = new GameObject("Forward");
            forwardObj.AddComponent<RectTransform>();
            forwardObj.transform.SetParent(canvas.transform);
            forwardObj.transform.localPosition = Vector3.zero;

            uirootScript.Forward = forwardObj.transform;


            var uirootScriptSerializedObj = new SerializedObject(uirootScript);

            uirootScriptSerializedObj.FindProperty("mRootCanvas").objectReferenceValue = canvas.GetComponent<Canvas>();
            uirootScriptSerializedObj.ApplyModifiedPropertiesWithoutUndo();


            // 制作 prefab
            var savedFolder = Application.dataPath + "/Resources";

            if (!Directory.Exists(savedFolder))
            {
                Directory.CreateDirectory(savedFolder);
            }

            var savedFilePath = savedFolder + "/UIRoot.prefab";

            PrefabUtility.SaveAsPrefabAssetAndConnect(uiRootObj, savedFilePath, InteractionMode.AutomatedAction);
        }
    }
}