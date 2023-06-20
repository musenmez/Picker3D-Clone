using NaughtyAttributes;
using Picker3D.Interfaces;
using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Models 
{
    [System.Serializable]
    public class Pool
    {        
        public PoolObject Prefab;        
        public IDepositArea depositArea;
        public int InitialSize;
    }
}

