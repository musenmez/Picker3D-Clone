using Picker3D.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Picker3D.Interfaces
{
    public interface IPanel
    {
        PanelID PanelID { get; }
        bool IsPanelOpen { get; }     
        void ShowPanel();
        void HidePanel();    
    }
}

