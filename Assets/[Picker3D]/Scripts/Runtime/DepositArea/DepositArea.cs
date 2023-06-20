using DG.Tweening;
using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Runtime 
{
    public class DepositArea : MonoBehaviour, IDepositArea
    {
        public bool IsCompleted { get; private set; }
        public bool IsAreaChecked { get; private set; }
        public int RequiredCollectable { get{ return requiredCollectable; } set { requiredCollectable = value; } }
        public int CurrentCollectableAmount { get; private set; }
        public List<IDepositorBlocker> DepositorBlockers { get; private set; } = new List<IDepositorBlocker>();
        public UnityEvent OnInitialized { get; } = new UnityEvent();
        public UnityEvent OnDepositStarted { get; } = new UnityEvent();
        public UnityEvent OnDepositCompleted { get; } = new UnityEvent();
        public UnityEvent OnDepositFailed { get; } = new UnityEvent();
        public UnityEvent OnCollectableAmountChanged { get; } = new UnityEvent();        

        public const float DEPOSIT_DURATION = 2.5f;

        [SerializeField] private int requiredCollectable;        

        private IDepositor _lastDepositor;
        private Tween _depositTimerTween;       

        public void Initialize(int requiredCollectable) 
        {
            RequiredCollectable = requiredCollectable;

            IsAreaChecked = false;
            IsCompleted = false;

            CurrentCollectableAmount = 0;
            DepositorBlockers.Clear();

            OnInitialized.Invoke();
        }

        public void AddCollectable() 
        {
            if (IsAreaChecked)
                return;

            CurrentCollectableAmount++;
            OnCollectableAmountChanged.Invoke();
        }

        public void AddDepositorBlocker(IDepositorBlocker blocker) 
        {
            if (DepositorBlockers.Contains(blocker))
                return;

            DepositorBlockers.Add(blocker);
        }

        public void RemoveDepositorBlocker(IDepositorBlocker blocker) 
        {
            if (!DepositorBlockers.Contains(blocker))
                return;

            DepositorBlockers.Remove(blocker);
            CheckBlockers();
        }

        public void StartDeposit(IDepositor depositor)
        {
            if (IsCompleted)
                return;

            _lastDepositor = depositor;
            StartDeposit();
        }

        private void CheckArea() 
        {
            IsAreaChecked = true;

            if (CurrentCollectableAmount >= RequiredCollectable)
            {
                CompleteDeposit();
            }
            else
            {
                FailDeposit();
            }
        }

        private void CheckBlockers() 
        {
            if (!IsAreaChecked || DepositorBlockers.Count > 0)
                return;

            _lastDepositor.OnDepositCompleted();           
        }

        private void StartDeposit()
        {
            _depositTimerTween?.Kill();
            _depositTimerTween = DOVirtual.DelayedCall(DEPOSIT_DURATION, CheckArea);
            OnDepositStarted.Invoke();
        }

        private void CompleteDeposit() 
        {
            IsCompleted = true;
            OnDepositCompleted.Invoke();
        }

        private void FailDeposit() 
        {
            _lastDepositor.OnDepositFailed();
            OnDepositFailed.Invoke();
        }
    }
}

