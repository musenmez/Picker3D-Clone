using DG.Tweening;
using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class PlayerSize : MonoBehaviour
    {
        public float CurrentScaleMultiplier { get; private set; } = 1f;

        [SerializeField] private Transform body;

        private const float MAX_MULTIPLIER = 1.3f;
        private const float SCALE_AMOUNT = 0.05f;

        private const Ease SCALE_EASE = Ease.InOutBack;
        private const float SCALE_DURATION = 0.25f;

        private Tween _scaleTween;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = body.localScale;
        }

        private void OnEnable()
        {
            PlayerManager.Instance.OnDepositCompleted.AddListener(ScaleUp);
            PlayerManager.Instance.OnPlayerInitialized.AddListener(Initialize);
        }

        private void OnDisable()
        {
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(ScaleUp);
            PlayerManager.Instance.OnPlayerInitialized.RemoveListener(Initialize);
        }

        private void Initialize() 
        {
            CurrentScaleMultiplier = 1;
            ScaleTween(_defaultScale);
        }

        private void ScaleUp() 
        {
            CurrentScaleMultiplier += SCALE_AMOUNT;
            CurrentScaleMultiplier = Mathf.Min(CurrentScaleMultiplier, MAX_MULTIPLIER);

            Vector3 scale = _defaultScale * CurrentScaleMultiplier;
            ScaleTween(scale);
        }

        private void ScaleTween(Vector3 endValue) 
        {
            _scaleTween?.Kill();
            _scaleTween = body.DOScale(endValue, SCALE_DURATION).SetEase(SCALE_EASE);
        }
    }
}

