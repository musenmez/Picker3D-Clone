using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Picker3D.Runtime;
using UnityEngine.Events;
using Picker3D.Models;
using System.Linq;

namespace Picker3D.EditorSystem 
{  
    public class LevelEditorWindow : EditorWindow
    {
        public static Stack<LevelEditorActionType> EditorActions { get; set; } = new Stack<LevelEditorActionType>();
        public static GameObject LevelParent { get; private set; }
        public static GameObject PlatformParent { get; private set; }
        public static GameObject DepositAreaParent { get; private set; }
        public static GameObject CollectablesParent { get; private set; }
        public static bool IsOpened { get; private set; }
        public static LevelData CurrentLevelData { get; private set; }
        public static UnityEvent OnOpened { get; } = new UnityEvent();
        public static UnityEvent OnClosed { get; } = new UnityEvent();

        public static List<Platform> SpawnedPlaforms { get; set; } = new List<Platform>();
        public static List<DepositArea> SpawnedDepositAreas { get; set; } = new List<DepositArea>();        
        static List<DepositArea> DepositAreaPrefabs { get; set; } = new List<DepositArea>();
        static List<string> DepositAreaPrefabNames { get; set; } = new List<string>();
        static List<string> LevelDataNames { get; set; } = new List<string>();
        static List<LevelData> LevelDatas { get; set; } = new List<LevelData>();
        static List<Platform> PlatformPrefabs { get; set; } = new List<Platform>();
        static List<string> PlatformPrefabNames { get; set; } = new List<string>();        
        
        static readonly Vector3 PlatformDefaultSpawnPosition = new Vector3(0, 0, -12f);

        private const float MIN_WINDOW_WIDTH = 400f;
        private const float MIN_WINDOW_HEIGHT = 600f;       

        private const int MAX_REQUIRED_COLLECTABLE = 100;

        private const string PLATFORM_PREFAB_PATH = "Assets/[Picker3D]/Prefabs/Platforms";
        private const string DEPOSIT_AREA_PREFAB_PATH = "Assets/[Picker3D]/Prefabs/DepositAreas";
        private const string LEVEL_DATA_PATH = "Assets/[Picker3D]/Data/Levels";

        static int _platformPrefabIndex;
        static int _previewLevelDataIndex;
        static int _depositAreaPrefabIndex;
        static int _requiredCollectable;

        static Rect _platformSection;
        static Rect _depositAreaSection;
        static Rect _bottomSection;           

