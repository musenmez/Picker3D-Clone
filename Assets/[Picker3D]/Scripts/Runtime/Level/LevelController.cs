using Picker3D.Managers;
using Picker3D.Models;
using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Picker3D.Runtime
{
    public class LevelController : MonoBehaviour
    {
        public List<Level> Levels { get; private set; } = new List<Level>();
        public Level CurrentLevel => Levels[0];
        public Level NextLevel => Levels[1];        
        private int CurrentLevelIndex => LevelManager.CurrentLevel;      
        private List<LevelData> LevelDatas => LevelManager.Instance.Levels;

        [SerializeField] private Transform levelContainer;

        private const int START_LEVEL_COUNT = 3;
        private const string LEVEL_NAME_PREFIX = "LEVEL ";

        private void Awake()
        {
            LevelManager.Instance.SetLevelController(this);
            Initialize();   
        }

        public void AddLevel(Level level) 
        {
            if (Levels.Contains(level))
                return;

            Levels.Add(level);
            SortLevels();
        }

        public void RemoveLevel(Level level) 
        {
            if (!Levels.Contains(level))
                return;

            Levels.Remove(level);
        }

        public void RestartLevel() 
        {
            LevelData currentLevelData = CurrentLevel.LevelData;
            Vector3 currentLevelPosition = CurrentLevel.transform.position;

            CurrentLevel.DestroyLevel();

            Level level = CreateLevel(CurrentLevelIndex, currentLevelData);
            level.transform.position = currentLevelPosition;
        }

        public void UpdateLevel() 
        {
            CurrentLevel.DestroyLevel();
            int levelIndex = CurrentLevelIndex + Levels.Count;
            CreateLevel(levelIndex);
        }

        private void Initialize() 
        {
            for (int i = 0; i < START_LEVEL_COUNT; i++)
            {
                CreateLevel(CurrentLevelIndex + i);                
            }
        }             

        private Level CreateLevel(int levelIndex, LevelData levelData = null) 
        {
            levelData = levelData == null ? GetLevelData(levelIndex) : levelData;

            GameObject levelParent = new GameObject(LEVEL_NAME_PREFIX + levelIndex);
            levelParent.transform.SetParent(levelContainer);

            Level level = levelParent.AddComponent<Level>();
            level.Initialize(levelData, this);

            int index = levelIndex - CurrentLevelIndex;
            if (index != 0)
            {
                float offet = (level.transform.position - level.GetMinPosition()).z;
                Vector3 position = Levels[index - 1].GetMaxPosition() + Vector3.forward * offet;
                level.transform.position = position;
            }

            AddLevel(level);
            return level;
        }          

        private LevelData GetLevelData(int levelIndex) 
        {
            if (levelIndex < LevelDatas.Count - 1)
            {
                return LevelDatas[levelIndex];
            }

            List<LevelData> levelDatas = new List<LevelData>(LevelDatas);
            if (Levels.Count > 0)
            {
                Level lastLevel = Levels[Levels.Count - 1];
                int index = LevelDatas.IndexOf(lastLevel.LevelData);
                levelDatas.RemoveAt(index);
            }  

            Random.InitState(CurrentLevelIndex);
            levelDatas.Shuffle();

            return levelDatas[0];
        }

        private void SortLevels() 
        {
            Levels = Levels.OrderBy(level => level.transform.position.z).ToList();
        }
    }
}

