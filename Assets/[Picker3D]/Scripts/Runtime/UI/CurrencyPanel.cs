using DG.Tweening.Core.Easing;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picker3D.Managers;
using Picker3D.Interfaces;
using Picker3D.Enums;
using Picker3D.Runtime;
using System;
using TMPro;

namespace Picker3D.UI 
{
    public class CurrencyPanel : PanelBase
    {
        private int CurrencyAmount => CurrencyManager.CurrenyAmount;

        [Header("Currency Panel")]
        [SerializeField] private TextMeshProUGUI currencyTextMesh;
        [SerializeField] private Transform currencyTarget;
        [SerializeField] private Transform punchBody;

        private const float MOVEMENT_DURATION = 0.75f;
        private const Ease MOVEMENT_EASE = Ease.Linear;        

        private const Ease SCALE_EASE = Ease.Linear;
        private const float SCALE_DURATION = 0.3f;
        private const float MIN_SCALE_MULTIPLIER = 0.5f;

        private const float PUNCH_STRENGTH = 0.2f;
        private const float PUNCH_DURATION = 0.3f;
        private const Ease PUNCH_EASE = Ease.InOutSine;

        private Tween _punchTween;

        private void Awake()
        {
            UpdateCurrencyText();
            CurrencyManager.Instance.SetCurrencyPanel(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CurrencyManager.Instance.OnCurrencyAmountChanged.AddListener(UpdateCurrencyText);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CurrencyManager.Instance.OnCurrencyAmountChanged.RemoveListener(UpdateCurrencyText);
        }

        public void CreateGem(Vector3 position, int currencyAmount, Action onComplete = null)
        {            
            PoolObject gemIcon = PoolingManager.Instance.Instantiate(PoolID.GemIcon, position, Quaternion.identity);
            gemIcon.transform.SetParent(currencyTarget);

            GemMovement(gemIcon, currencyAmount, onComplete);
            GemScale(gemIcon);
        }

        private void GemMovement(PoolObject gem, int coinValue, Action onComplete)
        {
            string tweenID = gem.GetInstanceID() + "GemMovementTween";
            DOTween.Kill(tweenID);
            gem.transform.DOLocalMove(Vector3.zero, MOVEMENT_DURATION).SetEase(MOVEMENT_EASE).SetId(tweenID).OnComplete(() =>
            {
                onComplete?.Invoke();
                OnCoinMovementCompleted(gem, coinValue);
            });   
        }

        private void GemScale(PoolObject gem)
        {
            Vector3 defaultScale = gem.transform.localScale;
            gem.transform.localScale *= MIN_SCALE_MULTIPLIER;

            string tweenID = gem.GetInstanceID() + "GemScaleTween";
            DOTween.Kill(tweenID);
            gem.transform.DOScale(defaultScale, SCALE_DURATION).SetEase(SCALE_EASE).SetId(tweenID);            
        }

        private void PunchScaleTween()
        {
            _punchTween?.Complete();
            _punchTween = punchBody.DOPunchScale(Vector3.one * PUNCH_STRENGTH, PUNCH_DURATION, vibrato: 1).SetEase(PUNCH_EASE);
        }       

        private void OnCoinMovementCompleted(PoolObject gem, int currenyAmount)
        {
            PunchScaleTween();
            CurrencyManager.Instance.AddCurrency(currenyAmount);            
            PoolingManager.Instance.DestroyPoolObject(gem);
        }

        private void UpdateCurrencyText() 
        {
            currencyTextMesh.SetText(CurrencyAmount.ToString());
        }
    }
}

