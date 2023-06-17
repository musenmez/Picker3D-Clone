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

        [HideInInspector]
        public UnityEvent OnLevelStarted = new UnityEvent();

        private void OnEnable()
        {
            InputManager.Instance.OnTouched.AddListener(StartLevel);
        }

        private void OnDisable()
        {
            InputManager.Instance.OnTouched.RemoveListener(StartLevel);
        }

        private void StartLevel() 
        {
            if (IsLevelStarted)
                return;

            IsLevelStarted = true;
            OnLevelStarted.Invoke();
        }
    }
}

