using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picker3D.Enums;

namespace Picker3D.Runtime 
{
    public class DepositAreaParticle : MonoBehaviour
    {
        private DepositAreaBarrier _depositAreaBarrer;
        private DepositAreaBarrier DepositAreaBarrier => _depositAreaBarrer == null ? _depositAreaBarrer = GetComponent<DepositAreaBarrier>() : _depositAreaBarrer;

        [SerializeField] private Transform confettiParticlePoint;

        private void OnEnable()
        {
            DepositAreaBarrier.OnBarriersOpened.AddListener(PlayParticle);
        }

        private void OnDisable()
        {
            DepositAreaBarrier.OnBarriersOpened.RemoveListener(PlayParticle);
        }

        private void PlayParticle() 
        {
            PoolingManager.Instance.Instantiate(PoolID.ConfettiParticle, confettiParticlePoint.position, Quaternion.identity);
        }
    }
}
