using DG.Tweening;
using Picker3D.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Picker3D.UI 
{
    public class SuccessPanel : PanelBase
    {
        public int Reward { get; private set; }
        public bool IsClaimed { get; private set; }
        public CurrencyPanel CurrencyPanel => CurrencyManager.Instance.CurrencyPanel;

        [Header("Success Panel")]
        [SerializeField] private Transform gemIcon;
        [SerializeField] private TextMeshProUGUI rewardTextMesh;
        [SerializeField] private CanvasGroup claimButtonCanvasGroup;

        private readonly WaitForSeconds SpawnDelay = new WaitForSeconds(SPAWN_DELAY);

        private const float PUNCH_STRENGTH = 0.2f;
        private const float PUNCH_DURATION = 0.3f;
        private const Ease PUNCH_EASE = Ease.InOutSine;

        private const int MIN_GEM_REWARD = 50;
        private const int MAX_GEM_REWARD = 200;
        private const int GEM_SPAWN_AMOUNT = 10;
        private const float SPAWN_DELAY = 0.1f;

        private Tween _punchTween;

        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.Instance.OnLevelCompleted.AddListener(ShowPanel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.Instance.OnLevelCompleted.RemoveListener(ShowPanel);
        }

        public void ClaimButton()
        {
            if (IsClaimed)
                return;

            IsClaimed = true;
            StartCoroutine(ClaimCoroutine());
        }

        public override void ShowPanel()
        {
            base.ShowPanel();
            Initialize();
        }

        private void Initialize() 
        {
            Reward = GetReward();
            IsClaimed = false;
            SetClaimButton(true);            
            UpdateRewardText();
        }

        private IEnumerator ClaimCoroutine() 
        {
            Action completeAction = null;
            int currencyAmountPerGem = Reward / GEM_SPAWN_AMOUNT;
            int remainder = Reward % GEM_SPAWN_AMOUNT;   

            for (int i = 0; i < GEM_SPAWN_AMOUNT; i++)
            {
                completeAction = i == (GEM_SPAWN_AMOUNT - 1) ? CompleteClaim : completeAction;
                CurrencyPanel.CreateGem(gemIcon.position, currencyAmountPerGem, completeAction);
                DecreaseReward(currencyAmountPerGem);
                yield return SpawnDelay;
            }

            CurrencyManager.Instance.AddCurrency(remainder);
            DecreaseReward(remainder);           
        }

        private void CompleteClaim() 
        {
            CurrencyPanel.HidePanel();
            HidePanel();            
        }

        private void DecreaseReward(int amount) 
        {
            Reward -= amount;
            UpdateRewardText();
            PunchGemIcon();
        }        

        private void UpdateRewardText() 
        {
            rewardTextMesh.SetText(Reward.ToString());
        }

        private int GetReward() 
        {
            int reward = UnityEngine.Random.Range(MIN_GEM_REWARD, MAX_GEM_REWARD);
            return reward;
        }

        private void SetClaimButton(bool isEnabled) 
        {
            claimButtonCanvasGroup.alpha = isEnabled ? 1 : 0;
            claimButtonCanvasGroup.blocksRaycasts = isEnabled;
            claimButtonCanvasGroup.interactable = isEnabled;
        }

        private void PunchGemIcon() 
        {
            _punchTween?.Complete();
            _punchTween = gemIcon.DOPunchScale(Vector3.one * PUNCH_STRENGTH, PUNCH_DURATION).SetEase(PUNCH_EASE);
        }
    }
}

