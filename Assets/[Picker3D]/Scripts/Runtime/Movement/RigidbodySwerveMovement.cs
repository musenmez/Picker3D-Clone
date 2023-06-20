using Picker3D.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Picker3D.Movements 
{
    public class RigidbodySwerveMovement : MonoBehaviour
    {
        private Camera _camera;
        protected Camera MainCamera => _camera == null ? _camera = Camera.main : _camera;

        private Rigidbody _rigidbody;
        protected Rigidbody Rigidbody => _rigidbody == null ? _rigidbody = GetComponent<Rigidbody>() : _rigidbody;

        public MovementData MovementData => movementData;
        public Vector2 FingerPosition { get; protected set; }
        public Vector2 ScreenDelta { get; protected set; }
        public bool IsFingerDown { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        public virtual bool IsSwerveEnabled { get; protected set; }
        public virtual bool IsForwardMovementEnabled { get; protected set; }

        [SerializeField] private MovementData movementData;

        protected const float MIN_SWERVE_AMOUNT = -0.5f;
        protected const float MAX_SWERVE_AMOUNT = 0.5f;

        protected virtual void Update()
        {
            CheckInput();
        }

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        protected virtual void CheckInput()
        {
            if (!IsActive)
                return;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                IsFingerDown = true;
                FingerPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                ScreenDelta = GetScreenDelta();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Release();
            }
        }

        protected virtual void Movement()
        {
            if (!IsActive)
                return;

            Vector3 targetPosition = Rigidbody.position;

            targetPosition += GetForwardAmount();
            targetPosition += GetSwerveAmount();

            Rigidbody.MovePosition(ClampPosition(targetPosition));

            if (IsFingerDown)
            {
                FingerPosition = Input.mousePosition;
            }
        }

        protected virtual Vector3 GetForwardAmount()
        {
            Vector3 forward = IsForwardMovementEnabled ? Time.fixedDeltaTime * MovementData.MovementSpeed * Vector3.forward : Vector3.zero;
            return forward;
        }

        protected virtual Vector3 GetSwerveAmount()
        {
            if (!IsFingerDown || !IsSwerveEnabled)
                return Vector3.zero;

            // Screen position of the transform
            Vector3 screenPoint = MainCamera.WorldToScreenPoint(Rigidbody.position);

            // Add the deltaPosition
            screenPoint += (Vector3)ScreenDelta * MovementData.Sensitivity;

            // Convert back to world space
            Vector3 targetPosition = MainCamera.ScreenToWorldPoint(screenPoint);

            Vector3 swerveAmount = Vector3.Scale(targetPosition - Rigidbody.position, Vector3.right);
            swerveAmount.x = Mathf.Clamp(swerveAmount.x, MIN_SWERVE_AMOUNT, MAX_SWERVE_AMOUNT);

            return swerveAmount;
        }

        protected virtual Vector2 GetScreenDelta()
        {
            Vector2 delta = IsFingerDown ? (Vector2)Input.mousePosition - FingerPosition : Vector2.zero;         
            return delta;
        }        

        protected virtual Vector3 ClampPosition(Vector3 position)
        {
            float border = MovementData.MovementWidth / 2f;
            position.x = Mathf.Clamp(position.x, -border, border);
            return position;
        }

        protected virtual void Release() 
        {
            IsFingerDown = false;
            ScreenDelta = Vector2.zero;
        }
    }
}

