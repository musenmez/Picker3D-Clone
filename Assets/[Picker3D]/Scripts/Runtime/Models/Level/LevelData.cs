using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace Picker3D.Models 
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Level Data", menuName = "Picker3D/Data/Level Data")]
    public class LevelData : ScriptableObject
    {        
        public Material GroundMaterial;
        public Material BorderMaterial;        
    }
}
