using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Interfaces 
{
    public interface IDepositArea 
    {
        bool IsCompleted { get; }       
        int RequiredCollectable { get; }
        void StartDeposit(IDepositor depositor);
    }
}

