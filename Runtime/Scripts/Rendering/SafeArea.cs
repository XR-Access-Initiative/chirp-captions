using UnityEngine;

namespace XRAccess.Chirp
{
    public class SafeArea : MonoBehaviour
    {
        public GameObject safeAreaVisualPrefab;
        [Range(0f, 100f)] public float safeAreaXPercent;
        [Range(0f, 100f)] public float safeAreaYPercent;
        public bool showSafeAreaVisual = false;
        public float visualDistance;

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

        public Vector2 GetSize(float distance)
        {
            Vector2 fov = GetAngles();

            float width = 2.0f * distance * Mathf.Tan(fov.x * 0.5f * Mathf.Deg2Rad);
            float height = 2.0f * distance * Mathf.Tan(fov.y * 0.5f * Mathf.Deg2Rad);

            return new Vector2(width, height);
        }

        public Vector2 GetAngles()
        {
            float verticalAngle = _mainCamera.fieldOfView * (safeAreaYPercent / 100f);
            float horizontalAngle = Camera.VerticalToHorizontalFieldOfView(_mainCamera.fieldOfView, _mainCamera.aspect) * (safeAreaXPercent / 100f);

            return new Vector2(horizontalAngle, verticalAngle);
        }
    }
}
