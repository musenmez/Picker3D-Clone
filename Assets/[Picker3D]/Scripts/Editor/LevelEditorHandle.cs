using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Picker3D.EditorSystem 
{
    [InitializeOnLoad]
    public class LevelEditorHandle : Editor
    {
        public static bool IsMouseInValidArea { get; private set; }
        public static Vector3 CurrentHandlePosition = Vector3.zero;

        static readonly Color PaintColor = Color.green;
        static readonly Color EraseColor = Color.red;
        static readonly Vector3 SnapOffset = new Vector3(0.2f, 0.2f, 0.2f);

        private const float CUBE_WIDTH = 0.25f;
        private const string EDITOR_SCENE_NAME = "Editor";

        private static Vector3 _lastHandlePosition = Vector3.zero;

        static LevelEditorHandle()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (!IsEditorScene())
                return;            

            UpdateHandlePosition();
            UpdateIsMouseInValidArea(sceneView.position);
            UpdateRepaint();

            DrawCubeDrawPreview();
        }
        
        static void UpdateIsMouseInValidArea(Rect sceneViewRect)
        {
            bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35;

            if (isInValidArea != IsMouseInValidArea)
            {
                IsMouseInValidArea = isInValidArea;
                SceneView.RepaintAll();
            }
        }

        static void UpdateHandlePosition()
        {
            if (Event.current == null)
                return;

            Vector2 mousePosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);

            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 offset = Vector3.zero;

                if (LevelEditorToolsMenu.SelectedTool == 1)
                {
                    offset = hit.normal;
                }               

                CurrentHandlePosition.x = Mathf.Floor(hit.point.x + offset.x);
                CurrentHandlePosition.y = Mathf.Floor(hit.point.y + offset.y);
                CurrentHandlePosition.z = Mathf.Floor(hit.point.z + offset.z);

                CurrentHandlePosition += SnapOffset;
            }
        }

        static void UpdateRepaint()
        {
            //If the cube handle position has changed, repaint the scene
            if (CurrentHandlePosition != _lastHandlePosition)
            {
                SceneView.RepaintAll();
                _lastHandlePosition = CurrentHandlePosition;
            }
        }

        static void DrawCubeDrawPreview()
        {
            if (IsMouseInValidArea == false)
                return;

            Handles.color = GetHandleColor(); ;
            DrawHandlesCube(CurrentHandlePosition);
        }

        static void DrawHandlesCube(Vector3 center)
        {
            Vector3 p1 = center + Vector3.up * CUBE_WIDTH + Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;
            Vector3 p2 = center + Vector3.up * CUBE_WIDTH + Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p3 = center + Vector3.up * CUBE_WIDTH - Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p4 = center + Vector3.up * CUBE_WIDTH - Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;

            Vector3 p5 = center - Vector3.up * CUBE_WIDTH + Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;
            Vector3 p6 = center - Vector3.up * CUBE_WIDTH + Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p7 = center - Vector3.up * CUBE_WIDTH - Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p8 = center - Vector3.up * CUBE_WIDTH - Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;
          
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);
            Handles.DrawLine(p3, p4);
            Handles.DrawLine(p4, p1);

            Handles.DrawLine(p5, p6);
            Handles.DrawLine(p6, p7);
            Handles.DrawLine(p7, p8);
            Handles.DrawLine(p8, p5);

            Handles.DrawLine(p1, p5);
            Handles.DrawLine(p2, p6);
            Handles.DrawLine(p3, p7);
            Handles.DrawLine(p4, p8);
        }

        static Color GetHandleColor() 
        {
            return LevelEditorToolsMenu.SelectedTool == 1 ? EraseColor : PaintColor;
        }
        
        static bool IsEditorScene()
        {
            return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == EDITOR_SCENE_NAME;
        }
    }
}

