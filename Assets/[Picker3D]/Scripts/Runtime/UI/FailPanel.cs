using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.UI 
{
    public class FailPanel : PanelBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.Instance.OnLevelFailed.AddListener(ShowPanel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.Instance.OnLevelFailed.RemoveListener(ShowPanel);
        }
       
        public void RestartButton() 
        {
            HidePanel();
            LevelManager.Instance.RestartLevel();
        }
    }
}

