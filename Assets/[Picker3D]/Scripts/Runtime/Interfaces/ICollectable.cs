using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Interfaces 
{
    public interface ICollectable
    {
        bool IsCollected { get; }
        UnityEvent OnCollected { get; }
        void Collect();       
    }
}

