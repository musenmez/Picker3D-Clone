using Picker3D.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime
{
    public class LevelController : MonoBehaviour
    {
        public Level CurrentLevel { get; private set; }
        public Level NextLevel { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize() 
        {
            
        }
    }
}

