using Picker3D.Models;
using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Picker3D.EditorSystem 
{    
    public static class LevelEditorSave
    {
        private const string LEVEL_NAME_PREFIX = "Level ";
        private const string LEVEL_DATA_PATH = "Assets/[Picker3D]/Data/Levels";

        public static LevelData SaveLevel() 
        {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(LEVEL_DATA_PATH + "/" + GetLevelDataName() + ".asset");

            levelData.LevelItemDatas = GetLevelItemDatas();
            levelData.DepositItemDatas = GetDepostiAreaLevelItemDatas();
            levelData.GroundMaterial = LevelEditorWindow.GroundMaterial;
            levelData.BorderMaterial = LevelEditorWindow.BorderMaterial;
            AssetDatabase.CreateAsset(levelData, uniqueAssetPath);            
            AssetDatabase.SaveAssets();        

            return levelData;
        }

        public static void UpdateLevel(LevelData levelData) 
        {
            levelData.LevelItemDatas = GetLevelItemDatas();
            levelData.DepositItemDatas = GetDepostiAreaLevelItemDatas();

            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
        }

        private static List<LevelItemData> GetLevelItemDatas() 
        {
            List<LevelItem> levelItems = LevelEditorWindow.LevelParent.GetComponentsInChildren<LevelItem>().ToList();
            List<LevelItemData> levelItemDatas = new List<LevelItemData>();

            foreach (var levelItem in levelItems)
            {
                LevelItemData levelItemData = new LevelItemData(levelItem.PoolID, levelItem.transform.localPosition, levelItem.transform.rotation, levelItem.transform.localScale);
                levelItemDatas.Add(levelItemData);
            }
            return levelItemDatas;
        }

        private static List<DepositAreaLevelItemData> GetDepostiAreaLevelItemDatas()
        {
            List<DepositAreaLevelItem> depositAreas = LevelEditorWindow.LevelParent.GetComponentsInChildren<DepositAreaLevelItem>().ToList();
            List<DepositAreaLevelItemData> depositItemDatas = new List<DepositAreaLevelItemData>();        

            foreach (DepositAreaLevelItem depositArea in depositAreas)
            {
                int requiredCollectable = depositArea.GetComponent<DepositArea>().RequiredCollectable;
                DepositAreaLevelItemData depositAreaItemData = new DepositAreaLevelItemData(depositArea.PoolID, depositArea.transform.localPosition, depositArea.transform.rotation, depositArea.transform.localScale, requiredCollectable);
                depositItemDatas.Add(depositAreaItemData);
            }            

            return depositItemDatas;
        }

        private static string GetLevelDataName() 
        {
            string[] guids = AssetDatabase.FindAssets("t:LevelData", new string[] { LEVEL_DATA_PATH });
            string levelDataName = LEVEL_NAME_PREFIX + (guids.Length + 1);
            return levelDataName;
        }
    }
}

