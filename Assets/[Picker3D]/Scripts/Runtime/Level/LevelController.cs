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
        public Level CurrentLevel => Levels[0];
        public Level NextLevel => Levels[1];
        public List<Level> Levels { get; private set; } = new List<Level>();       
        private int CurrentLevelIndex => LevelManager.CurrentLevel;
        private List<LevelData> LevelDatas => LevelManager.Instance.Levels;

        [SerializeField] private Transform levelContainer;

        private const int START_LEVEL_COUNT = 3;
        private const string LEVEL_NAME_PREFIX = "LEVEL ";

        private void Awake()
        {
            InitializeLevels();   
        }

        private void InitializeLevels() 
        {
            for (int i = 0; i < START_LEVEL_COUNT; i++)
            {
                CreateLevel(CurrentLevelIndex + i);
            }
        }       

        private void CreateLevel(int levelIndex) 
        {
            LevelData levelData = GetLevelData(levelIndex);

            GameObject levelParent = new GameObject(LEVEL_NAME_PREFIX + levelIndex);
            levelParent.transform.SetParent(levelContainer);

            Level level = levelParent.AddComponent<Level>();
            level.Initialize(levelData);
            Levels.Add(level);           

            if (Levels.Count == 1)
                return;

            float offet = (level.transform.position - level.GetMinPosition()).z;           
            Vector3 position = Levels[Levels.Count - 2].GetMaxPosition() + Vector3.forward * offet;
            level.transform.position = position;
        }        

        private LevelData GetLevelData(int levelIndex) 
        {
            if (LevelDatas.Count == 1)
            {
                return LevelDatas[0];
            }

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
    }
}

