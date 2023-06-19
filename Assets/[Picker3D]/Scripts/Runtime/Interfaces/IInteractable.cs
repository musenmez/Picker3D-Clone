using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Interfaces 
{
    public interface IInteractable
    {
        bool IsInteracted { get; }
        UnityEvent OnInteracted { get; }
        void Interact(Player player);        
    }
}

