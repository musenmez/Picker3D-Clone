using DG.Tweening;
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

        private const float PUNCH_STRENGTH = 0.3f;
        private const float PUNCH_DURATION = 0.3f;
        private const Ease PUNCH_EASE = Ease.InOutSine;

        private Tween _punchTween;

        public override void Initialize()
        {
            base.Initialize();
            SetColor(emptyColor);
        }

        public void SetFill() 
        {
            SetColor(fillColor);
            PunchSegment();
        }

        private void SetColor(Color color) 
        {
            segmentImage.color = color;
        }

        private void PunchSegment() 
        {
            _punchTween?.Kill();
            _punchTween = transform.DOPunchScale(Vector3.one * PUNCH_STRENGTH, PUNCH_DURATION, 1, 1).SetEase(PUNCH_EASE);
        }
    }
}
