/*
 * manage captions
 */

using UnityEngine;

namespace XRAccess.Chirp
{
    [RequireComponent(typeof(CaptionRenderManager))]
    public class CaptionSystem : MonoBehaviour
    {
        public static CaptionSystem Instance;

        public Camera mainCamera;
        public AudioListener mainAudioListener;

        public CaptionOptions options;

        private void Awake()
        {
            Instance = this;
        }
    }
}
