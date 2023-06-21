using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Picker3D.EditorSystem 
{
    [InitializeOnLoad]
    public class LevelEditorToolsMenu : Editor
    {
        public static int SelectedTool { get; set; }
        static string[] ButtonLabels { get; } = new string[] { "None", "Erase", "Paint" };

        private const float GRID_WIDTH = 300f;

        static LevelEditorToolsMenu() 
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView) 
        {
            DrawToolsMenu(sceneView);
        }

        static void DrawToolsMenu(SceneView sceneView) 
        {
            Handles.BeginGUI();
           
            GUILayout.BeginArea(new Rect(0, sceneView.position.height - 40, sceneView.position.width, 100), EditorStyles.toolbar);
            {
                SelectedTool = GUILayout.SelectionGrid(SelectedTool, ButtonLabels, 3, EditorStyles.toolbarButton, GUILayout.Width(GRID_WIDTH));
            }
            GUILayout.EndArea();

            Handles.EndGUI();
        }
    }

}
