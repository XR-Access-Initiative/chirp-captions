/*
 * Script responsible for managing the active caption renderer.
 * CaptionSource components communicate with this script to
 * display captions using the active caption renderer.
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

        /// <summary>
        /// A list of currently active captions in the system.
        /// </summary>
        public List<(uint, Caption)> currentCaptions
        {
            get { return _currentCaptions; }
        }

        /// <summary>
        /// Currently active caption renderer component.
        /// </summary>
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

        /// <summary>
        /// Clears all current captions from both the captions list and the active caption renderer.
        /// </summary>
        public void ClearCaptions()
        {
            _currentCaptions.Clear();
            RefreshCaptions();
        }

        /// <summary>
        /// Forces the active caption renderer to refresh its captions based on the current captions list.
        /// </summary>
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

        /// <summary>
        /// Method to display a timed caption in the caption system.
        /// </summary>
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

        /// <summary>
        /// Method to switch the caption system to the specified caption renderer.
        /// </summary>
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

        /// <summary>
        /// Method to destroy the current caption renderer, effectively turning captions off.
        /// </summary>
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
