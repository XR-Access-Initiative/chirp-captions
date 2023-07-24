/*
 * Main script to add caption support and access options
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
