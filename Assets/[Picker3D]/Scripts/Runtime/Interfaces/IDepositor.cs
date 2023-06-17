using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Interfaces 
{
    public interface IDepositor 
    {
        bool IsAvailable { get; }
        void OnDepositCompleted();
        void OnDepositFailed();
    }
}

