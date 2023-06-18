using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Models 
{
    [CreateAssetMenu(fileName = "Movement Data", menuName = "Picker3D/Data/Movement Data")]
    public class MovementData : ScriptableObject
    {
        public float MovementSpeed;
        public float MovementWidth;
        public float Sensitivity;
    }
}

