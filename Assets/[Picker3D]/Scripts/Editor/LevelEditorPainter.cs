using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Picker3D.EditorSystem 
{
    [InitializeOnLoad]
    public class LevelEditorPainter : Editor
    {
        public static bool IsEnabled => LevelEditorWindow.IsOpened;
        static List<GameObject> Collectables { get; set; } = new List<GameObject>();
        static int SelectedCollectableIndex { get; set; }        

        private const float HEIGHT_OFFSET = 125;
        private const string COLLECTABLES_PATH = "Assets/[Picker3D]/Prefabs/Collectables/Items";           

        static LevelEditorPainter() 
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;

            LevelEditorWindow.OnOpened.RemoveListener(Initialize);
            LevelEditorWindow.OnOpened.AddListener(Initialize);
        }

        private void OnDestroy()
        {
            LevelEditorWindow.OnOpened.RemoveListener(Initialize);
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (!IsEnabled)
                return;     
            
            DrawCollectables();
            CheckHandle();   
        }

        static void Initialize()
        {
            SetDefaultValues();
            SetCollectablePrefabs();
        }

        static void DrawCollectables() 
        {
            Handles.BeginGUI();
            GUI.Box(new Rect(0, HEIGHT_OFFSET, 110, 400), GUIContent.none, EditorStyles.textArea);

            for (int i = 0; i < Collectables.Count; ++i)
            {
                DrawCollectable(i);
            }

            Handles.EndGUI();
        }

        static void CheckHandle()
        {
            if (LevelEditorToolsMenu.SelectedTool == 0)
            {
                return;
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            if (Event.current.type == EventType.MouseDown && LevelEditorHandle.IsMouseAvailable && Event.current.button == 0)
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
            
            if (Event.current.type == EventType.KeyDown &&
                Event.current.keyCode == KeyCode.Escape)
            {
                LevelEditorToolsMenu.SelectedTool = 0;
            }

            HandleUtility.AddDefaultControl(controlId);
        }

        static void DrawCollectable(int index)
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

        static void AddObject(Vector3 position, GameObject prefab)
        {
            if (prefab == null)
                return;

            if (!IsPositionAvailable(position))
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
                float distance = Vector3.Distance(LevelEditorWindow.CollectablesParent.transform.GetChild(i).transform.position, position);
                if (distance <= 0.2f)
                {
                    Undo.DestroyObjectImmediate(LevelEditorWindow.CollectablesParent.transform.GetChild(i).gameObject);                    
                    return;
                }
            }
        }     
        
        static bool IsPositionAvailable(Vector3 position) 
        {
            bool isAvailable = true;
            for (int i = 0; i < LevelEditorWindow.CollectablesParent.transform.childCount; ++i)
            {
                float distance = Vector3.Distance(LevelEditorWindow.CollectablesParent.transform.GetChild(i).transform.position, position);
                if (distance <= 0.1f)
                {
                    isAvailable = false;
                    break;
                }
            }
            return isAvailable;
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
    }
}

