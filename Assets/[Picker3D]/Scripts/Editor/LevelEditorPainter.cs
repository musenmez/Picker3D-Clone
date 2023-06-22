﻿using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Picker3D.EditorSystem 
{
    [InitializeOnLoad]
    public class LevelEditorPainter : Editor
    {
        public static bool IsEnabled { get; private set; }
        static List<GameObject> Collectables { get; set; } = new List<GameObject>();
        static int SelectedCollectableIndex { get; set; }

        private const float HEIGHT_OFFSET = 125;
        private const string COLLECTABLES_PATH = "Assets/[Picker3D]/Prefabs/Collectables/Items";
        private const string EDITOR_SCENE_NAME = "Editor";        

        static LevelEditorPainter() 
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;  
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;            
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (!IsEditorScene())
            {
                Disable();
                return;
            }               

            Enable();
            DrawCollectables(sceneView);
            CheckHandle();   
        }

        static void DrawCollectables(SceneView sceneView) 
        {
            Handles.BeginGUI();
            GUI.Box(new Rect(0, HEIGHT_OFFSET, 110, 400), GUIContent.none, EditorStyles.textArea);

            for (int i = 0; i < Collectables.Count; ++i)
            {
                DrawCollectable(i, sceneView.position);
            }

            Handles.EndGUI();
        }

        static void CheckHandle()
        {
            if (LevelEditorToolsMenu.SelectedTool == 0)
            {
                return;
            }

            //This method is very similar to the one in E08. Only the AddBlock function is different

            //By creating a new ControlID here we can grab the mouse input to the SceneView and prevent Unitys default mouse handling from happening
            //FocusType.Passive means this control cannot receive keyboard input since we are only interested in mouse input
            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            //If the left mouse is being clicked and no modifier buttons are being held
            if (Event.current.type == EventType.MouseDown && LevelEditorHandle.IsMouseInValidArea)
            {
                if (LevelEditorToolsMenu.SelectedTool == 1)
                {
                    RemoveObject(LevelEditorHandle.CurrentHandlePosition);
                }

                if (LevelEditorToolsMenu.SelectedTool == 2)
                {
                    if (SelectedCollectableIndex < Collectables.Count)
                    {
                        AddObject(LevelEditorHandle.CurrentHandlePosition, Collectables[SelectedCollectableIndex]);
                    }
                }
            }

            //If we press escape we want to automatically deselect our own painting or erasing tools
            if (Event.current.type == EventType.KeyDown &&
                Event.current.keyCode == KeyCode.Escape)
            {
                LevelEditorToolsMenu.SelectedTool = 0;
            }

            HandleUtility.AddDefaultControl(controlId);
        }

        static void DrawCollectable(int index, Rect sceneViewRect)
        {
            bool isActive = false;           

            if (LevelEditorToolsMenu.SelectedTool == 2 && index == SelectedCollectableIndex)
            {
                isActive = true;
            }
            
            Texture2D previewImage = AssetPreview.GetAssetPreview(Collectables[index]);
            GUIContent buttonContent = new GUIContent(previewImage);
           
            GUI.Label(new Rect(5, HEIGHT_OFFSET + index * 128 + 5, 100, 20), Collectables[index].name);
            bool isToggleDown = GUI.Toggle(new Rect(5, HEIGHT_OFFSET + index * 128 + 25, 100, 100), isActive, buttonContent, GUI.skin.button);
                        
            if (isToggleDown == true && isActive == false)
            {
                SelectedCollectableIndex = index;
                LevelEditorToolsMenu.SelectedTool = 2;
            }
        }

        public static void AddObject(Vector3 position, GameObject prefab)
        {
            if (prefab == null)
                return;

            GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            spawnedObject.transform.parent = LevelEditorWindow.CollectablesParent.transform;
            spawnedObject.transform.position = position;
           
            Undo.RegisterCreatedObjectUndo(spawnedObject, "Add " + prefab.name);         
        }

        static void RemoveObject(Vector3 position)
        {
            for (int i = 0; i < LevelEditorWindow.CollectablesParent.transform.childCount; ++i)
            {
                float distanceToBlock = Vector3.Distance(LevelEditorWindow.CollectablesParent.transform.GetChild(i).transform.position, position);
                if (distanceToBlock <= 0.2f)
                {
                    Undo.DestroyObjectImmediate(LevelEditorWindow.CollectablesParent.transform.GetChild(i).gameObject);                    
                    return;
                }
            }
        }

        static void Enable() 
        {
            if (IsEnabled)
                return;

            IsEnabled = true;
            SetDefaultValues();
            SetCollectablePrefabs();           
        }

        static void Disable() 
        {
            if (!IsEnabled)
                return;

            IsEnabled = false;
        }
        
        static void SetDefaultValues() 
        {
            Collectables.Clear();
        }

        static void SetCollectablePrefabs() 
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { COLLECTABLES_PATH });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject collectable = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                Collectables.Add(collectable);
            }
        }

        static bool IsEditorScene()
        {
            return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == EDITOR_SCENE_NAME;
        }
    }
}

