using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Picker3D.Models 
{
    [CreateAssetMenu(fileName = "Pool Database", menuName = "Picker3D/Data/Pool Database")]
    public class PoolDatabase : ScriptableObject
    {
        [ReorderableList]
        public List<Pool> Pools = new List<Pool>();
    }
}

