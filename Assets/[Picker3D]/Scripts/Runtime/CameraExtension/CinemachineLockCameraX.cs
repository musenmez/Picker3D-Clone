using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.CameraExtensions 
{
    using UnityEngine;
    using Cinemachine;
    
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineLockCameraX : CinemachineExtension
    {
        [Tooltip("Lock the camera's X position to this value")]
        [SerializeField] private float position;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Finalize)
            {
                var pos = state.RawPosition;
                pos.x = position;
                state.RawPosition = pos;
            }
        }
    }
}

