using UnityEngine;
using LanternTrail.Core;
using LanternTrail.Audio;

namespace LanternTrail.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float rotationSpeed = 14.0f;
        [SerializeField] private float gravity = 15.0f;
        [SerializeField] private float stepOffset = 0.3f;
        [SerializeField] private float slopeLimit = 45f;

        [Header("Footsteps")]
        [SerializeField] private float footstepInterval = 0.45f;
        private float footstepTimer = 0f;

        private CharacterController controller;
        private Vector3 moveDirection;
        private float verticalVelocity = 0f;

        public Vector3 Velocity => controller.velocity;
        public bool IsGrounded => controller.isGrounded;
        public bool IsMoving => new Vector2(controller.velocity.x, controller.velocity.z).sqrMagnitude > 0.05f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            controller.stepOffset = stepOffset;
            controller.slopeLimit = slopeLimit;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
            {
                return;
            }

            Vector2 input = GatherInput();
            HandleMovement(input);
            HandleFootsteps();
        }

        private Vector2 GatherInput()
        {
            // Virtual Joystick on mobile
            Vector2 joystick = VirtualJoystick.Instance != null ? VirtualJoystick.Instance.Direction : Vector2.zero;

            // Keyboard input fallback
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 keyboard = new Vector2(h, v);

            return joystick.sqrMagnitude > 0.05f ? joystick : keyboard.normalized;
        }

        private void HandleMovement(Vector2 input)
        {
            // Convert 2D input into 3D isometric plane movement (fixed camera angled 45 degrees)
            Vector3 targetDir = new Vector3(input.x, 0f, input.y).normalized;

            if (controller.isGrounded)
            {
                verticalVelocity = -0.5f; // Keep grounded firmly
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            Vector3 finalMove = (targetDir * moveSpeed) + (Vector3.up * verticalVelocity);
            controller.Move(finalMove * Time.deltaTime);

            // Rotate towards movement direction smoothly
            if (targetDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
            }
        }

        private void HandleFootsteps()
        {
            if (!IsMoving || !controller.isGrounded)
            {
                footstepTimer = 0f;
                return;
            }

            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0f;
                PlayFootstepSound();
            }
        }

        private void PlayFootstepSound()
        {
            // Check surface under player via raycast
            string surfaceTag = "Grass";
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, 1.2f))
            {
                surfaceTag = hit.collider.tag;
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayFootstep(surfaceTag);
            }
        }
    }
}
