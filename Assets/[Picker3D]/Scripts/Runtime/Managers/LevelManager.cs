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

        public List<LevelData> Levels => levels;
        public bool IsLevelStarted { get; private set; }      
        public LevelController LevelController { get; private set; }
        public UnityEvent OnLevelStarted { get; } = new UnityEvent();
        public UnityEvent OnLevelFailed{ get; } = new UnityEvent();
        public UnityEvent OnLevelRestarted { get; } = new UnityEvent();
        public UnityEvent OnLevelUpdated { get; } = new UnityEvent();

        [ReorderableList]
        [SerializeField] private List<LevelData> levels = new List<LevelData>();

        private void OnEnable()
        {
            InputManager.Instance.OnTouched.AddListener(StartLevel);
            PlayerManager.Instance.OnDepositFailed.AddListener(FailLevel);
            PlayerManager.Instance.OnReachedStartPoint.AddListener(ResetVariables);
            PlayerManager.Instance.OnReachedFinishLine.AddListener(UpdateLevel);
        }

        private void OnDisable()
        {
            InputManager.Instance.OnTouched.RemoveListener(StartLevel);
            PlayerManager.Instance.OnDepositFailed.RemoveListener(FailLevel);
            PlayerManager.Instance.OnReachedStartPoint.RemoveListener(ResetVariables);
            PlayerManager.Instance.OnReachedFinishLine.RemoveListener(UpdateLevel);
        }        

        public void SetLevelController(LevelController levelController) 
        {
            LevelController = levelController;
        }

        public void RestartLevel() 
        {
            ResetVariables();
            LevelController.RestartLevel();
            OnLevelRestarted.Invoke();
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

        private void UpdateLevel() 
        {
            CurrentLevel++;
            LevelController.UpdateLevel();
            OnLevelUpdated.Invoke();
        }

        private void ResetVariables() 
        {
            IsLevelStarted = false;
        }
    }
}

