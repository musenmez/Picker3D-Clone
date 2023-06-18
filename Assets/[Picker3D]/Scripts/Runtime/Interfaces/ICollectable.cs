using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Interfaces 
{
    public interface ICollectable
    {
        bool IsCollected { get; }
        void Collect();
    }
}

