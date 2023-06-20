﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Picker3D.Runtime
{
    public class DepositAreaText : MonoBehaviour
    {
        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;

        [SerializeField] private TextMeshPro depositTextMesh; 

        private void Start()
        {
            UpdateText();
        }

        private void OnEnable()
        {
            DepositArea.OnCollectableAmountChanged.AddListener(UpdateText);
            DepositArea.OnInitialized.AddListener(UpdateText);
        }

        private void OnDisable() 
        {
            DepositArea.OnCollectableAmountChanged.RemoveListener(UpdateText);
            DepositArea.OnInitialized.RemoveListener(UpdateText);
        }

        private void UpdateText() 
        {
            string text = DepositArea.CurrentCollectableAmount + "/" + DepositArea.RequiredCollectable;
            depositTextMesh.SetText(text);
        }
    }
}
