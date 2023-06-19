using Picker3D.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Runtime 
{
    public class FinishLine : InteractableBase
    {
        public override void Interact(Player player)
        {
            if (IsInteracted)
                return;

            base.Interact(player);
            PlayerManager.Instance.OnReachedFinishLine.Invoke();
        }
    }
}

