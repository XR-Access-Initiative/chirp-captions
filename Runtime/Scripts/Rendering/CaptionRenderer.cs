using System.Collections.Generic;
using UnityEngine;

namespace XRAccess.Chirp
{
    public abstract class CaptionRenderer : MonoBehaviour
    {
        public abstract RendererOptions options { get; set; }

        public abstract void AddCaption(uint captionID, Caption caption);
        public abstract void RemoveCaption(uint captionID);
        public abstract void RefreshCaptions(List<(uint, Caption)> currentCaptions);
    }
}