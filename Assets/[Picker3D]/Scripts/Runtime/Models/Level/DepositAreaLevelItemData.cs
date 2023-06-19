using Picker3D.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Models 
{
    [System.Serializable]
    public class DepositAreaLevelItemData : LevelItemData
    {
        public int RequiredCollectable;

        public DepositAreaLevelItemData(PoolID poolID, Vector3 position, Quaternion rotation, Vector3 scale, int requiredCollectable) : base(poolID, position, rotation, scale)
        {
            RequiredCollectable = requiredCollectable;
        }
    }
}

