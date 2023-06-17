using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Player 
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        public bool IsForwardMovementEnabled { get; private set; }
        public bool IsSwerveEnabled { get; private set; }       

        [HideInInspector]
        public UnityEvent OnDepositStarted = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnDepositCompleted = new UnityEvent();

        private void Awake()
        {
            Instance = this;
            Initialize();
        }

        private void OnEnable()
        {
            OnDepositStarted.AddListener(StartDeposit);
            OnDepositCompleted.AddListener(CompleteDeposit);
        }

        private void OnDisable()
        {
            OnDepositStarted.RemoveListener(StartDeposit);
            OnDepositCompleted.RemoveListener(CompleteDeposit);
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

