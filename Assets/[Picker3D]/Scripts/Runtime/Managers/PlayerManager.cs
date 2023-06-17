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

        [HideInInspector]
        public UnityEvent OnDepositStarted = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnDepositCompleted = new UnityEvent();
        [HideInInspector]
        public UnityEvent OnDepositFailed = new UnityEvent();

        public void SetPlayer(Player player) 
        {
            CurrentPlayer = player;
        }
    }
}

