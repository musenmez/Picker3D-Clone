using Picker3D.Managers;
using Picker3D.Models;
using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime
{
    public class LevelController : MonoBehaviour
    {
        public Level CurrentLevel { get; private set; }
        public Level NextLevel { get; private set; }
        private int CurrentLevelDataIndex => LevelManager.CurrentLevelDataIndex;
        private int CurrentLevelIndex => LevelManager.CurrentLevel;
        private List<LevelData> LevelDatas => LevelManager.Instance.Levels;

        [SerializeField] private Transform levelParent;
        
        private const string LEVEL_NAME_PREFIX = "LEVEL ";

        private void Awake()
        {
            Initialize();   
        }

        private void Initialize() 
        {
            CreateCurrentLevel();
            CreateNextLevel();
        }

        private void CreateCurrentLevel() 
        {           
            LevelData levelData = GetLevelData(CurrentLevelDataIndex);
            GameObject level = new GameObject(LEVEL_NAME_PREFIX + CurrentLevelIndex);
            level.transform.SetParent(levelParent);

            CurrentLevel = level.AddComponent<Level>();
            CurrentLevel.Initialize(levelData);
        }

        private void CreateNextLevel() 
        {
            LevelData levelData = GetLevelData(CurrentLevelDataIndex + 1);
            GameObject level = new GameObject(LEVEL_NAME_PREFIX + (CurrentLevelIndex + 1));            
            level.transform.SetParent(levelParent);

            NextLevel = level.AddComponent<Level>();
            NextLevel.Initialize(levelData);

            float offet = (NextLevel.transform.position - NextLevel.GetMinPosition()).z;
            Vector3 position = CurrentLevel.GetMaxPosition() + Vector3.forward * offet;
            NextLevel.transform.position = position;
        }

        private LevelData GetLevelData(int levelDataIndex) 
        {
            if (LevelDatas.Count == 1)
            {
                return LevelDatas[0];
            }

            if (levelDataIndex < LevelDatas.Count - 1)
            {
                return LevelDatas[levelDataIndex];
            }

            List<LevelData> levelDatas = new List<LevelData>(LevelDatas);
            levelDatas.RemoveAt(CurrentLevelDataIndex);

            Random.InitState(CurrentLevelIndex);
            levelDatas.Shuffle();

            return levelDatas[0];
        }
    }
}

