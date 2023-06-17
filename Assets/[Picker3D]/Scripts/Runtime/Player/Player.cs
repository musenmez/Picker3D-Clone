using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Picker3D.Managers;

namespace Picker3D.PlayerSystem 
{
    public class Player : MonoBehaviour
    {
        public bool IsForwardMovementEnabled { get; private set; }
        public bool IsSwerveEnabled { get; private set; }          

        private void Awake()
        {
            PlayerManager.Instance.SetPlayer(this);
            Initialize();
        }

        private void OnEnable()
        {
            PlayerManager.Instance.OnDepositStarted.AddListener(StartDeposit);
            PlayerManager.Instance.OnDepositCompleted.AddListener(CompleteDeposit);
        }

        private void OnDisable()
        {
            PlayerManager.Instance.OnDepositStarted.RemoveListener(StartDeposit);
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(CompleteDeposit);
        }        

        private void Initialize() 
        {
            IsForwardMovementEnabled = false;
        }

        private void StartDeposit() 
        {
            IsForwardMovementEnabled = false;
        }

        private void CompleteDeposit() 
        {
            IsForwardMovementEnabled = true;
        }
    }
}

