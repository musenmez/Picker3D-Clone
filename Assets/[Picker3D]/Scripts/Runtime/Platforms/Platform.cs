using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer platformRenderer;
        [SerializeField] protected List<MeshRenderer> borderRenderes;
      
        public virtual Vector3 GetMaxPosition() 
        {
            Vector3 position = transform.position;
            position.z = platformRenderer.bounds.max.z;          
            return position;
        }
        
        public virtual Vector3 GetMinPosition()
        {
            Vector3 position = transform.position;
            position.z = platformRenderer.bounds.min.z;           
            return position;
        }       

        public virtual void SetGroundMaterial(Material groundMaterial) 
        {
            if (groundMaterial == null)
                return;

            platformRenderer.material = groundMaterial;
        }

        public virtual void SetBorderMaterial(Material borderMaterial) 
        {
            if (borderMaterial == null)
                return;

            foreach (MeshRenderer borderRenderer in borderRenderes)
            {
                borderRenderer.material = borderMaterial;
            }
        }
    }
}

