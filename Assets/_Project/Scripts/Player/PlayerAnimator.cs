using UnityEngine;

namespace LanternTrail.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMover mover;
        [SerializeField] private Transform modelTransform;
        [SerializeField] private Animator animator;

        [Header("Procedural Bob & Tilt Settings")]
        [SerializeField] private float bobFrequency = 10f;
        [SerializeField] private float bobAmplitude = 0.08f;
        [SerializeField] private float tiltAmount = 6f;
        [SerializeField] private float returnSpeed = 8f;

        private Vector3 initialLocalPos;
        private Quaternion initialLocalRot;
        private float bobTimer = 0f;

        private void Start()
        {
            if (mover == null) mover = GetComponent<PlayerMover>();
            if (modelTransform == null) modelTransform = transform;

            initialLocalPos = modelTransform.localPosition;
            initialLocalRot = modelTransform.localRotation;
        }

        private void Update()
        {
            if (mover == null) return;

            // If an Animator exists, set parameters
            if (animator != null)
            {
                animator.SetBool("IsMoving", mover.IsMoving);
                animator.SetFloat("Speed", mover.Velocity.magnitude);
            }
            else
            {
                // Procedural bobbing and tilt for low-poly aesthetics
                ApplyProceduralAnimation();
            }
        }

        private void ApplyProceduralAnimation()
        {
            if (mover.IsMoving && mover.IsGrounded)
            {
                bobTimer += Time.deltaTime * bobFrequency;
                float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
                float tiltAngle = Mathf.Cos(bobTimer * 0.5f) * tiltAmount;

                modelTransform.localPosition = initialLocalPos + new Vector3(0f, bobOffset, 0f);
                modelTransform.localRotation = initialLocalRot * Quaternion.Euler(tiltAmount * 0.5f, 0f, tiltAngle);
            }
            else
            {
                bobTimer = 0f;
                modelTransform.localPosition = Vector3.Lerp(modelTransform.localPosition, initialLocalPos, Time.deltaTime * returnSpeed);
                modelTransform.localRotation = Quaternion.Slerp(modelTransform.localRotation, initialLocalRot, Time.deltaTime * returnSpeed);
            }
        }
    }
}
