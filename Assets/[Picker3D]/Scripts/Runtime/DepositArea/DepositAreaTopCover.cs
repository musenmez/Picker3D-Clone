using DG.Tweening;
using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Runtime
{
    public class DepositAreaTopCover : MonoBehaviour, IDepositorBlocker
    {
        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;
        public bool IsCompleted { get; private set; }
        public UnityEvent OnTopCoverPlaced { get; private set; } = new UnityEvent();        

        [SerializeField] private Transform body;

        private const Ease MOVEMENT_EASE = Ease.InOutBack;
        private const float MOVEMENT_DURATION = 1f;

        private Vector3 _defaultLocalPosition;
        private Tween _movementTween;

        private void Awake()
        {
            _defaultLocalPosition = body.localPosition;
        }

        private void OnEnable()
        {
            DepositArea.OnDepositCompleted.AddListener(PlaceTopCover);
            DepositArea.OnInitialized.AddListener(Initialize);
        }

        private void OnDisable()
        {
            DepositArea.OnDepositCompleted.RemoveListener(PlaceTopCover);
            DepositArea.OnInitialized.RemoveListener(Initialize);
        }

        private void Initialize() 
        {
            IsCompleted = false;
            DepositArea.AddDepositorBlocker(this);
            body.localPosition = _defaultLocalPosition;
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

