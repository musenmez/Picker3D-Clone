using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime
{
    public class DepositAreaCollector : MonoBehaviour
    {
        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;

        private void OnTriggerEnter(Collider other)
        {
            ICollectable collectable = other.GetComponentInParent<ICollectable>();
            if (collectable != null && !collectable.IsCollected)
            {
                collectable.Collect();
                DepositArea.AddCollectable();
            }
        }
    }
}
