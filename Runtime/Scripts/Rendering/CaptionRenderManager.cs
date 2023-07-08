/*
 * Render captions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRAccess.Chirp
{
    public class CaptionRenderManager : MonoBehaviour
    {
        public static CaptionRenderManager Instance;

        public GameObject HeadLockedPrefab;

        private uint nextCaptionID = 0;

        private List<(uint, Caption)> _currentCaptions = new List<(uint, Caption)>();
        private GameObject _currentRendererObj;

        public List<(uint, Caption)> currentCaptions
        {
            get { return _currentCaptions; }
        }

        public CaptionRenderer currentRenderer
        {
            get
            {
                if (_currentRendererObj != null)
                {
                    return _currentRendererObj.GetComponent<CaptionRenderer>();
                }
                else
                {
                    return null;
                }
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (CaptionSystem.Instance.options.enableCaptions)
            {
                EnableRenderer(CaptionSystem.Instance.options.positioningMode);
            }
        }

        private uint GenerateCaptionID()
        {
            return nextCaptionID++;
        }

        public void ClearCaptions()
        {
            _currentCaptions.Clear();
            RefreshCaptions();
        }

        public void RefreshCaptions()
        {
            if (CaptionSystem.Instance.options.enableCaptions == false)
            {
                DestroyCurrentRenderer();
                return;
            }

            if (_currentRendererObj == null) { return; }

            currentRenderer.RefreshCaptions(_currentCaptions);
        }

        public void AddTimedCaption(TimedCaption caption)
        {
            if (_currentRendererObj == null) { return; }

            uint newID = GenerateCaptionID();
            _currentCaptions.Add((newID, caption));
            currentRenderer.AddCaption(newID, caption);

            float extendedDuration = caption.duration + CaptionSystem.Instance.options.extendDuration;

            StartCoroutine(RemoveAfterDuration(newID, extendedDuration));
        }

        private IEnumerator RemoveAfterDuration(uint captionID, float duration)
        {
            yield return new WaitForSeconds(duration);

            if (currentRenderer != null) { currentRenderer.RemoveCaption(captionID); }
            _currentCaptions.RemoveAll(caption => caption.Item1 == captionID);

        }

        public void EnableRenderer(PositioningMode mode)
        {
            DestroyCurrentRenderer();

            switch (mode)
            {
                case PositioningMode.HeadLocked:
                    _currentRendererObj = Instantiate(HeadLockedPrefab, CaptionSystem.Instance.transform);
                    break;
                default:
                    break;
            }
        }

        public void DestroyCurrentRenderer()
        {
            if (_currentRendererObj != null)
            {
                Destroy(_currentRendererObj);
                _currentRendererObj = null;
            }
        }
    }
}
