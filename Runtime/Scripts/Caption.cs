using UnityEngine;

namespace XRAccess.Chirp
{
    public abstract class Caption
    {
        public float startTime;
        public AudioSource audioSource;
        public GameObject boundingObject;
    }

    public class TimedCaption : Caption
    {
        public string captionText;
        public float duration;
    }

    public class RealtimeCaption : Caption
    {
        public string partialCaption;
    }
}