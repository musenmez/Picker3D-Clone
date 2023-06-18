using DG.Tweening;
using Picker3D.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.DepositAreaSystem 
{
    public class DepositAreaBarrier : MonoBehaviour, IDepositorBlocker
    {
        private DepositAreaTopCover _topCover;
        private DepositAreaTopCover TopCover => _topCover == null ? _topCover = GetComponentInParent<DepositAreaTopCover>() : _topCover;

        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;

        public bool IsCompleted {get; private set;}
        public UnityEvent OnBarriersOpened { get; private set; } = new UnityEvent();

        [SerializeField] private Transform leftBarrier;
        [SerializeField] private Transform rightBarrier;

        private const float LEFT_BARRIER_ANGLE = 60f;
        private const float RIGHT_BARRIER_ANGLE = -60f;
        private const float BARRIER_DURATION = 1f;
        private const Ease BARRIER_EASE = Ease.Linear;

        private void Awake()
        {
            DepositArea.AddDepositorBlocker(this);
        }

        private void OnEnable()
        {
            TopCover.OnTopCoverPlaced.AddListener(OpenBarriers);
        }

        private void OnDisable()
        {
            TopCover.OnTopCoverPlaced.RemoveListener(OpenBarriers);
        }

        private void OpenBarriers() 
        {
            BarrierTween(leftBarrier, LEFT_BARRIER_ANGLE);
            BarrierTween(rightBarrier, RIGHT_BARRIER_ANGLE, CompleteOpen);
        }

        private void CompleteOpen() 
        {
            DepositArea.RemoveDepositorBlocker(this);
            OnBarriersOpened.Invoke();
        }

        private void BarrierTween(Transform barrier, float endValue, Action onComplete = null) 
        {
            barrier.DOKill();
            barrier.DOLocalRotate(Vector3.right * endValue, BARRIER_DURATION).SetEase(BARRIER_EASE).OnComplete(() => onComplete?.Invoke());
        }
    }
}

