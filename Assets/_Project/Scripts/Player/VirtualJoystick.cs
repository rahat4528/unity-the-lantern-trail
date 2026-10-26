using UnityEngine;
using UnityEngine.EventSystems;

namespace LanternTrail.Player
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public static VirtualJoystick Instance { get; private set; }

        [Header("UI Rects")]
        [SerializeField] private RectTransform backgroundRect;
        [SerializeField] private RectTransform handleRect;

        [Header("Configuration")]
        [SerializeField] private float handleRange = 60f;
        [SerializeField] private bool isLeftHanded = false;

        private Vector2 inputVector = Vector2.zero;
        private Canvas parentCanvas;

        public Vector2 Direction => inputVector;
        public float Horizontal => inputVector.x;
        public float Vertical => inputVector.y;

        private void Awake()
        {
            Instance = this;
            if (backgroundRect == null) backgroundRect = GetComponent<RectTransform>();
            if (handleRect == null && backgroundRect.childCount > 0)
                handleRect = backgroundRect.GetChild(0).GetComponent<RectTransform>();

            parentCanvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                backgroundRect, eventData.position, parentCanvas.worldCamera, out Vector2 localPoint))
            {
                localPoint = Vector2.ClampMagnitude(localPoint, handleRange);
                inputVector = localPoint / handleRange;
                if (handleRect != null)
                {
                    handleRect.anchoredPosition = localPoint;
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputVector = Vector2.zero;
            if (handleRect != null)
            {
                handleRect.anchoredPosition = Vector2.zero;
            }
        }

        public void SetLeftHanded(bool leftHanded)
        {
            isLeftHanded = leftHanded;
            if (backgroundRect != null)
            {
                // Toggle between bottom-left and bottom-right anchors
                if (isLeftHanded)
                {
                    backgroundRect.anchorMin = new Vector2(1f, 0f);
                    backgroundRect.anchorMax = new Vector2(1f, 0f);
                    backgroundRect.pivot = new Vector2(1f, 0f);
                    backgroundRect.anchoredPosition = new Vector2(-60f, 60f);
                }
                else
                {
                    backgroundRect.anchorMin = new Vector2(0f, 0f);
                    backgroundRect.anchorMax = new Vector2(0f, 0f);
                    backgroundRect.pivot = new Vector2(0f, 0f);
                    backgroundRect.anchoredPosition = new Vector2(60f, 60f);
                }
            }
        }

        public void SetJoystickScale(float scaleMultiplier)
        {
            float scale = Mathf.Clamp(scaleMultiplier, 0.7f, 1.4f);
            if (backgroundRect != null)
            {
                backgroundRect.localScale = Vector3.one * scale;
            }
        }
    }
}
