using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picker3D.PlayerSystem;
using UnityEngine.Events;

namespace Picker3D.Managers 
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        public Player CurrentPlayer { get; private set; }        
        public UnityEvent OnDepositStarted { get; } = new UnityEvent();      
        public UnityEvent OnDepositCompleted { get; } = new UnityEvent();        
        public UnityEvent OnDepositFailed { get; } = new UnityEvent();
        public UnityEvent OnReachedFinishLine { get; } = new UnityEvent();
        public UnityEvent OnReachedStartPoint { get; } = new UnityEvent();

        public void SetPlayer(Player player) 
        {
            CurrentPlayer = player;
        }
    }
}

