using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Picker3D.Utilities 
{
    public class ButtonPunch : MonoBehaviour
    {
        private Button _button;
        public Button Button => _button == null ? _button = GetComponent<Button>() : _button;

        [SerializeField] private Transform body;
        [Space(10)]
        [SerializeField] private Ease punchEase = Ease.InOutSine;
        [SerializeField] private float punchStrength = 0.1f;
        [SerializeField] private float pucnhDuration = 0.25f;

        private Tween _punchTween;       

        private void OnEnable()
        {
            if (Button == null) return;
            Button.onClick.AddListener(AnimateButton);
        }

        private void OnDisable()
        {
            if (Button == null) return;
            Button.onClick.RemoveListener(AnimateButton);
        }

        private void AnimateButton()
        {
            _punchTween?.Kill();
            _punchTween = body.DOPunchScale(Vector2.one * punchStrength, pucnhDuration, 1, 1).SetEase(punchEase).SetLink(body.gameObject);
        }
    }
}

