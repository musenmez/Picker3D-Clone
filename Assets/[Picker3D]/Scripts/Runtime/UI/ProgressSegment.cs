using Picker3D.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Picker3D.UI 
{
    public class ProgressSegment : PoolObject
    {
        [Header("Progress Segment")]
        [SerializeField] private Image segmentImage;
        [Space]
        [SerializeField] private Color emptyColor;
        [SerializeField] private Color fillColor;    

        public override void Initialize()
        {
            base.Initialize();
            SetColor(emptyColor);
        }

        public void SetFill() 
        {
            SetColor(fillColor);
        }

        private void SetColor(Color color) 
        {
            segmentImage.color = color;
        }
    }
}
