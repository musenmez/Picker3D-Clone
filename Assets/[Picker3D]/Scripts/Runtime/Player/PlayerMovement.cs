using DG.Tweening;
using Picker3D.Managers;
using Picker3D.Movements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class PlayerMovement : RigidbodySwerveMovement
    {
        private const float MOVEMENT_TWEEN_SPEED = 40f;
        private const Ease MOVEMENT_TWEEN_EASE = Ease.InOutSine;

        private Tween _movementTween;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {            
            LevelManager.Instance.OnLevelStarted.AddListener(() => SetForwardMovement(true));
            PlayerManager.Instance.OnDepositStarted.AddListener(() => SetForwardMovement(false));
            PlayerManager.Instance.OnDepositCompleted.AddListener(() => SetForwardMovement(true));
            LevelManager.Instance.OnLevelFailed.AddListener(DisableMovement);
            PlayerManager.Instance.OnPlayerInitialized.AddListener(Initialize);
            PlayerManager.Instance.OnReachedFinishLine.AddListener(MoveTowardsNextLevel);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelStarted.RemoveListener(() => SetForwardMovement(true));
            PlayerManager.Instance.OnDepositStarted.RemoveListener(() => SetForwardMovement(false));
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(() => SetForwardMovement(true));
            LevelManager.Instance.OnLevelFailed.RemoveListener(DisableMovement);
            PlayerManager.Instance.OnPlayerInitialized.RemoveListener(Initialize);
            PlayerManager.Instance.OnReachedFinishLine.RemoveListener(MoveTowardsNextLevel);
        }

        private void Initialize() 
        {
            IsActive = true;
            SetForwardMovement(false);
            SetSwerve(true);
        }  

        private void MoveTowardsNextLevel() 
        {
            DisableMovement();
            Vector3 targetPosition = LevelManager.Instance.LevelController.NextLevel.transform.position;
            MovementTween(targetPosition, MOVEMENT_TWEEN_SPEED, MOVEMENT_TWEEN_EASE, () =>
            {                
                PlayerManager.Instance.OnReachedStartPoint.Invoke();
            });
        }

        private void DisableMovement() 
        {
            IsActive = false;
            Release();
        }

        private void SetForwardMovement(bool isEnabled) 
        {
            IsForwardMovementEnabled = isEnabled;
        }

        private void SetSwerve(bool isEnabled) 
        {
            IsSwerveEnabled = isEnabled;
        }

        private void MovementTween(Vector3 targetPosition, float speed, Ease ease, Action onComplete = null) 
        {
            _movementTween?.Kill();
            _movementTween = transform.DOMove(targetPosition, speed).SetSpeedBased(true).SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }
    }
}

