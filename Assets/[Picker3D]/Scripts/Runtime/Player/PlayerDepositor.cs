using Picker3D.Interfaces;
using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.PlayerSystem 
{
    public class PlayerDepositor : MonoBehaviour, IDepositor
    {
        public bool IsAvailable {get; private set;}

        private void OnTriggerEnter(Collider other)
        {
            IDepositArea depositArea = other.GetComponentInParent<IDepositArea>();
            if (depositArea != null && !depositArea.IsCompleted)
            {
                StartDeposit(depositArea);
            }
        }

        public void OnDepositCompleted()
        {
            IsAvailable = true;
            PlayerManager.Instance.OnDepositCompleted.Invoke();
        }

        public void OnDepositFailed()
        {
            IsAvailable = false;
            PlayerManager.Instance.OnDepositFailed.Invoke();
        }

        private void StartDeposit(IDepositArea depositArea) 
        {
            if (!IsAvailable)
                return;

            IsAvailable = false;
            depositArea.StartDeposit(this);
        }
    }
}

