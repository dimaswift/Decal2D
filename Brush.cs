using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    public abstract class Brush : ScriptableObject
    {
        public abstract SingleBrush GetBrush(string tag, int order);

        public abstract SingleBrush GetBrush(float angle, int order);

        public abstract SingleBrush GetBrush(string tag, float angle, int order);

        public abstract SingleBrush GetBrush(int order);

        public abstract SingleBrush GetBrush();

        public abstract SingleBrush GetBrushSafe();
    }
}
