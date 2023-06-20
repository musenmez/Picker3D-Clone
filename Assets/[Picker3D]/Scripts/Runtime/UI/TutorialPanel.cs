using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.UI 
{
    public class TutorialPanel : PanelBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.Instance.OnLevelStarted.AddListener(HidePanel);
            CurrencyManager.Instance.OnSuccessRewardClaimed.AddListener(ShowPanel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.Instance.OnLevelStarted.RemoveListener(HidePanel);
            CurrencyManager.Instance.OnSuccessRewardClaimed.RemoveListener(ShowPanel);
        }
    }
}

