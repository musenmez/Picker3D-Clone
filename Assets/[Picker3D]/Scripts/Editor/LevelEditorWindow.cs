using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Picker3D.Runtime;

namespace Picker3D.EditorSystem 
{
    public class LevelEditorWindow : EditorWindow
    {
        private Stack<LevelEditorActionType> EditorActions { get; set; } = new Stack<LevelEditorActionType>();
        private List<Platform> PlatformPrefabs { get; set; } = new List<Platform>();
        private List<string> PlatformPrefabNames { get; set; } = new List<string>();
        private List<Platform> SpawnedPlaforms { get; set; } = new List<Platform>();

        private readonly Color SectionColor = new Color(40 / 255f, 40 / 255f, 40 / 255f);
        private readonly Vector3 PlatformDefaultSpawnPosition = new Vector3(0, 0, -12f);

        private const float WINDOW_WIDTH = 400f;
        private const float WINDOW_HEIGHT = 600f;

        private const string PLATFORM_PREFAB_PATH = "Assets/[Picker3D]/Prefabs/Platforms";

        private const string OFFSET_INFO = "Spawn position OFFSET necessary for the level start camera view. Without OFFSET level CANNOT look seemles.";

        private int _platformPrefabIndex;
        private Vector3 _platformSpawnPosition;

        private Texture2D _sectionTexture;

        private Rect _platformSection;
        private Rect _depositAreaSection;
        private Rect _bottomSection;

        private GameObject _levelParent;
        private GameObject _platformParent;
        private GameObject _depositAreaParent;
        private GameObject _collectablesParent;

        [MenuItem("Picker3D/Level Editor Window")]
        public static void OpenWindow() 
        {
            LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
            SetWindowSize(window);
            window.Show();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
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
            CreateSectionTexture();
            CreateLevelStructure();
        }

        private void SetDefaultValues() 
        {
            EditorActions.Clear();
            SpawnedPlaforms.Clear();
            PlatformPrefabs.Clear();
            PlatformPrefabNames.Clear();
            _platformPrefabIndex = 0;
            _platformSpawnPosition = PlatformDefaultSpawnPosition;
        }

        private void CreateSectionTexture() 
        {
            _sectionTexture = new Texture2D(1, 1);
            _sectionTexture.SetPixel(0, 0, SectionColor);
            _sectionTexture.Apply();
        }

        private void CreateLevelStructure() 
        {
            _levelParent = new GameObject("Level Parent");
            _levelParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            _platformParent = new GameObject("Platforms");
            _platformParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _platformParent.transform.SetParent(_levelParent.transform);

            _depositAreaParent = new GameObject("Deposit Areas");
            _depositAreaParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _depositAreaParent.transform.SetParent(_levelParent.transform);

            _collectablesParent = new GameObject("Collectables");
            _collectablesParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            _collectablesParent.transform.SetParent(_levelParent.transform);
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
            _platformSpawnPosition = EditorGUILayout.Vector3Field("Spawn Position", _platformSpawnPosition);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Platform Prefab");
            _platformPrefabIndex = EditorGUILayout.Popup( _platformPrefabIndex, PlatformPrefabNames.ToArray());

            //_platformPrefab = (Platform)EditorGUILayout.ObjectField(_platformPrefab, typeof(Platform), false);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Create Platform", GUILayout.Height(30)))
            {
                CreatePlatform();
            }

            EditorGUILayout.HelpBox(OFFSET_INFO, MessageType.Info);
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
            spawnedPlatform.transform.SetParent(_platformParent.transform);
            SpawnedPlaforms.Add(spawnedPlatform);
            EditorActions.Push(LevelEditorActionType.PlatformCreation);
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

        private Platform GetPlatformPrefab() 
        {
            if (_platformPrefabIndex >= PlatformPrefabs.Count)
                return null;

            return PlatformPrefabs[_platformPrefabIndex];
        }

        private void DrawDepositAreaSection()
        {
            GUILayout.BeginArea(_depositAreaSection);

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
            GUI.DrawTexture(_depositAreaSection, _sectionTexture);
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

        private void Dispose() 
        {
            if (_levelParent != null) 
            {
                DestroyImmediate(_levelParent);
            }                
        }

        private static void SetWindowSize(LevelEditorWindow window) 
        {
            window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        }
    }
}

