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
        private int CurrentLevelPrefabIndex => LevelManager.CurrentLevelPrefabIndex;
        private int CurrentLevelIndex => LevelManager.CurrentLevelIndex;
        private List<Level> LevelPrefabs => LevelManager.Instance.LevelPrefabs;

        [SerializeField] private Transform levelParent;       

        private void Awake()
        {
            Initialize();   
        }

        private void Initialize() 
        {
            int index = Mathf.Clamp(CurrentLevelPrefabIndex, 0, LevelPrefabs.Count - 1);
            Level currentLevelPrefab = LevelPrefabs[index];
            float offset = (currentLevelPrefab.PlayerStartPoint.position - currentLevelPrefab.transform.position).z;
            CurrentLevel = CreateLevel(Vector3.back * offset, currentLevelPrefab);

            Level nextLevelPrefab = GetNextLevelPrefab();
            float offet = (nextLevelPrefab.transform.position - nextLevelPrefab.GetMinPosition()).z;
            NextLevel = CreateLevel(CurrentLevel.GetMaxPosition() + Vector3.forward * offet, nextLevelPrefab);
        }

        private Level CreateLevel(Vector3 position, Level prefab) 
        {
            Level level = Instantiate(prefab, position, Quaternion.identity, levelParent);
            return level;
        }

        private Level GetNextLevelPrefab() 
        {
            if (LevelPrefabs.Count == 1)
            {
                return LevelPrefabs[0];
            }

            if (LevelManager.CurrentLevelIndex < LevelPrefabs.Count - 1) 
            {
                return LevelPrefabs[LevelManager.CurrentLevelIndex];
            }            

            List<Level> levelPrefabs = new List<Level>(LevelPrefabs);
            levelPrefabs.RemoveAt(CurrentLevelPrefabIndex);

            Random.InitState(CurrentLevelIndex);
            levelPrefabs.Shuffle();          

            return levelPrefabs[0];
        }
    }
}

