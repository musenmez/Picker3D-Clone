using Picker3D.Managers;
using Picker3D.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.PlayerSystem 
{
    public class PlayerMovement : RigidbodySwerveMovement
    {
        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelStarted.AddListener(OnLevelStarted);
            PlayerManager.Instance.OnDepositStarted.AddListener(OnDepositStarted);
            PlayerManager.Instance.OnDepositCompleted.AddListener(OnDepositCompleted);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelStarted.RemoveListener(OnLevelStarted);
            PlayerManager.Instance.OnDepositStarted.RemoveListener(OnDepositStarted);
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(OnDepositCompleted);
        }

        private void Initialize() 
        {
            IsForwardMovementEnabled = false;
            IsSwerveEnabled = true;
        }

        private void OnLevelStarted() 
        {
            IsForwardMovementEnabled = true;
        }

        private void OnDepositStarted() 
        {
            IsForwardMovementEnabled = false;
        }

        private void OnDepositCompleted() 
        {
            IsForwardMovementEnabled = true;
        }
    }
}

