using System.Collections;
using UnityEngine;

namespace XRAccess.Chirp
{
    public static class AnimationHelpers
    {
        public static IEnumerator AnimateMove(Transform transform, Vector3 startPos, Vector3 endPos, float duration)
        {
            float elapsedTime = 0f;

            while (transform != null && elapsedTime <= duration)
            {
                elapsedTime += Time.deltaTime;
                float percent = Mathf.Clamp01(elapsedTime / duration);

                transform.localPosition = Vector3.Lerp(startPos, endPos, percent);

                yield return null;
            }
        }
    }
}
