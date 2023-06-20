﻿using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Managers
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public static int CurrenyAmount
        {
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.CurrencyAmount, 0);
            }
            private set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrencyAmount, value);
            }
        }

        public UnityEvent OnCurrencyAmountChanged { get; } = new UnityEvent();

        public void AddCurrency(int amount) 
        {
            CurrenyAmount += amount;
            OnCurrencyAmountChanged.Invoke();
        }
    }
}