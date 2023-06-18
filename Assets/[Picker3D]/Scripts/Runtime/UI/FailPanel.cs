using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.UI 
{
    public class FailPanel : PanelBase
    {
        private void OnEnable()
        {
            LevelManager.Instance.OnLevelFailed.AddListener(ShowPanel);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelFailed.RemoveListener(ShowPanel);
        }
    }
}

