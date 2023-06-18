using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Picker3D.UI;

namespace Picker3D.EditorSystem 
{
    [CustomEditor(typeof(PanelBase), true)]
    public class PanelBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PanelBase panelBase = (PanelBase)target;
            if (GUILayout.Button("Toggle Panel"))
            {
                TogglePanel(panelBase);
            }
        }

        private void TogglePanel(PanelBase panelBase) 
        {
            if (panelBase.CanvasGroup.interactable)
                panelBase.SetCanvasGroup(false);
            else
                panelBase.SetCanvasGroup(true);
        }
    }
}

