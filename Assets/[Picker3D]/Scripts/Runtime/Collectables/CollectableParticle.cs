using Picker3D.Enums;
using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class CollectableParticle : MonoBehaviour
    {
        private Collectable _collectable;
        private Collectable Collectable => _collectable == null ? _collectable = GetComponent<Collectable>() : _collectable;

        private void OnEnable()
        {
            Collectable.OnDisposed.AddListener(PlayParticle);
        }

        private void OnDisable()
        {
            Collectable.OnDisposed.RemoveListener(PlayParticle);
        }

        private void PlayParticle() 
        {
            PoolingManager.Instance.Instantiate(PoolID.CollectParticle,transform.position, Quaternion.identity);
        }
    }
}

