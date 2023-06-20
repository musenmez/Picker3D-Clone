using Picker3D.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class PlayerInteractor : MonoBehaviour
    {
        private Player _player;
        private Player Player => _player == null ? _player = GetComponentInParent<Player>() : _player;

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(Player);
            }
        }
    }
}

