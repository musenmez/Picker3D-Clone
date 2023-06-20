using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class FinishLine : InteractableBase
    {
        private PoolObject _poolObject;
        private PoolObject PoolObject => _poolObject == null ? _poolObject = GetComponentInParent<PoolObject>() : _poolObject;

        private void OnEnable()
        {
            PoolObject.OnInitialized.AddListener(Initialize);
        }

        private void OnDisable()
        {
            PoolObject.OnInitialized.RemoveListener(Initialize);
        }

        public override void Interact(Player player)
        {
            if (IsInteracted)
                return;

            base.Interact(player);
            PlayerManager.Instance.OnReachedFinishLine.Invoke();
        }

        private void Initialize()
        {
            IsInteracted = false;
        }
    }
}