        [MenuItem("Picker3D/Level Editor Window")]
        public static void OpenWindow() 
        {
            Initialize();

            LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");            
            SetWindowSize(window);            
            window.Show();

            IsOpened = true;
            OnOpened.Invoke();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void CloseWindow()
        {
            if (!CheckIfWindowOpen())
                return;
         
            LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");          
            window.Close();           
        }        

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnGUI()
        {
            DrawLayouts();        
            DrawPlatformSection();
            DrawDepositAreaSection();
            DrawButtomSection();
        }  

        static void Initialize() 
        {
            SetDefaultValues();
            SetPlatformPrefabs();
            SetDepositAreaPrefabs();
            SetLevelDataCollection();
            CreateLevelStructure();
        }

        static void SetDefaultValues() 
        {
            EditorActions.Clear();
            SpawnedPlaforms.Clear();
            PlatformPrefabs.Clear();
            PlatformPrefabNames.Clear();
            _platformPrefabIndex = 0;    
        }

        static void CreateLevelStructure() 
        {
            LevelParent = new GameObject("Level Parent");
            LevelParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            PlatformParent = new GameObject("Platforms");
            PlatformParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            PlatformParent.transform.SetParent(LevelParent.transform);

            DepositAreaParent = new GameObject("Deposit Areas");
            DepositAreaParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            DepositAreaParent.transform.SetParent(LevelParent.transform);

            CollectablesParent = new GameObject("Collectables");
            CollectablesParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            CollectablesParent.transform.SetParent(LevelParent.transform);
        }

        static void DrawLayouts() 
        {
            SetPlatformSection();
            SetDepositAreaSection();
            SetBottomSection();
        }

        static void DrawPlatformSection()
        {
            GUILayout.BeginArea(_platformSection);        

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Platform Prefab");
            _platformPrefabIndex = EditorGUILayout.Popup( _platformPrefabIndex, PlatformPrefabNames.ToArray());           
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Create Platform", GUILayout.Height(30)))
            {
                CreatePlatform();
            }
            
            GUILayout.EndArea();
        }

        static void CreatePlatform()
        {
            Platform lastPlatform = GetLastSpawnedPlatform();
            Vector3 spawnPosition = lastPlatform == null ? PlatformDefaultSpawnPosition : lastPlatform.GetMaxPosition();

            Platform prefab = GetPlatformPrefab();
            if (prefab == null) 
            {
                //TODO: Error message
                return;
            }
            Platform spawnedPlatform = (Platform)PrefabUtility.InstantiatePrefab(prefab);
            spawnedPlatform.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            spawnedPlatform.transform.SetParent(PlatformParent.transform);
            SpawnedPlaforms.Add(spawnedPlatform);
            EditorActions.Push(LevelEditorActionType.PlatformCreation);
        }

        static void CreateDepositArea()
        {
            Platform lastPlatform = GetLastSpawnedPlatform();
            Vector3 spawnPosition = lastPlatform == null ? PlatformDefaultSpawnPosition : lastPlatform.GetMaxPosition();

            DepositArea prefab = GetDepositAreaPrefab();
            if (prefab == null)
            {
                //TODO: Error message
                return;
            }
            DepositArea spawnedDepositArea = (DepositArea)PrefabUtility.InstantiatePrefab(prefab);
            spawnedDepositArea.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            spawnedDepositArea.transform.SetParent(PlatformParent.transform);
            spawnedDepositArea.Initialize(_requiredCollectable);

            SpawnedPlaforms.Add(spawnedDepositArea);
            SpawnedDepositAreas.Add(spawnedDepositArea);

            EditorActions.Push(LevelEditorActionType.DepositAreaCreation);
        }

        static Platform GetLastSpawnedPlatform() 
        {
            List<Platform> platforms = new List<Platform>(SpawnedPlaforms);
            for (int i = platforms.Count - 1; i >= 0; i--)
            {
                if (platforms[i] == null)
                {
                    SpawnedPlaforms.RemoveAt(i);
                    continue;
                }
                return platforms[i];
            }
            return null;
        }

        static DepositArea GetLastSpawnedDepositArea()
        {
            List<DepositArea> depositAreas = new List<DepositArea>(SpawnedDepositAreas);
            for (int i = depositAreas.Count - 1; i >= 0; i--)
            {
                if (depositAreas[i] == null)
                {
                    SpawnedDepositAreas.RemoveAt(i);
                    continue;
                }
                return depositAreas[i];
            }
            return null;
        }

        static Platform GetPlatformPrefab() 
        {
            if (_platformPrefabIndex >= PlatformPrefabs.Count)
                return null;

            return PlatformPrefabs[_platformPrefabIndex];
        }

        static DepositArea GetDepositAreaPrefab() 
        {
            if (_depositAreaPrefabIndex >= DepositAreaPrefabs.Count)
                return null;

            return DepositAreaPrefabs[_depositAreaPrefabIndex];
        }

        static LevelData GetPreviewLevelData()
        {
            if (_previewLevelDataIndex >= LevelDatas.Count)
                return null;

            return LevelDatas[_previewLevelDataIndex];
        }

        static void DrawDepositAreaSection()
        {
            GUILayout.BeginArea(_depositAreaSection);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Required Collectable");
            _requiredCollectable = EditorGUILayout.IntSlider(_requiredCollectable, 0, MAX_REQUIRED_COLLECTABLE);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Deposit Area Prefab");
            _depositAreaPrefabIndex = EditorGUILayout.Popup(_depositAreaPrefabIndex, DepositAreaPrefabNames.ToArray());
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Create Deposit Area", GUILayout.Height(30)))
            {
                CreateDepositArea();
            }

            GUILayout.EndArea();
        }

        static void DrawButtomSection()
        {
            GUILayout.BeginArea(_bottomSection);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level Data");
            _previewLevelDataIndex = EditorGUILayout.Popup(_previewLevelDataIndex, LevelDataNames.ToArray());
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Preview Level", GUILayout.Height(30)))
            {
                PreviewLevel();               
            }

            GUILayout.Space(30);
            GUILayout.BeginHorizontal();           
            if (GUILayout.Button("Undo", GUILayout.Height(30)))
            {
                Undo();
            }

            if (CurrentLevelData == null)
            {
                if (GUILayout.Button("Save Level", GUILayout.Height(30)))
                {
                    CurrentLevelData = LevelEditorSave.SaveLevel();
                }
            }
            else
            {
                if (GUILayout.Button("Update Level", GUILayout.Height(30)))
                {
                    LevelEditorSave.UpdateLevel(CurrentLevelData);
                }
            }  
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        static void Undo()
        {
            if (EditorActions.Count == 0)
                return;

            LevelEditorActionType lastAction = EditorActions.Pop();
            switch (lastAction)
            {
                case LevelEditorActionType.PlatformCreation:
                    UndoPlatform();
                    break;

                case LevelEditorActionType.DepositAreaCreation:
                    UndoDepositArea();
                    break;

                default:
                    break;
            }
        }

        static void UndoPlatform() 
        {
            Platform lastPlatform = GetLastSpawnedPlatform();
            if (lastPlatform == null)
                return;

            SpawnedPlaforms.Remove(lastPlatform);
            DestroyImmediate(lastPlatform.gameObject);
        }

        static void UndoDepositArea()
        {
            DepositArea depositArea = GetLastSpawnedDepositArea();
            if (depositArea == null)
                return;

            SpawnedDepositAreas.Remove(depositArea);
            DestroyImmediate(depositArea.gameObject);
        }

        static void PreviewLevel() 
        {
            CurrentLevelData = GetPreviewLevelData();
            DestroyLevel();
            CreateLevelStructure();
            LevelEditorPreview.PreviewLevel(CurrentLevelData);
        }

        static void SetPlatformSection() 
        {
            _platformSection.x = 0;
            _platformSection.y = 0;
            _platformSection.width = Screen.width;
            _platformSection.height = Screen.height / 3f;
        }

        static void SetDepositAreaSection() 
        {
            _depositAreaSection.x = 0;
            _depositAreaSection.y = Screen.height / 3f;
            _depositAreaSection.width = Screen.width;
            _depositAreaSection.height = Screen.height / 3f;     
        }

        static void SetBottomSection() 
        {
            _bottomSection.x = 0;
            _bottomSection.y = (Screen.height / 3f) * 2f;
            _bottomSection.width = Screen.width;
            _bottomSection.height = Screen.height / 3f;
        }

        static void SetPlatformPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { PLATFORM_PREFAB_PATH });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Platform platform = AssetDatabase.LoadAssetAtPath<GameObject>(path).GetComponent<Platform>();
                PlatformPrefabs.Add(platform);
                PlatformPrefabNames.Add(platform.name);
            } 
        }

        static void SetDepositAreaPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { DEPOSIT_AREA_PREFAB_PATH });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                DepositArea depositArea = AssetDatabase.LoadAssetAtPath<GameObject>(path).GetComponent<DepositArea>();
                DepositAreaPrefabs.Add(depositArea);
                DepositAreaPrefabNames.Add(depositArea.name);
            }
        }

        static void SetLevelDataCollection() 
        {
            string[] guids = AssetDatabase.FindAssets("t:LevelData", new string[] { LEVEL_DATA_PATH });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                LevelDatas.Add(levelData);
                LevelDataNames.Add(levelData.name);
            }
        }

        static bool CheckIfWindowOpen() 
        {
            LevelParent = GameObject.Find("Level Parent");
            return IsOpened || LevelParent != null;
        }

        static void DestroyLevel() 
        {
            LevelParent = GameObject.Find("Level Parent");
            if (LevelParent != null)
            {
                DestroyImmediate(LevelParent);
            }
        }

        static void Dispose() 
        {
            DestroyLevel();
            IsOpened = false;
            OnClosed.Invoke();
        }

        private static void SetWindowSize(LevelEditorWindow window) 
        {
            window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
        }
    }
}

