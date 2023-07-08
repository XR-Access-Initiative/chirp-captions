using UnityEngine;
using TMPro;

namespace XRAccess.Chirp
{
    public class IndicatorArrowsController : MonoBehaviour
    {
        public Transform leftArrow;
        public Transform rightArrow;
        public float arrowGap;
        public float scaleFactor;

        public AudioSource audioSource;

        private Camera _mainCamera;
        private TMP_Text _TMPText;
        private Vector2 _safeAreaAngles;

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        }

        private void Awake()
        {
            _TMPText = transform.parent.GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _safeAreaAngles = CaptionRenderManager.Instance.currentRenderer.GetComponent<SafeArea>().GetAngles();
            _mainCamera = CaptionSystem.Instance.mainCamera;
        }

        private void Update()
        {
            if (audioSource != null)
            {
                Vector3 target = audioSource.transform.position;
                Vector3 relativeTarget = _mainCamera.transform.InverseTransformPoint(target);

                float angle = Mathf.Atan2(relativeTarget.x, relativeTarget.z) * Mathf.Rad2Deg;

                bool isLeftActive = leftArrow.gameObject.activeSelf;
                bool isRightActive = rightArrow.gameObject.activeSelf;

                if (Mathf.Abs(angle) <= _safeAreaAngles.x / 2) // show no arrows if object is in fov
                {
                    if (isLeftActive) { leftArrow.gameObject.SetActive(false); }
                    if (isRightActive) { rightArrow.gameObject.SetActive(false); }
                }
                else if (angle > 0) // show only right arrow
                {
                    if (isLeftActive) { leftArrow.gameObject.SetActive(false); }
                    if (!isRightActive) { rightArrow.gameObject.SetActive(true); }
                }
                else // show only left arrow
                {
                    if (isRightActive) { rightArrow.gameObject.SetActive(false); }
                    if (!isLeftActive) { leftArrow.gameObject.SetActive(true); }
                }
            }
        }

        private void OnTextChanged(UnityEngine.Object obj)
        {
            if (obj != _TMPText)
            {
                return;
            }

            // TODO: support left aligned and right aligned text in the future
            var textSize = _TMPText.GetRenderedValues(true);
            float xOffset = (textSize.x / 2) + (arrowGap * _TMPText.fontSize * scaleFactor);
            float yOffset = textSize.y / 2;

            // position the arrows vertically centred relative to text
            leftArrow.localPosition = new Vector3(-xOffset, -yOffset, 0f);
            rightArrow.localPosition = new Vector3(xOffset, -yOffset, 0f);

            var relativeScale = Vector3.one * _TMPText.fontSize * scaleFactor;
            leftArrow.localScale = relativeScale;
            rightArrow.localScale = relativeScale;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_TMPText != null) { OnTextChanged(_TMPText); }
        }
#endif
    }
}
