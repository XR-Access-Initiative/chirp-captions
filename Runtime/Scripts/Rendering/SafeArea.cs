/*
 * Safe area is the area within the user's vision where captions can be safely displayed.
 * Captions displayed outside the safe area might cause eye strain or be clipped off.
 * 
 * Safe area can be thought of as a smaller frustum within the main camera's frustum.
 * It is defined as percentages of the camera's horizontal (X) and vertical (Y) field of views.
 */

using UnityEngine;

namespace XRAccess.Chirp
{
    public class SafeArea : MonoBehaviour
    {
        public GameObject safeAreaVisualPrefab;
        [Range(0f, 100f)] public float safeAreaXPercent;
        [Range(0f, 100f)] public float safeAreaYPercent;
        public bool showSafeAreaVisual = false;
        public float visualDistance; // distance at which to show safe area visual

        private Camera _mainCamera;
        private GameObject _safeAreaVisual;

        public void Init()
        {
            _mainCamera = CaptionSystem.Instance.mainCamera;

            _safeAreaVisual = Instantiate(safeAreaVisualPrefab, this.transform);
            _safeAreaVisual.transform.localPosition = new Vector3(0f, 0f, visualDistance);
            _safeAreaVisual.SetActive(false);
        }

        private void Update()
        {
            if (showSafeAreaVisual)
            {
                if (!_safeAreaVisual.activeSelf) { _safeAreaVisual.SetActive(true); };
                var SafeAreaSize = GetSize(visualDistance);
                _safeAreaVisual.transform.localScale = new Vector3(SafeAreaSize.x, SafeAreaSize.y, 1f);
                _safeAreaVisual.transform.localPosition = new Vector3(0f, 0f, visualDistance);
            }
            else
            {
                if (_safeAreaVisual.activeSelf) { _safeAreaVisual.SetActive(false); };
            }
        }

        /// <summary>
        /// Get width and height of safe area, given a distance from the camera.
        /// </summary>
        /// <param name="distance">Distance from the camera at which to calculate safe area size.</param>
        public Vector2 GetSize(float distance)
        {
            Vector2 fov = GetAngles();

            float width = 2.0f * distance * Mathf.Tan(fov.x * 0.5f * Mathf.Deg2Rad);
            float height = 2.0f * distance * Mathf.Tan(fov.y * 0.5f * Mathf.Deg2Rad);

            return new Vector2(width, height);
        }

        /// <summary>
        /// Get horizontal and vertical angles of safe area.
        /// </summary>
        public Vector2 GetAngles()
        {
            float verticalAngle = _mainCamera.fieldOfView * (safeAreaYPercent / 100f);
            float horizontalAngle = Camera.VerticalToHorizontalFieldOfView(_mainCamera.fieldOfView, _mainCamera.aspect) * (safeAreaXPercent / 100f);

            return new Vector2(horizontalAngle, verticalAngle);
        }
    }
}
