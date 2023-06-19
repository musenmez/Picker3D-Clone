using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Picker3D.Models;
using Picker3D.Runtime;
using System.Linq;
using Picker3D.Interfaces;

public class LevelMakerTest : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    [Button]
    private void UpdateLevelData() 
    {
        List<DepositAreaLevelItem> depositAreas = GetComponentsInChildren<DepositAreaLevelItem>().ToList();
        List<LevelItem> levelItems = GetComponentsInChildren<LevelItem>().ToList();

        List<DepositAreaLevelItemData> depositItemDatas = new List<DepositAreaLevelItemData>();
        List<LevelItemData> levelItemDatas = new List<LevelItemData>();

        foreach (DepositAreaLevelItem depositArea in depositAreas)
        {
            int requiredCollectable = depositArea.GetComponent<DepositArea>().RequiredCollectable;
            DepositAreaLevelItemData depositAreaItemData = new DepositAreaLevelItemData(depositArea.PoolID, depositArea.transform.localPosition, depositArea.transform.rotation, depositArea.transform.localScale, requiredCollectable);
            depositItemDatas.Add(depositAreaItemData);
        }

        foreach (var levelItem in levelItems)
        {
            LevelItemData levelItemData = new LevelItemData(levelItem.PoolID, levelItem.transform.localPosition, levelItem.transform.rotation, levelItem.transform.localScale);
            levelItemDatas.Add(levelItemData);
        }

        levelData.DepositItemDatas = depositItemDatas;
        levelData.LevelItemDatas = levelItemDatas;
    }
}
