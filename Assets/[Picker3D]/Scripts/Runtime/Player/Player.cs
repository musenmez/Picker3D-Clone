using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Picker3D.Managers;

namespace Picker3D.Runtime 
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            PlayerManager.Instance.SetPlayer(this);    
            Initialize();
        }

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelRestarted.AddListener(Initialize);
            LevelManager.Instance.OnLevelUpdated.AddListener(Initialize);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelRestarted.RemoveListener(Initialize);
            LevelManager.Instance.OnLevelUpdated.RemoveListener(Initialize);
        }

        private void Initialize() 
        {
            Vector3 initialPosition = LevelManager.Instance.LevelController.CurrentLevel.transform.position;
            transform.position = initialPosition;
            PlayerManager.Instance.OnPlayerInitialized.Invoke();
        }
    }
}

