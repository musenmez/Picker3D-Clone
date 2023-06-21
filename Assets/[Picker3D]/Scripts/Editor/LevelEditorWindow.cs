using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using log4net.Layout;

namespace Picker3D.EditorSystem 
{
    public class LevelEditorWindow : EditorWindow
    {
        private readonly Color SectionColor = new Color(40 / 255f, 40 / 255f, 40 / 255f);

        private const float WINDOW_WIDTH = 400f;
        private const float WINDOW_HEIGHT = 600f;

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
            CreateSectionTexture();
            CreateLevelStructure();
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

        }

        private void DrawDepositAreaSection()
        {

        }

        private void DrawButtomSection()
        {

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

