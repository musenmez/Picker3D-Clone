using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.DepositAreaSystem 
{
    public class DepositAreaBarrier : MonoBehaviour
    {
        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}

