using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Managers 
{
    public class LevelManager : Singleton<LevelManager>
    {
        public bool IsLevelStarted { get; private set; }       
        public UnityEvent OnLevelStarted { get; } = new UnityEvent();
        public UnityEvent OnLevelFailed{ get; } = new UnityEvent();

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

