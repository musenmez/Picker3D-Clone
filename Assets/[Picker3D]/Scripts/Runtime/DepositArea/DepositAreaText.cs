using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Picker3D.Runtime
{
    public class DepositAreaText : MonoBehaviour
    {
        private DepositArea _depositArea;
        private DepositArea DepositArea => _depositArea == null ? _depositArea = GetComponentInParent<DepositArea>() : _depositArea;

        [SerializeField] private TextMeshPro depositTextMesh;

        private readonly WaitForSeconds UpdateDelay = new WaitForSeconds(0.07f);

        private Coroutine _updateCoroutine;
        private int _currentCollectableAmount = 0;

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            DepositArea.OnCollectableAmountChanged.AddListener(UpdateText);            
        }

        private void OnDisable() 
        {
            DepositArea.OnCollectableAmountChanged.RemoveListener(UpdateText);           
        }

        //Also Listens Unity Editor Event
        public void Initialize() 
        {
            _currentCollectableAmount = 0;
            SetText();
        }

        //Also Listens Unity Editor Event
        public void SetText()
        {
            string text = _currentCollectableAmount + "/" + DepositArea.RequiredCollectable;
            depositTextMesh.SetText(text);
        }

        private void UpdateText() 
        {
            if (_updateCoroutine != null)
                StopCoroutine(_updateCoroutine);

            _updateCoroutine = StartCoroutine(UpdateCoroutine());
        }

        private IEnumerator UpdateCoroutine() 
        {
            while (true) 
            {
                if (_currentCollectableAmount >= DepositArea.CurrentCollectableAmount)
                    yield break;

                _currentCollectableAmount++;
                SetText();
                yield return UpdateDelay;
            }
        }        
    }
}
