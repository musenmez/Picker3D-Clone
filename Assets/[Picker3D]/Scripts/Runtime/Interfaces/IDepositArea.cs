using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Interfaces 
{
    public interface IDepositArea 
    {
        bool IsCompleted { get; }
        void StartDeposit(IDepositor depositor);
    }
}

