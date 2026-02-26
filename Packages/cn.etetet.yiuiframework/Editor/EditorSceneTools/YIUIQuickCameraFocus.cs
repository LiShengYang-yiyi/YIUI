using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace YIUIFramework.Editor
{
    internal static class YIUIQuickCameraFocusStyles
    {
        public static readonly GUIStyle SceneViewButtonStyle;

        static YIUIQuickCameraFocusStyles()
        {
            SceneViewButtonStyle = new GUIStyle("AppCommand")
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fixedWidth = 30
            };
        }
    }

    [InitializeOnLoad]
    public static class YIUIQuickCameraFocus
    {
        static YIUIQuickCameraFocus()
        {
            SceneView.duringSceneGui += DuringSceneGui;
        }

        private static void DuringSceneGui(SceneView view)
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(25, view.position.height - 100, 200, 200));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(EditorGUIUtility.TrTextContent("Main", "对齐到Main相机"), YIUIQuickCameraFocusStyles.SceneViewButtonStyle))
            {
                AlignToSceneCamera();
            }

            if (GUILayout.Button(EditorGUIUtility.TrTextContent("YIUI", "对齐到YIUI相机"), YIUIQuickCameraFocusStyles.SceneViewButtonStyle))
            {
                AlignToUICamera();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            Handles.EndGUI();
        }

        private static void AlignToSceneCamera()
        {
            if (TryAlignToGameObject("MainCamera", 3f, false) || TryAlignToMainCamera(3f))
            {
            }
        }

        private static void AlignToUICamera()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                Selection.activeGameObject = prefabStage.prefabContentsRoot;
                SceneView.lastActiveSceneView?.FrameSelected(); // 聚焦到选中的对象
            }
            else
            {
                if (TryAlignToGameObject("YIUICamera", 10f, true) || TryAlignToFirstCanvas(10f) || TryAlignToPrefabStage())
                {
                }
            }
        }

        private static bool TryAlignToPrefabStage()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage?.prefabContentsRoot?.GetComponent<RectTransform>() is { } rect)
            {
                AlignSceneView(rect.transform, 660f, true);
                return true;
            }

            return false;
        }

        private static bool TryAlignToGameObject(string name, float size, bool in2DMode, Vector3? offset = null)
        {
            if (GameObject.Find(name) is { } obj)
            {
                AlignSceneView(obj.transform, size, in2DMode, offset);
                return true;
            }

            return false;
        }

        private static bool TryAlignToFirstCanvas(float size)
        {
            if (Object.FindFirstObjectByType<Canvas>() is { } canvas)
            {
                AlignSceneView(canvas.transform, size, true);
                return true;
            }

            return false;
        }

        private static bool TryAlignToMainCamera(float size, Vector3? offset = null)
        {
            if (Camera.main is { } mainCamera)
            {
                AlignSceneView(mainCamera.transform, size, false, offset);
                return true;
            }

            return false;
        }

        private static void AlignSceneView(Transform target, float size, bool in2DMode, Vector3? offset = null)
        {
            if (target == null) return;

            var sceneView = SceneView.lastActiveSceneView;
            sceneView.in2DMode = in2DMode;
            sceneView.LookAt(target.position + (offset ?? Vector3.zero), target.rotation, size);
        }

        private static void AlignSceneView(Camera target, bool in2DMode, float offset = 0)
        {
            if (target == null) return;

            Vector3 offsetVec = target.transform.forward * offset;

            var sceneView = SceneView.lastActiveSceneView;
            sceneView.in2DMode = in2DMode;
            sceneView.orthographic = target.orthographic;
            sceneView.LookAt(target.transform.position + offsetVec, target.transform.rotation, target.orthographic ? target.orthographicSize : 10f);
        }
    }
}