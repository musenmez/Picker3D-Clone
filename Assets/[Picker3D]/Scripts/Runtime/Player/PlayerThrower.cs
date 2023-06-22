using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime
{
    public class PlayerThrower : MonoBehaviour
    {
        public bool IsThrowingEnabled { get; private set; } 
        public List<IThrowable> Throwables { get; private set; } = new List<IThrowable>();

        private const float THROW_FORCE = 3f;
        private const float CONSTANT_FORCE = 50f;

        private void OnEnable()
        {
            PlayerManager.Instance.OnDepositStarted.AddListener(EnableThrow);
            PlayerManager.Instance.OnDepositCompleted.AddListener(DisableThrow);
            PlayerManager.Instance.OnDepositFailed.AddListener(DisableThrow);
        }

        private void OnDisable()
        {
            PlayerManager.Instance.OnDepositStarted.RemoveListener(EnableThrow);
            PlayerManager.Instance.OnDepositCompleted.RemoveListener(DisableThrow);
            PlayerManager.Instance.OnDepositFailed.RemoveListener(DisableThrow);
        }

        private void FixedUpdate()
        {
            CheckThrowArea();
        }

        private void OnTriggerEnter(Collider other)
        {
            IThrowable throwable = other.GetComponentInParent<IThrowable>();
            if (throwable != null)
            {
                AddThrowable(throwable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IThrowable throwable = other.GetComponentInParent<IThrowable>();
            if (throwable != null)
            {
                RemoveThrowable(throwable);
            }
        }

        private void CheckThrowArea() 
        {
            if (!IsThrowingEnabled)
                return;

            AddForce(Vector3.forward * CONSTANT_FORCE);
        }

        private void AddThrowable(IThrowable throwable) 
        {
            if (Throwables.Contains(throwable))
                return;

            Throwables.Add(throwable);
        }

        private void RemoveThrowable(IThrowable throwable) 
        {
            if (!Throwables.Contains(throwable))
                return;

            Throwables.Remove(throwable);
        }

        private void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force) 
        {
            foreach (IThrowable throwable in Throwables)
            {
                throwable.Rigidbody.AddForce(force, forceMode);               
            }
        }

        private void EnableThrow() 
        {
            IsThrowingEnabled = true;
            AddForce(Vector3.forward * THROW_FORCE, ForceMode.Impulse);
        }

        private void DisableThrow()
        {
            IsThrowingEnabled = false;
        }
    }
}
