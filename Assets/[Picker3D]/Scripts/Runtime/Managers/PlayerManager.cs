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
        
        public UnityEvent OnDepositStarted { get; private set; } = new UnityEvent();      
        public UnityEvent OnDepositCompleted { get; private set; } = new UnityEvent();        
        public UnityEvent OnDepositFailed { get; private set; } = new UnityEvent();

        public void SetPlayer(Player player) 
        {
            CurrentPlayer = player;
        }
    }
}

