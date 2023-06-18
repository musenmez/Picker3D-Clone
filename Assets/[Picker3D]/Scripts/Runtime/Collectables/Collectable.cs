using DG.Tweening;
using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Collectables 
{
    public class Collectable : MonoBehaviour, ICollectable, IThrowable
    {
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody == null ? _rigidbody = GetComponent<Rigidbody>() : _rigidbody;
        public bool IsCollected { get; private set; }
        public UnityEvent OnCollected { get; private set; } = new UnityEvent();        

        private const float DISPOSE_DELAY = 1f;

        public void Collect()
        {
            if (IsCollected)
                return;

            IsCollected = true;
            OnCollected.Invoke();
            Dispose();
        }

        private void Dispose() 
        {
            DOVirtual.DelayedCall(DISPOSE_DELAY, () => gameObject.SetActive(false));
        }
    }
}
