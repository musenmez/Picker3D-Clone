using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Picker3D.Runtime;
using NaughtyAttributes;
using Picker3D.Models;

namespace Picker3D.Managers 
{
    public class LevelManager : Singleton<LevelManager>
    {
        public static int CurrentLevel 
        {
            get 
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevel, 1);    
            }
            private set 
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevel, value);
            }
        }

        public static int CurrentLevelDataIndex
        {
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelDataIndex, 0);
            }
            private set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentLevelDataIndex, value);
            }
        }

        public bool IsLevelStarted { get; private set; }
        public List<LevelData> Levels => levels;
        public UnityEvent OnLevelStarted { get; } = new UnityEvent();
        public UnityEvent OnLevelFailed{ get; } = new UnityEvent();

        [ReorderableList]
        [SerializeField] private List<LevelData> levels = new List<LevelData>();

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

