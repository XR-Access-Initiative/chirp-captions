/*
 * CaptionSource is attached to a GameObject for playing captions from it.
 */

using UnityEngine;

namespace XRAccess.Chirp
{
    public class CaptionSource : MonoBehaviour
    {
        public AudioSource audioSource;
        public GameObject boundingObject;
        public string sourceLabel;

        private void InitCaptionSource() { }

        /// <summary>
        /// Method to play a caption from the caption source.
        /// </summary> 
        /// <param name="captionText">The text content of the caption.</param>
        /// <param name="duration">The duration of the caption in seconds.</param>
        public void ShowTimedCaption(string captionText, float duration)
        {
            TimedCaption caption = new TimedCaption();
            caption.startTime = Time.time;
            caption.captionText = captionText;
            caption.duration = duration;
            caption.audioSource = audioSource;
            caption.boundingObject = boundingObject;

            CaptionRenderManager.Instance.AddTimedCaption(caption);
        }

        private void Reset()
        {
            audioSource = this.GetComponent<AudioSource>();
            boundingObject = this.gameObject;
        }
    }
}