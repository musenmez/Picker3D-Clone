using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Picker3D.Runtime;

namespace Picker3D.Managers 
{
    public class LevelManager : Singleton<LevelManager>
    {
        public static int CurrentLevelIndex 
        {
            get 
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndex, 1);    
            }
            private set 
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelIndex, value);
            }
        }

        public static int CurrentLevelPrefabIndex
        {
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelPrefabIndex, 0);
            }
            private set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelPrefabIndex, value);
            }
        }

        public bool IsLevelStarted { get; private set; }
        public List<Level> LevelPrefabs => levelPrefabs;
        public UnityEvent OnLevelStarted { get; } = new UnityEvent();
        public UnityEvent OnLevelFailed{ get; } = new UnityEvent();

        [SerializeField] private List<Level> levelPrefabs = new List<Level>();

        private void OnEnable()
        {
            InputManager.Instance.OnTouched.AddListener(StartLevel);
            PlayerManager.Instance.OnDepositFailed.AddListener(FailLevel);
        }

        private void OnDisable()
        {
            InputManager.Instance.OnTouched.RemoveListener(StartLevel);
            PlayerManager.Instance.OnDepositFailed.RemoveListener(FailLevel);
        }

        private void StartLevel() 
        {
            if (IsLevelStarted)
                return;

            IsLevelStarted = true;
            OnLevelStarted.Invoke();
        }

        private void FailLevel() 
        {
            OnLevelFailed.Invoke();
        }
    }
}

