using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Models 
{
    [CreateAssetMenu(fileName = "Pool Database", menuName = "Picker3D/Data/Pool Database")]
    public class PoolDatabase : ScriptableObject
    {
        public List<Pool> Pools = new List<Pool>();
    }
}

