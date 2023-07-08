using UnityEngine;

namespace XRAccess.Chirp
{
    public static class DistanceHelpers
    {
        private static float GetVolumeAfterRolloff(AudioSource audioSource)
        {
            float dist = GetAudioSourceDistance(audioSource);

            if (dist > audioSource.maxDistance)
            {
                return 0.0f;
            }
            else
            {
                float clampedDist = Mathf.Clamp(dist, audioSource.minDistance, audioSource.maxDistance);
                AnimationCurve curve = audioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
                float volume = curve.Evaluate(clampedDist);

                return volume;
            }
        }

        private static float GetAudioSourceDistance(AudioSource audioSource)
        {
            Vector3 sourcePos = audioSource.transform.position;
            Vector3 listenerPos = CaptionSystem.Instance.mainAudioListener.transform.position;

            float dist = Vector3.Distance(sourcePos, listenerPos);

            return dist;
        }
    }
}