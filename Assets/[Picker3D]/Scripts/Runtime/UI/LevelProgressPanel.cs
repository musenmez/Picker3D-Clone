using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Picker3D.Managers;
using Picker3D.Models;
using Picker3D.Enums;

namespace Picker3D.UI 
{
    public class LevelProgressPanel : PanelBase
    {
        public List<ProgressSegment> ProgressSegments { get; private set; } = new List<ProgressSegment>();
        public int CurrentLevel => LevelManager.CurrentLevel;
        public int NextLevel => CurrentLevel + 1;
        public LevelData CurrentLevelData => LevelManager.Instance.LevelController.CurrentLevel.LevelData;

        [Header("Level Progress Panel")]
        [SerializeField] private Transform segmentParent;
        [SerializeField] private TextMeshProUGUI currentLevelTextMesh;
        [SerializeField] private TextMeshProUGUI nextLevelTextMesh;

        private int _currentSegmentIndex = 0;

        private void Awake()
        {
            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.Instance.OnLevelCompleted.AddListener(OnLevelCompleted);
            LevelManager.Instance.OnLevelStarted.AddListener(ShowPanel);
            LevelManager.Instance.OnLevelFailed.AddListener(HidePanel);
            LevelManager.Instance.OnLevelRestarted.AddListener(Initialize);
            CurrencyManager.Instance.OnSuccessRewardClaimed.AddListener(ShowPanel);
            PlayerManager.Instance.OnDepositCompleted.AddListener(FillSegment);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.Instance.OnLevelCompleted.RemoveListener(OnLevelCompleted);
            LevelManager.Instance.OnLevelStarted.RemoveListener(ShowPanel);
            LevelManager.Instance.OnLevelFailed.AddListener(HidePanel);
            LevelManager.Instance.OnLevelRestarted.AddListener(Initialize);
            CurrencyManager.Instance.OnSuccessRewardClaimed.RemoveListener(ShowPanel);
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(FillSegment);
        }

        private void Initialize() 
        {
            _currentSegmentIndex = 0;
            SetLevelTexts();
            DestroySegments();
            CreateSegments();
            SortSegments();
        }

        private void OnLevelCompleted() 
        {
            Initialize();
            HidePanel();
        }

        private void CreateSegments() 
        {
            for (int i = 0; i < CurrentLevelData.DepositItemDatas.Count; i++)
            {
                ProgressSegment progressSegment = PoolingManager.Instance.Instantiate(PoolID.ProgressSegment, segmentParent.position, Quaternion.identity).GetComponent<ProgressSegment>();
                progressSegment.transform.SetParent(segmentParent.transform);
                progressSegment.transform.SetSiblingIndex(i + 1);
                ProgressSegments.Add(progressSegment);
                progressSegment.Initialize();
            }            
        }

        private void FillSegment() 
        {
            if (_currentSegmentIndex > ProgressSegments.Count - 1)
                return;

            ProgressSegments[_currentSegmentIndex].SetFill();
            _currentSegmentIndex++;
        }

        private void SetLevelTexts() 
        {
            currentLevelTextMesh.SetText(CurrentLevel.ToString());
            nextLevelTextMesh.SetText(NextLevel.ToString());
        }

        private void SortSegments() 
        {
            ProgressSegments = ProgressSegments.OrderBy(segment => segment.transform.position.x).ToList();
        }

        private void DestroySegments() 
        {
            foreach (ProgressSegment progressSegment in ProgressSegments)
            {
                PoolingManager.Instance.DestroyPoolObject(progressSegment);
            }
            ProgressSegments.Clear();
        }        
    }
}

