using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private MeshRenderer platformRenderer;
        [SerializeField] private List<MeshRenderer> borderRenderes;

        public Vector3 GetMaxPosition() 
        {
            Vector3 position = transform.position;
            position.z = platformRenderer.bounds.max.z;
            return position;
        }

        public Vector3 GetMinPosition()
        {
            Vector3 position = transform.position;
            position.z = platformRenderer.bounds.min.z;
            return position;
        }

        public void SetMaterials(Material groundMaterial, Material borderMaterial) 
        {
            platformRenderer.material = groundMaterial;
            foreach (MeshRenderer borderRenderer in borderRenderes)
            {
                borderRenderer.material = borderMaterial;
            }
        }
    }
}

