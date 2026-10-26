using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Player;
using LanternTrail.UI;
using LanternTrail.Audio;

namespace LanternTrail.Interaction
{
    public class Sign : MonoBehaviour, IInteractable
    {
        [Header("Lore Identity")]
        [SerializeField] private string noteId = "L1_Note_01";
        [SerializeField] private string noteTitle = "The Wanderer's Path";
        [TextArea(3, 8)]
        [SerializeField] private string noteBody = "I left this lantern here so others might find their way. The light in the darkness is never wasted — it guides every soul that follows.";

        public string GetPromptText() => "Read Note";

        public bool CanInteract() => true;

        public Vector3 GetWorldPosition() => transform.position;

        public void Interact(InteractionDetector detector)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddJournalNote(noteId, noteTitle, noteBody);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayNoteRead();
            }

            if (HUDController.Instance != null)
            {
                HUDController.Instance.DisplayNoteModal(noteTitle, noteBody);
            }
        }
    }
}
