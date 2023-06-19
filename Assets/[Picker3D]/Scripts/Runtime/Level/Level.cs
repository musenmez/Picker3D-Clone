using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Picker3D.Runtime;
using UnityEngine.Events;

namespace Picker3D.LevelSystem 
{
    public class Level : MonoBehaviour
    {
        private List<Platform> _platforms;
        private List<Platform> Platforms { get => _platforms == null ? _platforms = GetComponentsInChildren<Platform>().ToList() : _platforms; set => _platforms = value; } 
        public Transform PlayerStartPoint => playerStartPoint;
        public UnityEvent OnInitialized { get; } = new UnityEvent();

        [SerializeField] private Transform playerStartPoint;

        private void Awake()
        {
            SortPlatforms();
        }

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

