using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(RandomBrushSet))]
    public class RandomBrushSetEditor : BrushSetEditor<SingleBrushBinding>
    {
        public override BrushSet<SingleBrushBinding> brushSet
        {
            get
            {
                return (RandomBrushSet) target;
            }
        }
    }
}
