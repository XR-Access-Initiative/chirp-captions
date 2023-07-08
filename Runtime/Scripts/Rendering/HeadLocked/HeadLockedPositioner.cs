using System.Collections;
using UnityEngine;
using TMPro;

namespace XRAccess.Chirp
{
    public class HeadLockedPositioner : MonoBehaviour
    {
        private Camera _mainCamera;
        private HeadLockedRenderer _renderer;
        private HeadLockedOptions _rendererOptions;
        private SafeArea _safeArea;

        public void Init()
        {
            _mainCamera = CaptionSystem.Instance.mainCamera;
            _renderer = (HeadLockedRenderer)CaptionRenderManager.Instance.currentRenderer;
            _rendererOptions = (HeadLockedOptions)_renderer.options;
            _safeArea = transform.parent.GetComponent<SafeArea>();
        }

        private float CalculateYOffset(int captionIndex)
        {
            float yOffset = 0f;

            // move offset to bottom edge of safe area
            var SafeAreaSize = _safeArea.GetSize(_rendererOptions.defaultCaptionDistance);
            yOffset -= SafeAreaSize.y / 2f;

            // move offset upwards for each succeeding caption
            for (int i = _renderer.captionObjectMap.Count - 1; i >= captionIndex; i--)
            {
                var text = _renderer.captionObjectMap[i].Item2.GetComponentInChildren<TMP_Text>();
                yOffset += text.preferredHeight * _rendererOptions.canvasScale;
                if (i != captionIndex && !_renderer.isSubsequentCaption(i)) { yOffset += _rendererOptions.captionGap; }
            }

            return yOffset;
        }

        public void PositionLastCaption()
        {
            float yOffset = 0f;

            int captionCount = _renderer.captionObjectMap.Count;

            if (captionCount == 0)
            {
                return;
            }
            else if (captionCount == 1)
            {
                yOffset = CalculateYOffset(0);

                // reserve space for new caption to appear
                if (_rendererOptions.reservedSpaceLines > 0)
                {
                    TMP_Text captionTMP = _renderer.captionObjectMap[0].Item2.GetComponentInChildren<TMP_Text>();
                    var faceInfo = captionTMP.font.faceInfo;

                    float lineHeight = (faceInfo.lineHeight / faceInfo.pointSize) * captionTMP.fontSize * _rendererOptions.canvasScale;
                    yOffset += (_rendererOptions.reservedSpaceLines * lineHeight) + _rendererOptions.captionGap;
                }
            }
            else if (captionCount > 1) // if previous caption exists, position last caption below that
            {
                GameObject previousCaptionObj = _renderer.captionObjectMap[captionCount - 2].Item2; // get second last caption
                TMP_Text previousCaptionTMP = previousCaptionObj.GetComponentInChildren<TMP_Text>();
                yOffset = previousCaptionObj.transform.localPosition.y - (previousCaptionTMP.preferredHeight * _rendererOptions.canvasScale);

                // is caption is from same source as previous caption, do not add a gap between them
                if (!_renderer.isSubsequentCaption(captionCount - 1))
                {
                    yOffset -= _rendererOptions.captionGap;
                }
            }

            var lastCaptionObj = _renderer.captionObjectMap[captionCount - 1].Item2;
            lastCaptionObj.transform.localPosition = new Vector3(0f, yOffset, 0f);

            StartCoroutine(RepositionCaptions(_rendererOptions.repositionDelay));
        }

        public IEnumerator RepositionCaptions(float delay, int fromIndex = 0)
        {
            yield return new WaitForSeconds(delay);

            for (int i = fromIndex; i < _renderer.captionObjectMap.Count; i++)
            {
                float yOffset = CalculateYOffset(i);
                var captionObj = _renderer.captionObjectMap[i].Item2;
                var endPos = new Vector3(0f, yOffset, captionObj.transform.localPosition.z);
                MoveCaption(captionObj.transform, endPos, _rendererOptions.captionMoveDuration);
            }
        }

        private void MoveCaption(Transform target, Vector3 endPos, float duration)
        {
            // only move captions upwards, never downwards
            // to prevent unnecessary shifting
            if (endPos.y < target.localPosition.y)
            {
                return;
            }

            if (CaptionSystem.Instance.options.reducedMotion)
            {
                target.localPosition = endPos;
            }
            else
            {
                StartCoroutine(AnimationHelpers.AnimateMove(target, target.localPosition, endPos, duration));
            }
        }
    }
}