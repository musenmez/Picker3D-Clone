using DG.Tweening;
using Picker3D.Enums;
using Picker3D.Interfaces;
using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Picker3D.UI 
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class PanelBase : MonoBehaviour, IPanel
    {
        private CanvasGroup _canvasGroup = null;
        public CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;

        public bool IsPanelOpen { get; protected set; }
        public PanelID PanelID { get => panelID; protected set => panelID = value; }
        public UnityEvent OnStartPanelOpening { get; } = new UnityEvent();
        public UnityEvent OnPanelOpened { get; } = new UnityEvent();
        public UnityEvent OnStartPanelClosing { get; } = new UnityEvent();
        public UnityEvent OnPanelClosed { get; } = new UnityEvent();

        [SerializeField] protected PanelID panelID;

        [Header("Show")]
        [SerializeField] private float fadeInDuration = 0.2f;      
        [SerializeField] private float fadeInDelay;

        [Header("Hide")]
        [SerializeField] private float fadeOutDuration = 0.2f;    
        [SerializeField] private float fadeOutDelay;

        private const float MAX_ALPHA = 1f;
        private const float MIN_ALPHA = 0f;

        private Tween _alphaTween;

        private void OnEnable()
        {
            UIManager.Instance.AddPanel(this);
        }

        private void OnDisable()
        {
            UIManager.Instance.RemovePanel(this);
        }       

        public virtual void ShowPanel()
        {
            if (IsPanelOpen)
                return;

            IsPanelOpen = true;
            OnStartPanelOpening.Invoke();
            FadeTween(MAX_ALPHA, fadeInDuration, fadeInDelay, onStart: () => SetCanvasGroup(true), onComplete: () => OnPanelOpened.Invoke());
        }

        public virtual void HidePanel()
        {
            IsPanelOpen = false;
            OnStartPanelClosing.Invoke();
            FadeTween(MIN_ALPHA, fadeOutDuration, fadeOutDelay, onStart: () => SetCanvasGroup(false), onComplete: () => OnPanelClosed.Invoke());
        }

        public virtual void SetCanvasGroup(bool isEnabled)
        {
            CanvasGroup.alpha = isEnabled ? MAX_ALPHA : MIN_ALPHA;
            CanvasGroup.interactable = isEnabled;
            CanvasGroup.blocksRaycasts = isEnabled;
        }       

        protected virtual void FadeTween(float endValue, float duration, float delay, Action onStart = null, Action onComplete = null)
        {
            _alphaTween?.Kill();
            _alphaTween = CanvasGroup.DOFade(endValue, duration).SetDelay(delay).SetEase(Ease.Linear).SetUpdate(true).OnStart(() => onStart?.Invoke()).OnComplete(() => onComplete?.Invoke());
        }
    }
}

