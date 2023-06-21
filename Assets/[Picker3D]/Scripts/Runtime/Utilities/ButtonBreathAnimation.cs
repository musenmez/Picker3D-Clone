using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.UI 
{
    public class ButtonBreathAnimation : MonoBehaviour
    {
        [SerializeField] private Transform body;
        [Space(10)]
        [SerializeField] private float duration = 0.9f;
        [SerializeField] private float multiplier = 0.9f;
        [SerializeField] private Ease ease = Ease.InOutSine;

        private Vector3 _defaultScale;
        private Tween _breathTween;

        private void Awake()
        {
            _defaultScale = body.transform.localScale;
            StartBreathAnimation();
        }

        private void StartBreathAnimation()
        {
            _breathTween.Kill();
            body.transform.localScale = _defaultScale * multiplier;
            _breathTween = body.transform.DOScale(_defaultScale, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo).SetLink(body.gameObject);
        }
    }

}
