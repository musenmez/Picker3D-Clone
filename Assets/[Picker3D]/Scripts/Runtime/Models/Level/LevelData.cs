using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Picker3D.Models 
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Level Data", menuName = "Picker3D/Data/Level Data")]
    public class LevelData : ScriptableObject
    {        
        public Material GroundMaterial;
        public Material BorderMaterial;
        
        [Space, ReadOnly]
        public List<DepositAreaLevelItemData> DepositItemDatas = new List<DepositAreaLevelItemData>();
        [ReadOnly]
        public List<LevelItemData> LevelItemDatas = new List<LevelItemData>();
    }
}
