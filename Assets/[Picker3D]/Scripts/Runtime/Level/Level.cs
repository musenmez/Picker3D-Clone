using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Picker3D.Runtime
{
    public class Level : MonoBehaviour
    {
        private List<Platform> _platforms;
        private List<Platform> Platforms
        {
            get
            {
                if (_platforms == null || _platforms.Count == 0)
                {
                    _platforms = GetComponentsInChildren<Platform>().ToList();
                    SortPlatforms();
                }
                return _platforms;
            }
            set 
            {
                _platforms = value;
            } 
        } 
        public Transform PlayerStartPoint => playerStartPoint;
        public UnityEvent OnInitialized { get; } = new UnityEvent();

        [SerializeField] private Transform playerStartPoint;        

        public void Initialize() 
        {
            OnInitialized.Invoke();
        }

        public Vector3 GetMaxPosition() 
        {
            Platform platform = Platforms[Platforms.Count - 1];          
            return platform.GetMaxPosition();
        }

        public Vector3 GetMinPosition() 
        {            
            Platform platform = Platforms[0];            
            return platform.GetMinPosition();
        }

        private void SortPlatforms() 
        {
            Platforms = Platforms.OrderBy(platform => platform.transform.position.z).ToList();
        }
    }
}

