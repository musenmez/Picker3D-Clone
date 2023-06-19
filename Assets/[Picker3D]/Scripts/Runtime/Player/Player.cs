using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Picker3D.Managers;

namespace Picker3D.PlayerSystem 
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            PlayerManager.Instance.SetPlayer(this);         
        }       
    }
}

