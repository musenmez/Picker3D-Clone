using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Runtime 
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        public bool IsInteracted { get; protected set; }
        public UnityEvent OnInteracted { get; } = new UnityEvent();

        public virtual void Interact(Player player)
        {
            if (IsInteracted)
                return;

            IsInteracted = true;
            OnInteracted.Invoke();
        }
    }
}
