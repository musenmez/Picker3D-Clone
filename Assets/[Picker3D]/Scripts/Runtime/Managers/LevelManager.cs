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

        public List<LevelData> Levels => levels;
        public bool IsLevelStarted { get; private set; }      
        public LevelController LevelController { get; private set; }
        public UnityEvent OnLevelStarted { get; } = new UnityEvent();
        public UnityEvent OnLevelFailed{ get; } = new UnityEvent();
        public UnityEvent OnLevelRestarted { get; } = new UnityEvent();
        public UnityEvent OnLevelCompleted { get; } = new UnityEvent();

        [ReorderableList]
        [SerializeField] private List<LevelData> levels = new List<LevelData>();

        private void OnEnable()
        {
            InputManager.Instance.OnTouched.AddListener(StartLevel);
            PlayerManager.Instance.OnDepositFailed.AddListener(FailLevel);
            PlayerManager.Instance.OnReachedStartPoint.AddListener(CompleteLevel);            
        }

        private void OnDisable()
        {
            InputManager.Instance.OnTouched.RemoveListener(StartLevel);
            PlayerManager.Instance.OnDepositFailed.RemoveListener(FailLevel);
            PlayerManager.Instance.OnReachedStartPoint.RemoveListener(CompleteLevel);            
        }        

        public void SetLevelController(LevelController levelController) 
        {
            LevelController = levelController;
        }

        public void RestartLevel() 
        {
            ResetManager();
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

        private void CompleteLevel() 
        {
            CurrentLevel++;
            ResetManager();

            LevelController.CompleteLevel();
            OnLevelCompleted.Invoke();
        }

        private void ResetManager() 
        {
            IsLevelStarted = false;
        }
    }
}

