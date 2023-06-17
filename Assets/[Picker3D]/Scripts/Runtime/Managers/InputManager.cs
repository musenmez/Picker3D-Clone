using Picker3D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Picker3D.Managers 
{
    public class InputManager : Singleton<InputManager>
    {
        [HideInInspector]
        public UnityEvent OnTouched = new UnityEvent();

        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject())
                return;

            foreach (var touch in Input.touches)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;
            }

            if (Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
            {
                OnTouched.Invoke();
            }
        }
    }
}

