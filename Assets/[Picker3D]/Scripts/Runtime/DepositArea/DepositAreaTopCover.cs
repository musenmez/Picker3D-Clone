using DG.Tweening;
using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.DepositAreaSystem 
{
    public class DepositAreaTopCover : MonoBehaviour, IDepositorBlocker
    {
        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;
        public bool IsCompleted { get; private set; }
        public UnityEvent OnTopCoverPlaced { get; private set; } = new UnityEvent();        

        [SerializeField] private Transform body;

        private const Ease MOVEMENT_EASE = Ease.InOutBack;
        private const float MOVEMENT_DURATION = 0.5f;

        private Tween _movementTween;

        private void Awake()
        {
            DepositArea.AddDepositorBlocker(this);
        }

        private void OnEnable()
        {
            DepositArea.OnDepositCompleted.AddListener(PlaceTopCover);
        }

        private void OnDisable()
        {
            DepositArea.OnDepositCompleted.RemoveListener(PlaceTopCover);
        }

        private void PlaceTopCover() 
        {
            _movementTween?.Kill();
            _movementTween = body.DOLocalMove(Vector3.zero, MOVEMENT_DURATION).SetEase(MOVEMENT_EASE).OnComplete(CompletePlacement);           
        }

        private void CompletePlacement() 
        {
            DepositArea.RemoveDepositorBlocker(this);
            OnTopCoverPlaced.Invoke();
        }
    }
}

