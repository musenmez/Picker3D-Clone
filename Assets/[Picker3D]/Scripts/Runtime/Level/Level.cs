using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Picker3D.Models;
using Picker3D.Managers;

namespace Picker3D.Runtime
{
    public class Level : MonoBehaviour
    {
        private List<Platform> _platforms = null;
        private List<Platform> Platforms { get => _platforms == null ? _platforms = GetComponentsInChildren<Platform>().ToList() : _platforms; set => _platforms = value; }
        public List<PoolObject> LevelItems { get; private set; } = new List<PoolObject>();     
        public LevelController LevelController { get; private set; }
        public LevelData LevelData { get; private set; }
        public UnityEvent OnInitialized { get; } = new UnityEvent();             

        public void Initialize(LevelData levelData, LevelController levelController) 
        {
            LevelData = levelData;
            LevelController = levelController;

            CreateLevel();
            SortPlatforms();

            OnInitialized.Invoke();
        }

        public void DestroyLevel() 
        {
            LevelController.RemoveLevel(this);
            foreach (PoolObject levelItem in LevelItems)
            {
                PoolingManager.Instance.DestroyPoolObject(levelItem);
            }
            gameObject.SetActive(false);       
        }

        public Vector3 GetMaxPosition() 
        {
            Platform platform = Platforms[Platforms.Count - 1];          
            return platform.GetMaxPosition();
        }

        public Vector3 GetMinPosition() 
        {            
            Platform platform = Platforms[0];            
            return platform.GetMinPosition();
        }

        private void CreateLevel() 
        {
            List<DepositAreaLevelItemData> depositAreaItemDatas = new List<DepositAreaLevelItemData>(LevelData.DepositItemDatas);
            foreach (DepositAreaLevelItemData depositAreaItemData in depositAreaItemDatas)
            {
                Vector3 spawnPosition = transform.TransformPoint(depositAreaItemData.Position);
                PoolObject levelItem = PoolingManager.Instance.Instantiate(depositAreaItemData.PoolID, spawnPosition, depositAreaItemData.Rotation);
                levelItem.transform.SetParent(transform);
                LevelItems.Add(levelItem);

                DepositArea depositArea = levelItem.GetComponent<DepositArea>();
                depositArea.Initialize(depositAreaItemData.RequiredCollectable);                
            }

            List<LevelItemData> levelItemDatas = new List<LevelItemData>(LevelData.LevelItemDatas);
            foreach (LevelItemData levelItemData in levelItemDatas)
            {
                Vector3 spawnPosition = transform.TransformPoint(levelItemData.Position);
                PoolObject levelItem = PoolingManager.Instance.Instantiate(levelItemData.PoolID, spawnPosition, levelItemData.Rotation);
                levelItem.transform.SetParent(transform);
                LevelItems.Add(levelItem);
            }
        }       

        private void SortPlatforms() 
        {
            Platforms = Platforms.OrderBy(platform => platform.transform.position.z).ToList();
        }
    }
}

