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
        [SerializeField] private Transform gemSpawnPoint;
        [SerializeField] private TextMeshProUGUI rewardTextMesh;
        [SerializeField] private CanvasGroup claimButtonCanvasGroup;

        private readonly WaitForSeconds SpawnDelay = new WaitForSeconds(SPAWN_DELAY);

        private const int MIN_GEM_REWARD = 50;
        private const int MAX_GEM_REWARD = 200;
        private const int GEM_SPAWN_AMOUNT = 10;
        private const float SPAWN_DELAY = 0.1f;        

        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.Instance.OnLevelUpdated.AddListener(ShowPanel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.Instance.OnLevelUpdated.RemoveListener(ShowPanel);
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
            IsClaimed = false;
            SetClaimButton(true);
            Reward = GetReward();
            UpdateRewardText();
        }

        private IEnumerator ClaimCoroutine() 
        {
            int currencyAmountPerGem = Reward / GEM_SPAWN_AMOUNT;
            int remainder = Reward % GEM_SPAWN_AMOUNT;
            Vector3 spawnPosition = gemSpawnPoint.position;
            Action gemAction = null;

            for (int i = 0; i < GEM_SPAWN_AMOUNT; i++)
            {
                gemAction = i == (GEM_SPAWN_AMOUNT - 1) ? () => CompleteClaim() : gemAction;
                CurrencyPanel.CreateGem(spawnPosition, currencyAmountPerGem, gemAction);
                DecreaseReward(currencyAmountPerGem);
                yield return SpawnDelay;
            }

            CurrencyManager.Instance.AddCurrency(remainder);
            DecreaseReward(remainder);
        }

        private void DecreaseReward(int amount) 
        {
            Reward -= amount;
            UpdateRewardText();
        }

        private void CompleteClaim()
        {
            HidePanel();
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
    }
}

