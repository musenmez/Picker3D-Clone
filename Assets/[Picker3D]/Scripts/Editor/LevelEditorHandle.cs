﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Picker3D.EditorSystem 
{
    [InitializeOnLoad]
    public class LevelEditorHandle : Editor
    {
        public static bool IsEnabled => LevelEditorWindow.IsOpened;
        public static bool IsMouseAvailable { get; private set; }
        public static Vector3 CurrentHandlePosition = Vector3.zero;

        static readonly Vector3 SnapOffset = new Vector3(0.2f, 0f, 0.2f);
        static readonly Color PaintColor = Color.green;
        static readonly Color EraseColor = Color.red;        

        private const float CUBE_WIDTH = 0.25f;    

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
            if (!IsEnabled)
                return;            

            UpdateHandlePosition();
            CheckMouseAvailable(sceneView.position);
            UpdateRepaint();
            DrawCubeHandle();
        }
        
        static void CheckMouseAvailable(Rect sceneViewRect)
        {
            bool isMouseAvailable = Event.current.mousePosition.y < sceneViewRect.height - LevelEditorToolsMenu.GRID_HEIGHT;

            if (isMouseAvailable != IsMouseAvailable)
            {
                IsMouseAvailable = isMouseAvailable;
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
                CurrentHandlePosition = new Vector3(Mathf.Floor(hit.point.x), Mathf.Floor(hit.point.y), Mathf.Floor(hit.point.z));
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

        static void DrawCubeHandle()
        {
            if (IsMouseAvailable == false || LevelEditorToolsMenu.SelectedTool == 0)
                return;

            Handles.color = GetHandleColor(); ;
            DrawHandlesCube(CurrentHandlePosition);
        }

        static void DrawHandlesCube(Vector3 center)
        {
            Vector3 p1 = center + Vector3.up * CUBE_WIDTH * 2f + Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;
            Vector3 p2 = center + Vector3.up * CUBE_WIDTH * 2f + Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p3 = center + Vector3.up * CUBE_WIDTH * 2f - Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p4 = center + Vector3.up * CUBE_WIDTH * 2f - Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;

            Vector3 p5 = center + Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;
            Vector3 p6 = center + Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p7 = center - Vector3.right * CUBE_WIDTH - Vector3.forward * CUBE_WIDTH;
            Vector3 p8 = center - Vector3.right * CUBE_WIDTH + Vector3.forward * CUBE_WIDTH;
          
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
    }
}

