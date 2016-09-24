using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    public abstract class Brush : ScriptableObject
    {
        public abstract SingleBrush GetBrush(string tag);

        public abstract SingleBrush GetBrush(float angle);

        public abstract SingleBrush GetBrush(string tag, float angle);

        public abstract SingleBrush GetBrush();
    }
}
