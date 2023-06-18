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
            LevelManager.Instance.OnLevelStarted.AddListener(() => SetForwardMovement(true));
            LevelManager.Instance.OnLevelFailed.AddListener(DisableAllMovements);
            PlayerManager.Instance.OnDepositStarted.AddListener(() => SetForwardMovement(false));
            PlayerManager.Instance.OnDepositCompleted.AddListener(() => SetForwardMovement(true));            
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelStarted.RemoveListener(() => SetForwardMovement(true));
            LevelManager.Instance.OnLevelFailed.RemoveListener(DisableAllMovements);
            PlayerManager.Instance.OnDepositStarted.RemoveListener(() => SetForwardMovement(false));
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(() => SetForwardMovement(true));
        }

        private void Initialize() 
        {
            SetForwardMovement(false);
            SetSwerve(true);
        }  

        private void DisableAllMovements() 
        {
            SetForwardMovement(false);
            SetSwerve(false);
        }

        private void SetForwardMovement(bool isEnabled) 
        {
            IsForwardMovementEnabled = isEnabled;
        }

        private void SetSwerve(bool isEnabled) 
        {
            IsSwerveEnabled = isEnabled;
        }
    }
}

