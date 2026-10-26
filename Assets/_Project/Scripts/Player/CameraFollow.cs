using UnityEngine;

namespace LanternTrail.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target & Offset")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 9.5f, -8.0f);
        [SerializeField] private float smoothTime = 0.25f;
        [SerializeField] private float cameraPitch = 50.0f;

        [Header("Playable Area Boundaries")]
        [SerializeField] private bool useBounds = true;
        [SerializeField] private Vector2 minBounds = new Vector2(-25f, -25f);
        [SerializeField] private Vector2 maxBounds = new Vector2(25f, 25f);

        private Vector3 currentVelocity = Vector3.zero;

        private void Start()
        {
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }

            transform.rotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;

            if (useBounds)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
                desiredPosition.z = Mathf.Clamp(desiredPosition.z, minBounds.y, maxBounds.y);
            }

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void OnDrawGizmosSelected()
        {
            if (useBounds)
            {
                Gizmos.color = Color.green;
                Vector3 center = new Vector3((minBounds.x + maxBounds.x) * 0.5f, 0f, (minBounds.y + maxBounds.y) * 0.5f);
                Vector3 size = new Vector3(maxBounds.x - minBounds.x, 2f, maxBounds.y - minBounds.y);
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
