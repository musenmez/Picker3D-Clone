using Picker3D.Enums;
using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picker3D.Interfaces;

namespace Picker3D.Managers 
{
    public class UIManager : Singleton<UIManager>
    {
        public Dictionary<PanelID, IPanel> PanelsByID { get; private set; } = new Dictionary<PanelID, IPanel>();

        public void ShowPanel(PanelID panelID)
        {
            if (!PanelsByID.ContainsKey(panelID))
                return;

            PanelsByID[panelID].ShowPanel();
        }

        public void HidePanel(PanelID panelID)
        {
            if (!PanelsByID.ContainsKey(panelID))
                return;

            PanelsByID[panelID].HidePanel();
        }

        public void HideAllPanels()
        {
            foreach (var panel in PanelsByID.Values)
            {
                panel.HidePanel();
            }
        }

        public void AddPanel(IPanel panel)
        {
            if (PanelsByID.ContainsKey(panel.PanelID))
                return;

            PanelsByID.Add(panel.PanelID, panel);
        }

        public void RemovePanel(IPanel panel)
        {
            if (!PanelsByID.ContainsKey(panel.PanelID))
                return;

            PanelsByID.Remove(panel.PanelID);
        }
    }
}

