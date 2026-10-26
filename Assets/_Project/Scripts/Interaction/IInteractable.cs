using UnityEngine;
using LanternTrail.Player;

namespace LanternTrail.Interaction
{
    public interface IInteractable
    {
        string GetPromptText();
        bool CanInteract();
        void Interact(InteractionDetector detector);
        Vector3 GetWorldPosition();
    }
}
