using Picker3D.Managers;
using Picker3D.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Player 
{
    public class PlayerMovement : RigidbodySwerveMovement
    {
        private Player _player;
        private Player Player => _player == null ? _player = GetComponent<Player>() : _player;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelStarted.AddListener(OnLevelStarted);
            Player.OnDepositStarted.AddListener(OnDepositStarted);
            Player.OnDepositCompleted.AddListener(OnDepositCompleted);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelStarted.RemoveListener(OnLevelStarted);
            Player.OnDepositStarted.RemoveListener(OnDepositStarted);
            Player.OnDepositCompleted.RemoveListener(OnDepositCompleted);
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

