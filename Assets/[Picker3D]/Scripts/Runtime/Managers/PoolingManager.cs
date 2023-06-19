using Picker3D.Enums;
using Picker3D.Models;
using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picker3D.Pooling;
using System.Linq;

namespace Picker3D.Managers 
{
    public class PoolingManager : Singleton<PoolingManager>
    {
        private Dictionary<PoolID, Stack<PoolObject>> _poolStacksByID = new Dictionary<PoolID, Stack<PoolObject>>();
        public Dictionary<PoolID, Stack<PoolObject>> PoolStacksByID { get => _poolStacksByID; private set => _poolStacksByID = value; }

        private Dictionary<PoolID, Pool> _poolsByID = new Dictionary<PoolID, Pool>();
        public Dictionary<PoolID, Pool> PoolsByID { get => _poolsByID; private set => _poolsByID = value; }

        [SerializeField] private List<Pool> pools = new List<Pool>();

        private void Awake()
        {
            SetPoolCollection();
            SetInitialPoolStacks();
        }        

        public PoolObject Instantiate(PoolID poolID, Vector3 position, Quaternion rotation)
        {
            PoolObject poolObject = GetPoolObject(poolID);
            poolObject.transform.SetPositionAndRotation(position, rotation);
            poolObject.Initialize();
            return poolObject;
        }      

        public void DestroyPoolObject(PoolObject poolObject)
        {
            if (!PoolStacksByID.ContainsKey(poolObject.PoolID))
                return;

            poolObject.gameObject.SetActive(false);
            poolObject.transform.SetParent(transform);
            poolObject.Dispose();

            PoolStacksByID[poolObject.PoolID].Push(poolObject);
        }

        public PoolObject GetPoolObject(PoolID poolID)
        {
            if (!PoolStacksByID.ContainsKey(poolID))
            {
                Debug.LogError("Pool with ID " + poolID + " does not exist.");
                return null;
            }

            PoolObject poolObject;

            if (PoolStacksByID[poolID].Count == 0)
            {
                poolObject = CreatePoolObject(poolID);
            }
            else
            {
                poolObject = PoolStacksByID[poolID].Pop();
            }

            if (poolObject == null)
            {
                poolObject = CreatePoolObject(poolID);
            }

            if (poolObject != null)
            {
                poolObject.gameObject.SetActive(true);
            }

            return poolObject;
        }

        private PoolObject CreatePoolObject(PoolID poolID)
        {
            if (!PoolsByID.ContainsKey(poolID))
                return null;

            PoolObject poolObject = Instantiate(PoolsByID[poolID].Prefab).GetComponent<PoolObject>();
            poolObject.transform.SetParent(transform);
            poolObject.gameObject.SetActive(false);

            return poolObject;
        }

        private void SetInitialPoolStacks()
        {
            foreach (Pool pool in pools)
            {
                if (PoolStacksByID.ContainsKey(pool.PoolID))
                    continue;

                Stack<PoolObject> poolStack = new Stack<PoolObject>();
                for (int i = 0; i < pool.InitialSize; i++)
                {
                    PoolObject poolObject = CreatePoolObject(pool.PoolID);
                    poolStack.Push(poolObject);
                }

                PoolStacksByID.Add(pool.PoolID, poolStack);
            }
        }

        private void SetPoolCollection()
        {
            foreach (var pool in pools)
            {
                if (!PoolsByID.ContainsKey(pool.PoolID))
                {
                    PoolsByID.Add(pool.PoolID, pool);
                }
            }
        }
    }
}

