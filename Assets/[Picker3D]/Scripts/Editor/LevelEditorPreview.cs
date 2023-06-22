using log4net.Util;
using Picker3D.Enums;
using Picker3D.Managers;
using Picker3D.Models;
using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Picker3D.EditorSystem
{
    public static class LevelEditorPreview 
    {
        private const string POOL_DATABASE_PATH = "PoolDatabase";        

        public static void PreviewLevel(LevelData levelData) 
        {
            if (levelData == null)
                return;

            PoolDatabase poolDatabase = Resources.Load<PoolDatabase>(POOL_DATABASE_PATH);
            Dictionary<PoolID, PoolObject> prefabsByID = GetPoolCollection(poolDatabase);

            List<LevelItemData> levelItemDatas = new List<LevelItemData>(levelData.LevelItemDatas);
            List<Platform> platforms = new List<Platform>();
            foreach (LevelItemData levelItemData in levelItemDatas)
            {
                Vector3 spawnPosition = LevelEditorWindow.LevelParent.transform.TransformPoint(levelItemData.Position);

                PoolObject prefab = GetPrefab(levelItemData.PoolID, prefabsByID);
                if (prefab == null)
                    continue;

                PoolObject levelItem = (PoolObject)PrefabUtility.InstantiatePrefab(prefab);
                levelItem.transform.SetPositionAndRotation(spawnPosition, levelItemData.Rotation);
                
                Platform platform = levelItem.GetComponent<Platform>();
                if (platform != null)
                {
                    platforms.Add(platform);
                    levelItem.transform.SetParent(LevelEditorWindow.PlatformParent.transform);                 
                }
                else
                {
                    levelItem.transform.SetParent(LevelEditorWindow.CollectablesParent.transform);                   
                }                
            }

            List<DepositAreaLevelItemData> depositAreaItemDatas = new List<DepositAreaLevelItemData>(levelData.DepositItemDatas);
            List<DepositArea> depositAreas = new List<DepositArea>();
            foreach (DepositAreaLevelItemData depositAreaItemData in depositAreaItemDatas)
            {
                Vector3 spawnPosition = LevelEditorWindow.LevelParent.transform.TransformPoint(depositAreaItemData.Position);

                PoolObject prefab = GetPrefab(depositAreaItemData.PoolID, prefabsByID);
                if (prefab == null)
                    continue;

                PoolObject levelItem = (PoolObject)PrefabUtility.InstantiatePrefab(prefab);
                levelItem.transform.SetPositionAndRotation(spawnPosition, depositAreaItemData.Rotation);
                levelItem.transform.SetParent(LevelEditorWindow.DepositAreaParent.transform);                

                DepositArea depositArea = levelItem.GetComponent<DepositArea>();
                depositArea.Initialize(depositAreaItemData.RequiredCollectable);
                depositAreas.Add(depositArea);
                platforms.Add(depositArea);
            }

            LevelEditorWindow.SpawnedDepositAreas = depositAreas.OrderBy(depostiArea => depostiArea.transform.position.z).ToList();
            LevelEditorWindow.SpawnedPlaforms = platforms.OrderBy(platform => platform.transform.position.z).ToList();
            LevelEditorWindow.EditorActions = GetEditorActions(LevelEditorWindow.SpawnedPlaforms);
            LevelEditorWindow.GroundMaterial = levelData.GroundMaterial;
            LevelEditorWindow.BorderMaterial = levelData.BorderMaterial;
            LevelEditorWindow.SetMaterials();
        } 

        static Dictionary<PoolID, PoolObject> GetPoolCollection(PoolDatabase poolDatabase)
        {
            Dictionary<PoolID, PoolObject> prefabsByID = new Dictionary<PoolID, PoolObject>();
            foreach (var pool in poolDatabase.Pools)
            {
                if (!prefabsByID.ContainsKey(pool.Prefab.PoolID))
                {
                    prefabsByID.Add(pool.Prefab.PoolID, pool.Prefab);
                }
            }
            return prefabsByID;
        }

        static PoolObject GetPrefab(PoolID poolID, Dictionary<PoolID, PoolObject> prefabsByID) 
        {
            if (!prefabsByID.ContainsKey(poolID))
                return null;

            return prefabsByID[poolID];
        }

        static Stack<LevelEditorActionType> GetEditorActions(List<Platform> platforms) 
        {
            Stack<LevelEditorActionType> editorActions = new Stack<LevelEditorActionType>();            
            foreach (Platform platform in platforms)
            {
                if (platform.TryGetComponent<DepositArea>(out _))
                {
                    editorActions.Push(LevelEditorActionType.DepositAreaCreation);
                }
                else
                {
                    editorActions.Push(LevelEditorActionType.PlatformCreation);
                }
            }
            return editorActions;
        }
    }
}
