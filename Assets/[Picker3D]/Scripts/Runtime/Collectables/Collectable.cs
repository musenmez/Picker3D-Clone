using DG.Tweening;
using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Runtime
{
    public class Collectable : MonoBehaviour, ICollectable, IThrowable
    {
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody == null ? _rigidbody = GetComponent<Rigidbody>() : _rigidbody;
        public bool IsCollected { get; private set; }
        public UnityEvent OnCollected { get; private set; } = new UnityEvent();        

        private const float MIN_DISPOSE_DELAY = 0.75f;
        private const float MAX_DISPOSE_DELAY = 1.25f;      

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
            float disposeDuration = Random.Range(MIN_DISPOSE_DELAY, MAX_DISPOSE_DELAY);            
            DOVirtual.DelayedCall(disposeDuration, () => gameObject.SetActive(false));
        }
    }
}
