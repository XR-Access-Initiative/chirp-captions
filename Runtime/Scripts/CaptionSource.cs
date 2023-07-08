/*
 * Init and manage captions source
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