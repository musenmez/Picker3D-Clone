using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Picker3D.Runtime;
using UnityEngine.Events;

namespace Picker3D.EditorSystem 
{
    public class LevelEditorWindow : EditorWindow
    {
        public static GameObject LevelParent { get; private set; }
        public static GameObject PlatformParent { get; private set; }
        public static GameObject DepositAreaParent { get; private set; }
        public static GameObject CollectablesParent { get; private set; }
        public static bool IsOpened { get; private set; }
        public static UnityEvent OnOpened { get; } = new UnityEvent();
        public static UnityEvent OnClosed { get; } = new UnityEvent();

        private Stack<LevelEditorActionType> EditorActions { get; set; } = new Stack<LevelEditorActionType>();
        private List<DepositArea> DepositAreaPrefabs { get; set; } = new List<DepositArea>();
        private List<string> DepositAreaPrefabNames { get; set; } = new List<string>();
        private List<DepositArea> SpawnedDepositAreas { get; set; } = new List<DepositArea>();
        private List<Platform> PlatformPrefabs { get; set; } = new List<Platform>();
        private List<string> PlatformPrefabNames { get; set; } = new List<string>();
        private List<Platform> SpawnedPlaforms { get; set; } = new List<Platform>();

        private readonly Color SectionColor = new Color(40 / 255f, 40 / 255f, 40 / 255f);
        private readonly Vector3 PlatformDefaultSpawnPosition = new Vector3(0, 0, -12f);

        private const float MIN_WINDOW_WIDTH = 400f;
        private const float MIN_WINDOW_HEIGHT = 600f;       

        private const int MAX_REQUIRED_COLLECTABLE = 100;

        private const string PLATFORM_PREFAB_PATH = "Assets/[Picker3D]/Prefabs/Platforms";
        private const string DEPOSIT_AREA_PREFAB_PATH = "Assets/[Picker3D]/Prefabs/DepositAreas";       

        private int _platformPrefabIndex;
     
        private int _depositAreaPrefabIndex;
        private int _requiredCollectable;     

        private Rect _platformSection;
        private Rect _depositAreaSection;
        private Rect _bottomSection;        

        [MenuItem("Picker3D/Level Editor Window")]
        public static void OpenWindow() 
        {
            LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");            
            SetWindowSize(window);            
            window.Show();

            IsOpened = true;
            OnOpened.Invoke();
        }

        private void OnEnable()
        {
            Initialize();
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

        private void Initialize() 
        {
            SetDefaultValues();
            SetPlatformPrefabs();
            SetDepositAreaPrefabs();          
            CreateLevelStructure();
        }

        private void SetDefaultValues() 
        {
            EditorActions.Clear();
            SpawnedPlaforms.Clear();
            PlatformPrefabs.Clear();
            PlatformPrefabNames.Clear();
            _platformPrefabIndex = 0;    
        }      

        private void CreateLevelStructure() 
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

        private void DrawLayouts() 
        {
            SetPlatformSection();
            SetDepositAreaSection();
            SetBottomSection();
        }

        private void DrawPlatformSection()
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

        private void CreatePlatform()
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

        private void CreateDepositArea()
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

        private Platform GetLastSpawnedPlatform() 
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

        private DepositArea GetLastSpawnedDepositArea()
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

        private Platform GetPlatformPrefab() 
        {
            if (_platformPrefabIndex >= PlatformPrefabs.Count)
                return null;

            return PlatformPrefabs[_platformPrefabIndex];
        }

        private DepositArea GetDepositAreaPrefab() 
        {
            if (_depositAreaPrefabIndex >= DepositAreaPrefabs.Count)
                return null;

            return DepositAreaPrefabs[_depositAreaPrefabIndex];
        }

        private void DrawDepositAreaSection()
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

        private void DrawButtomSection()
        {
            GUILayout.BeginArea(_bottomSection);
            GUILayout.BeginHorizontal();           
            if (GUILayout.Button("Undo", GUILayout.Height(30)))
            {
                Undo();
            }
            if (GUILayout.Button("Save Level", GUILayout.Height(30)))
            {
                
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void Undo()
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

        private void UndoPlatform() 
        {
            Platform lastPlatform = GetLastSpawnedPlatform();
            if (lastPlatform == null)
                return;

            SpawnedPlaforms.Remove(lastPlatform);
            DestroyImmediate(lastPlatform.gameObject);
        }

        private void UndoDepositArea()
        {
            DepositArea depositArea = GetLastSpawnedDepositArea();
            if (depositArea == null)
                return;

            SpawnedDepositAreas.Remove(depositArea);
            DestroyImmediate(depositArea.gameObject);
        }

        private void SetPlatformSection() 
        {
            _platformSection.x = 0;
            _platformSection.y = 0;
            _platformSection.width = Screen.width;
            _platformSection.height = Screen.height / 3f;
        }

        private void SetDepositAreaSection() 
        {
            _depositAreaSection.x = 0;
            _depositAreaSection.y = Screen.height / 3f;
            _depositAreaSection.width = Screen.width;
            _depositAreaSection.height = Screen.height / 3f;     
        }

        private void SetBottomSection() 
        {
            _bottomSection.x = 0;
            _bottomSection.y = (Screen.height / 3f) * 2f;
            _bottomSection.width = Screen.width;
            _bottomSection.height = Screen.height / 3f;
        }

        private void SetPlatformPrefabs()
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

        private void SetDepositAreaPrefabs()
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

        private void Dispose() 
        {
            if (LevelParent != null) 
            {
                DestroyImmediate(LevelParent);
            }

            IsOpened = false;
            OnClosed.Invoke();
        }

        private static void SetWindowSize(LevelEditorWindow window) 
        {
            window.minSize = new Vector2(MIN_WINDOW_WIDTH, MIN_WINDOW_HEIGHT);
        }
    }
}

