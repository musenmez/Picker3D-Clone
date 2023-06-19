using Picker3D.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Models 
{
    [System.Serializable]
    public class LevelItemData
    {
        public PoolID PoolID;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public LevelItemData(PoolID poolID, Vector3 position, Quaternion rotation, Vector3 scale) 
        {
            PoolID = poolID;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
