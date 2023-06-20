using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Picker3D.EditorSystem 
{
    public class PlayerPrefsMenuItem
    {
        [MenuItem("Picker3D/Clear PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}

