using Picker3D.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySwerveMovement : MonoBehaviour
{
    private Camera _camera;
    protected Camera MainCamera => _camera == null ? _camera = Camera.main : _camera;

    private Rigidbody _rigidbody;
    protected Rigidbody Rigidbody => _rigidbody == null ? _rigidbody = GetComponent<Rigidbody>() : _rigidbody;

    public Vector2 FingerPosition { get; protected set; }
    public Vector2 ScreenDelta { get; protected set; }
    public bool IsFingerDown { get; private set; }
    public bool IsSwerveEnabled { get; protected set; } = true;
    public bool IsForwardMovementEnabled { get; protected set; } = true;

    [SerializeField] protected MovementData movementData;

    protected const float SCREEN_DELTA_OFFSET = 100f;

    protected virtual void Update()
    {
        CheckInput();       
    }

    protected virtual void FixedUpdate() 
    {
        Translate();
    }

    protected virtual void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
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
            IsFingerDown = false;
            ScreenDelta = Vector2.zero;
        }
    }

    protected virtual void Translate() 
    {
        Vector3 targetPosition = Rigidbody.position;

        targetPosition += GetForwardAmount();
        targetPosition += GetSwerveAmount();

        Rigidbody.MovePosition(ClampPosition(targetPosition));
        FingerPosition = Input.mousePosition;
    }

    protected virtual Vector3 GetForwardAmount() 
    {
        Vector3 forward = IsForwardMovementEnabled ? Time.fixedDeltaTime * movementData.MovementSpeed * Vector3.forward : Vector3.zero;
        return forward;
    }

    protected virtual Vector3 GetSwerveAmount() 
    {
        if (!IsFingerDown || !IsSwerveEnabled)
            return Vector3.zero;

        // Screen position of the transform
        Vector3 screenPoint = MainCamera.WorldToScreenPoint(Rigidbody.position);

        // Add the deltaPosition
        screenPoint += (Vector3)ScreenDelta * movementData.Sensitivity;

        // Convert back to world space
        Vector3 targetPosition = MainCamera.ScreenToWorldPoint(screenPoint);
        Vector3 swerveAmount = Vector3.Scale(targetPosition - Rigidbody.position, Vector3.right);
        
        return swerveAmount;      
    }

    protected virtual Vector2 GetScreenDelta()
    {
        Vector2 delta = IsFingerDown ? (Vector2)Input.mousePosition - FingerPosition : Vector2.zero;
        delta = ClampScreenDelta(delta);
        return delta;
    }

    protected virtual Vector2 ClampScreenDelta(Vector2 screenDelta) 
    {
        screenDelta.x = Mathf.Clamp(screenDelta.x, -SCREEN_DELTA_OFFSET / 2f, SCREEN_DELTA_OFFSET / 2f);
        return screenDelta;
    }

    protected virtual Vector3 ClampPosition(Vector3 position)
    {
        float border = movementData.MovementWidth / 2f;
        position.x = Mathf.Clamp(position.x, -border, border);
        return position;
    }   
}
